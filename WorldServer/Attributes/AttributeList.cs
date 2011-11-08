using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using SharedCode.Network;

namespace WorldServer.Attributes
{
    public class AttributeList : List<Attribute>
    {
        public long OwnerID;
        public AttributeList(long _OwnerID) 
            : base() {
            OwnerID = _OwnerID;
        }



        public T Get<T>(string Key){
            var attr = (from at in this where at.Key == Key select at).SingleOrDefault();
            if (attr != null)
                return (T)attr.Data;

            throw new Exception(string.Format("Invalid attr lookup for key: {0} id: {1}", Key, OwnerID));
            return default(T);
        }

        public bool HasKey(string Key) { 
            var attr = (from at in this where at.Key == Key select at).SingleOrDefault();
            if (attr == null)
                return false;
            return true;
        }

        public void Set(string Key, object Data) {
            var attr = (from at in this where at.Key == Key select at).Single();
            int index = this.IndexOf(attr);
            attr.Data = Data;
            attr.IsDirty = true;
            this[index] = attr;

            NetOutgoingMessage AttributeUpdate = Network.NetworkManager.Server.CreateMessage();
            AttributeUpdate.Write((byte)MessageTypes.AttributeSync);
            AttributeUpdate.Write(1);
            AttributeUpdate.Write(attr.OwnerID);
            AttributeUpdate.Write(1);
            attr.SerializeAttribute(AttributeUpdate);
            Network.NetworkManager.Server.SendToAll(AttributeUpdate, NetDeliveryMethod.ReliableOrdered);
        }
    }
}
