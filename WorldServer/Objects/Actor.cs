using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WorldServer.Objects
{
    public class Actor
    {
        public Vector2 Location;
        public long ID;

        public Actor() {
            Location = Vector2.Zero;
            ID = ObjectIDFactory.GetNewID();
        }
    }
}
