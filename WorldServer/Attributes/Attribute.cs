using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using WorldServer.Network;
using Microsoft.Xna.Framework;
using SharedCode.Attributes;

namespace WorldServer.Attributes
{
    public class Attribute
    {
        public string Key;
        public long OwnerID;
        public object Data;
        public AttributeTypeID Type;
        public bool SyncToAll = true;
        public bool IsDirty = false;

        public void SerializeAttribute(NetOutgoingMessage Message) {
            Message.Write(Key);
            Message.Write((byte)Type);
            WriteType(Message, Type, Data);
        }
        
        public void WriteType(NetOutgoingMessage Message, AttributeTypeID Type, object Data) {
            switch (Type) { 
                case AttributeTypeID.Float:
                    Message.Write((float)Data);
                    break;
                case AttributeTypeID.Int:
                    Message.Write((int)Data);
                    break;
                case AttributeTypeID.List:
                    Console.WriteLine("Engine unable to serialize list objects");
                    break;
                case AttributeTypeID.Long:
                    Message.Write((long)Data);
                    break;
                case AttributeTypeID.Rectangle:
                    Message.WriteRectangle((Rectangle)Data);
                    break;
                case AttributeTypeID.String:
                    Message.Write((string)Data);
                    break;
                case AttributeTypeID.Vector2:
                    Message.WriteVector2((Vector2)Data);
                    break;
                case AttributeTypeID.Bool:
                    Message.Write((bool)Data);
                    break;
                default:
                    Console.WriteLine("AttributeSystem Unrecognised Type In AttributeSystem Type: {0}", Type.ToString());
                    break;
            }
        }
    }
}
