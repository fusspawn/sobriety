using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharedCode.Attributes;
using Lidgren.Network;
using Lidgren.Network.Xna;

namespace Client.Attributes
{
    public class Attribute
    {
        public string Key;
        public long OwnerID;
        public object Data;
        public AttributeTypeID Type;

        public void Deserialize(NetIncomingMessage Message, long OwnerID) {
            Key = Message.ReadString();
            Type = (AttributeTypeID)Message.ReadByte();
            ReadData(Type, Message);
        }

        private void ReadData(AttributeTypeID Type, NetIncomingMessage Message)
        {
 	        switch(Type) {
                case AttributeTypeID.Float:
                    Data = Message.ReadFloat();
                    break;
                case AttributeTypeID.Int:
                    Data = Message.ReadInt32();
                    break;
                case AttributeTypeID.List:
                    Console.WriteLine("AttributeSystem: List<> Type not supported on network sync");
                    break;
                case AttributeTypeID.Long:
                    Data = Message.ReadInt64();
                    break;
                case AttributeTypeID.Rectangle:
                    Data = Message.ReadRectangle();
                    break;
                case AttributeTypeID.String:
                    Data = Message.ReadString();
                    break;
                case AttributeTypeID.Vector2:
                    Data = Message.ReadVector2();
                    break;
                case AttributeTypeID.Bool:
                    Data = Message.ReadBoolean();
                    break;
                default:
                    Console.WriteLine("Invalid Attribute Type: {0}", Type.ToString());
                    break;
            }
        }


    }
}
