using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorldServer.Objects
{
    public class ObjectIDFactory
    {
        private static long CurrentID = 0;
        public static long GetNewID() {
            var ID = CurrentID;
            CurrentID++;
            return ID;
        }
    }
}
