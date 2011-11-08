using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.Graphics
{
    class Texture2DExtentions
    {
        public static Texture2D FromColorData(GraphicsDevice Device, Color[] Data) {
            Texture2D retTex = new Texture2D(Device, 1, 1);
            retTex.SetData<Color>(Data);
            return retTex;
        }
    }
}
