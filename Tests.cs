using Microsoft.Xna.Framework;
using System;

namespace CardFlip
{
    public class Tests
    {
        public void Run()
        {
            Vector2 topLeft;
            Vector2 topRight;
            Vector2 bottomLeft;
            Vector2 bottomRight;
            {
                // Not flipped.
                topLeft = new Vector2(0, 0);
                topRight = new Vector2(10, 0);
                bottomLeft = new Vector2(0, 10);
                bottomRight = new Vector2(10, 10);

                bool result = ShowBack(topLeft, topRight, bottomLeft, bottomRight);
                if (result)
                    Console.WriteLine("Test 1 failed.");
                else
                    Console.WriteLine("Test 1 succeeded.");
            }
            {
                // Flipped horizontally.
                topLeft = new Vector2(10, 0);
                topRight = new Vector2(0, 0);
                bottomLeft = new Vector2(10, 10);
                bottomRight = new Vector2(0, 10);

                bool result = ShowBack(topLeft, topRight, bottomLeft, bottomRight);
                if (!result)
                    Console.WriteLine("Test 2 failed.");
                else
                    Console.WriteLine("Test 2 succeeded.");
            }
            {
                // Flipped vertically.
                topLeft = new Vector2(0, 10);
                topRight = new Vector2(10, 10);
                bottomLeft = new Vector2(0, 0);
                bottomRight = new Vector2(10, 0);

                bool result = ShowBack(topLeft, topRight, bottomLeft, bottomRight);
                if (!result)
                    Console.WriteLine("Test 3 failed.");
                else
                    Console.WriteLine("Test 3 succeeded.");
            }
            {
                // Rotated 180 degrees.
                topLeft = new Vector2(10, 10);
                topRight = new Vector2(0, 10);
                bottomLeft = new Vector2(10, 0);
                bottomRight = new Vector2(0, 0);

                bool result = ShowBack(topLeft, topRight, bottomLeft, bottomRight);
                if (result)
                    Console.WriteLine("Test 4 failed.");
                else
                    Console.WriteLine("Test 4 succeeded.");
            }
            {
                // Flipped diagonally.
                topLeft = new Vector2(10, 10);
                topRight = new Vector2(10, 0);
                bottomLeft = new Vector2(0, 10);
                bottomRight = new Vector2(0, 0);

                bool result = ShowBack(topLeft, topRight, bottomLeft, bottomRight);
                if (!result)
                    Console.WriteLine("Test 5 failed.");
                else
                    Console.WriteLine("Test 5 succeeded.");
            }
            {
                // Flipped diagonally.
                topLeft = new Vector2(10, 10);
                topRight = new Vector2(10, 0);
                bottomLeft = new Vector2(0, 10);
                bottomRight = new Vector2(0, 0);

                bool result = ShowBack(topLeft, topRight, bottomLeft, bottomRight);
                if (!result)
                    Console.WriteLine("Test 6 failed.");
                else
                    Console.WriteLine("Test 6 succeeded.");
            }
            {
                // ???
                topLeft = new Vector2(57.6666f, 206);
                topRight = new Vector2(32.8333f, 334.66f);
                bottomLeft = new Vector2(344.333f, 158.999f);
                bottomRight = new Vector2(295.333f, 280.49994f);
                // Should be back.

                bool result = ShowBack(topLeft, topRight, bottomLeft, bottomRight);
                if (!result)
                    Console.WriteLine("Test 7 failed.");
                else
                    Console.WriteLine("Test 7 succeeded.");
            }
            {
                // ???
                topLeft = new Vector2(52.8333f, 236.5f);
                topRight = new Vector2(56.666f, 367.333f);
                bottomLeft = new Vector2(320.1666f, 126.5f);
                bottomRight = new Vector2(299.1667f, 255.5f);
                // Should be back.

                bool result = ShowBack(topLeft, topRight, bottomLeft, bottomRight);
                if (!result)
                    Console.WriteLine("Test 8 failed.");
                else
                    Console.WriteLine("Test 8 succeeded.");
            }
            {
                // error.png
                topLeft = new Vector2(118.75f, 202.1f);
                topRight = new Vector2(271.55f, 104.105f);
                bottomLeft = new Vector2(115.95f, 372.05f);
                bottomRight = new Vector2(245.4f, 283.7f);
                // Should be front.

                bool result = ShowBack(topLeft, topRight, bottomLeft, bottomRight);
                if (result)
                    Console.WriteLine("Test 9 failed.");
                else
                    Console.WriteLine("Test 9 succeeded.");
            }
        }

        private bool ShowBack(Vector2 topLeft, Vector2 topRight, Vector2 bottomLeft, Vector2 bottomRight)
        {
            Vector3 topVector = new Vector3(topRight - topLeft, 0);
            Vector3 sideVector = new Vector3(bottomLeft - topLeft, 0);
            Vector3 normal = Vector3.Cross(topVector, sideVector);
            if (normal.Z < 0)
            {
                return true;
            }
            return false;
        }
    }
}