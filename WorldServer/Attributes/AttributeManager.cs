using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using System.Collections;
using WorldServer.Network;
using SharedCode.Network;
using SharedCode.Attributes;

namespace WorldServer.Attributes
{
    public class AttributeManager
    {
        public static Dictionary<long, AttributeList> KnownAttr = new Dictionary<long, AttributeList>();
        public static void RegisterAttribute<T>(long OwnID, string Key, object Data, AttributeTypeID TypeID) {
            if (!KnownAttr.ContainsKey(OwnID))
                KnownAttr.Add(OwnID, new AttributeList(OwnID));
            if (!KnownAttr[OwnID].HasKey(Key))
                KnownAttr[OwnID].Add(new Attribute() { 
                    OwnerID = OwnID,
                    Key = Key,
                    Data = Data,
                    Type = TypeID,
                });
        }

        public static void SetAttribute(long OwnerID, string Key, Object Data) {
            KnownAttr[OwnerID].Set(Key, Data);
        }

        public static IEnumerator SyncFullStateToConnection(NetConnection Connection)
        {
            NetOutgoingMessage ObjectData = NetworkManager.Server.CreateMessage();
            ObjectData.Write((byte)MessageTypes.AttributeSync);
            ObjectData.Write(KnownAttr.Count);
            foreach (AttributeList ObjectAttributes in KnownAttr.Values) {
                ObjectData.Write(ObjectAttributes.OwnerID);
                ObjectData.Write(ObjectAttributes.Count);
                foreach (Attribute Attr in ObjectAttributes) {
                    Attr.SerializeAttribute(ObjectData);    
                }

                yield return null;
            }

            yield return null;
        }
    }
}
