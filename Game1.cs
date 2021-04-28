using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace CardFlip
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private BasicEffect basicEffect;
        private VertexPositionTexture[] _vertexPositionTextures;
        private Texture2D cardTexture;
        private Texture2D cardBackTexture;
        private List<List<Vector2>> animation;
        private float currentFrame;
        private float currentFrameTime;
        private float frameRate;
        private float extraFrames;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            cardTexture = Content.Load<Texture2D>("card_front");
            cardBackTexture = Content.Load<Texture2D>("card_back");

            // Load animation.
            animation = new List<List<Vector2>>();
            string[] lines = System.IO.File.ReadAllLines(@"Content/flip_anim.txt");
            foreach (string line in lines)
            {
                Console.WriteLine(line);
                string[] points = line.Split(" ");
                Vector2 bottomLeft = new Vector2(Int32.Parse(points[0]), Int32.Parse(points[1]));
                Vector2 bottomRight = new Vector2(Int32.Parse(points[2]), Int32.Parse(points[3]));
                Vector2 topLeft = new Vector2(Int32.Parse(points[4]), Int32.Parse(points[5]));
                Vector2 topRight = new Vector2(Int32.Parse(points[6]), Int32.Parse(points[7]));

                animation.Add(new List<Vector2>());
                animation[animation.Count - 1].Add(topLeft);
                animation[animation.Count - 1].Add(topRight);
                animation[animation.Count - 1].Add(bottomLeft);
                animation[animation.Count - 1].Add(bottomRight);
            }
            extraFrames = 1;

            currentFrame = 0;
            currentFrameTime = 0;
            frameRate = 1f / 24f;// / (float)extraFrames;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // It boggles me, but without this, it doesn't work. The BasicEffect has to be recreated every frame.
            basicEffect = new BasicEffect(GraphicsDevice);
            basicEffect.World = Matrix.CreateOrthographicOffCenter(0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 0, 0, 1);
            basicEffect.TextureEnabled = true;
            basicEffect.Texture = cardTexture;
            GraphicsDevice.RasterizerState = RasterizerState.CullNone;


            if (gameTime.IsRunningSlowly)
                Console.WriteLine("Running slowly.");

            // Set the animation frame.
            var origin = new Vector2(100, 100);
            var scale = new Vector2(0.5f, 0.5f);

            // Interpolate.
            int prevAnimFrame = (int)MathF.Floor(currentFrame);
            int nextAnimFrame = (int)MathF.Ceiling(currentFrame);
            float distance = currentFrame - prevAnimFrame;
            var prevFrame = animation[prevAnimFrame];
            var nextFrame = animation[nextAnimFrame];

            var topLeft = new Vector2((1f - distance) * prevFrame[0].X + distance * nextFrame[0].X,
                                      (1f - distance) * prevFrame[0].Y + distance * nextFrame[0].Y) * scale + origin;
            var topRight = new Vector2((1f - distance) * prevFrame[1].X + distance * nextFrame[1].X,
                                      (1f - distance) * prevFrame[1].Y + distance * nextFrame[1].Y) * scale + origin;
            var bottomLeft = new Vector2((1f - distance) * prevFrame[2].X + distance * nextFrame[2].X,
                                         (1f - distance) * prevFrame[2].Y + distance * nextFrame[2].Y) * scale + origin;
            var bottomRight = new Vector2((1f - distance) * prevFrame[3].X + distance * nextFrame[3].X,
                                          (1f - distance) * prevFrame[3].Y + distance * nextFrame[3].Y) * scale + origin;
            _vertexPositionTextures = new[]
            {
                // For TriangleStrips the order needs to be top left, top right, bottom left, bottom right.
                new VertexPositionTexture(new Vector3(topLeft.X, topLeft.Y, 0), new Vector2(0, 0)),
                new VertexPositionTexture(new Vector3(topRight.X, topRight.Y, 0), new Vector2(1, 0)),
                new VertexPositionTexture(new Vector3(bottomLeft.X, bottomLeft.Y, 0), new Vector2(0, 1)),
                new VertexPositionTexture(new Vector3(bottomRight.X, bottomRight.Y, 0), new Vector2(1, 1)),
            };

            if (currentFrameTime > frameRate)
            {
                currentFrame += 1.0f / (float)extraFrames;
                if (currentFrame > animation.Count - 1)
                {
                    currentFrame = 0;
                }
                currentFrameTime = 0;
            }
            currentFrameTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Draw the backside of the card if we are flipped over.
            Vector3 topVector = new Vector3(topRight - topLeft, 0);
            Vector3 sideVector = new Vector3(bottomLeft - topLeft, 0);
            Vector3 normal = Vector3.Cross(topVector, sideVector);
            if (normal.Z < 0)
            {
                basicEffect.Texture = cardBackTexture;
            }

            GraphicsDevice.Clear(Color.CornflowerBlue);

            EffectTechnique effectTechnique = basicEffect.Techniques[0];
            EffectPassCollection effectPassCollection = effectTechnique.Passes;
            foreach (EffectPass pass in effectPassCollection)
            {
                pass.Apply();
                GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, _vertexPositionTextures, 0, 2);
            }

            base.Draw(gameTime);
        }
    }
}
