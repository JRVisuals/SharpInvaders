using System;

namespace SharpInvaders
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new Core())
                game.Run();
        }
    }
}
