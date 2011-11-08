#define CLIENT
using System;

namespace Client
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            Console.WriteLine("Please Enter your Characters Name: ");
            string PlayerName = Console.ReadLine();

            using (GameClient game = new GameClient(PlayerName))
            {
                game.Run();
            }
        }
    }
#endif
}

