using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharedCode.Objects;

namespace Client.Objects
{
    public class CActor : Actor
    {
        public byte Sex = 0;
        public virtual void Update(GameTime Time) { }
        public virtual void Draw(SpriteBatch Batch) { }
        public virtual void OnAttributeUpdate(String Key, Object Data) { }
    }
}
