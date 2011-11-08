using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Lidgren.Network;

namespace WorldServer.Network
{
    public static class NetworkExtension
    {
        public static Vector2 ReadVector2(this NetIncomingMessage Message) {
            var Vector = new Vector2();
            Vector.X = Message.ReadSingle();
            Vector.Y = Message.ReadSingle();
            return Vector;
        }

        public static void WriteVector2(this NetOutgoingMessage Message, Vector2 Vec) {
            Message.Write(Vec.X);
            Message.Write(Vec.Y);
        }

        public static Vector3 ReadVector3(this NetIncomingMessage Message)
        {
            var Vector = new Vector3();
            Vector.X = Message.ReadSingle();
            Vector.Y = Message.ReadSingle();
            Vector.Z = Message.ReadSingle();
            return Vector;
        }

        public static void WriteVector3(this NetOutgoingMessage Message, Vector3 Vec)
        {
            Message.Write(Vec.X);
            Message.Write(Vec.Y);
            Message.Write(Vec.Z);
        }

        public static void WriteRectangle(this NetOutgoingMessage Message, Rectangle Rect) {
            Message.Write(Rect.X);
            Message.Write(Rect.Y);
            Message.Write(Rect.Width);
            Message.Write(Rect.Height);
        }
    }
}
