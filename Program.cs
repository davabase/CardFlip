using System;

namespace CardFlip
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            // var tests = new Tests();
            // tests.Run();

            using (var game = new Game1())
                game.Run();
        }
    }
}
