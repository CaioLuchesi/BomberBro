using System;

namespace BomberBro
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (BomberBroGame game = new BomberBroGame())
            {
                game.Run();
            }
        }
    }
}

