using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LoveSeat;

namespace WorldServer.Database
{
    public class DatabaseManager  {
        static CouchClient Client;
        static string DBHOST = "sobriety.dyndns.org";
        static int DBPORT = 8754;

        public static void Init() {
            Client = new CouchClient(DBHOST, DBPORT, "fusspawn", "trasher", false, AuthenticationType.Cookie);
        }
    }

    class TestStorage {
        public string Name;
        public bool Result;
    }
}
