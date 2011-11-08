using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.World;

namespace Client.Attributes
{
    public class AttributeList : List<Attribute>
    {
        public AttributeList(long _OwnerID)  
            : base (){
            OwnerID = _OwnerID;
        }


        public long OwnerID;
        public T Get<T>(string Key)
        {
            var attr = (from at in this where at.Key == Key select at).SingleOrDefault();
            if (attr != null)
                return (T)attr.Data;

            throw new Exception(string.Format("Invalid attr lookup for key: {0} id: {1}", Key, OwnerID));
            return default(T);
        }

        public bool HasKey(string Key)
        {
            var attr = (from at in this where at.Key == Key select at).SingleOrDefault();
            if (attr == null)
                return false;
            return true;
        }

        public void Set(string Key, object Value) {
            var attr = (from at in this where at.Key == Key select at).SingleOrDefault();
            if (attr != null)
            {
                attr.Data = Value;
            }
        }

        public void Create(Attribute Value) {
            this.Add(Value);
        }
    }
}
