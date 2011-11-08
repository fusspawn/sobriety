using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using Client.World;

namespace Client.Attributes
{
    public static class AttributeManager
    {
        static Dictionary<long, AttributeList> KnownAttributes = new Dictionary<long, AttributeList>();
        
        public static void UpdateAttributes(NetIncomingMessage Packet) {
            var Count = 0;
            var Max_Count = Packet.ReadInt32();
            while(Count < Max_Count) {
                var OwnerID = Packet.ReadInt64();
                if (!KnownAttributes.ContainsKey(OwnerID))
                    KnownAttributes.Add(OwnerID, new AttributeList(OwnerID));
                var ListCount = 0;
                var List_MaxCount = Packet.ReadInt32();
                while (ListCount < List_MaxCount) {
                    Attribute Attr = new Attribute();
                    Attr.Deserialize(Packet, OwnerID);
                    if (!KnownAttributes[OwnerID].HasKey(Attr.Key))
                        KnownAttributes[OwnerID].Create(Attr);
                    else {
                        KnownAttributes[OwnerID].Set(Attr.Key, Attr.Data);
                    }
                    ActorManager.DrawableActors[OwnerID].OnAttributeUpdate(Attr.Key, Attr.Data);
                    ListCount++;
                }
                Count++;
            }
        }
    }
}
