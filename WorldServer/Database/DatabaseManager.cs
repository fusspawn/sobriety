#define Local
//#define EC2
//#define Dedi

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MongoDB;
using MongoDB.Linq;


namespace WorldServer.Database
{
    public class DatabaseManager  {
        private static Mongo DbConnection;
        private static Dictionary<String, IMongoDatabase> LoadedDBConnections;

        public static void Init() {
            LoadedDBConnections = new Dictionary<String, IMongoDatabase>();

            DbConnection = new Mongo();
            DbConnection.Connect();

            RunTests();
        }

        private static void RunTests()
        {
            var Collection = GetCollection("test", "foo");
            var Cursor = Collection.FindAll();

            foreach (Document Doc in Cursor.Documents) {
                Console.WriteLine("Found {0}", Doc.ToString());
                Collection.Remove(Doc);
            }
        }

        public static IMongoDatabase GetDatabase(string Database) {
            if (!LoadedDBConnections.ContainsKey(Database))
                LoadedDBConnections.Add(Database, DbConnection.GetDatabase(Database));

            return LoadedDBConnections[Database];
        }

        public static IMongoCollection GetCollection(string Database, string Key) {
            return GetDatabase(Database).GetCollection(Key);
        }
    }
}
