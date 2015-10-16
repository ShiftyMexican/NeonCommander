
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna;

namespace GameName1
{

    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var game = new Game1())
                game.Run();
        }
    }

}
