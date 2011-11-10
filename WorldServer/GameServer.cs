using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;
using WorldServer.Objects;
using WorldServer.Database;

namespace WorldServer
{
    class GameServer
    {
        public static Network.NetworkManager NetworkManager;
        public static World.TerrainManager TerrainManager;
        public static Tasks.Scheduler TaskScheduler;
        public static bool KeepServerRunning = true;

        public static void Init() {
            Console.WriteLine("Starting GameServer Services");
            NetworkManager = new Network.NetworkManager(9000, "GameServer01");
            TaskScheduler = new Tasks.Scheduler();
            TerrainManager = new World.TerrainManager();
            Console.WriteLine("Services Started");
        } 


        static void Main(string[] args)
        {
            GameServer.Init();  

            while (KeepServerRunning) {
                NetworkManager.PumpMessages();
                ObjectManager.Update();
                TaskScheduler.Run();
                Thread.Sleep(1);
            }

            Console.WriteLine("Server Must Be Dead");
        }
    }
}
