﻿using System;

namespace Invaders
{
#if WINDOWS || LINUX
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            using (var game = new InvadersGame())
            {
                game.Run();
            }
        }
    }
#endif
}
