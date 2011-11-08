using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Client.Graphics
{
	public class SpriteSheet
	{
        public Texture2D Texture;
        public List<Rectangle> SpriteRects;
        public int Height = 0;
        public int Width = 0;
        public int Cols;
        public int Rows;

        public SpriteSheet(Texture2D Tex, int SpriteWidth, int SpriteHeight) {
            Texture = Tex;
            Width = SpriteWidth;
            Height = SpriteHeight;
            Cols = Tex.Width / SpriteWidth;
            Rows = Tex.Height / SpriteHeight;
            CalcSpriteRects();
        }

        public void CalcSpriteRects()
        {
            SpriteRects = new List<Rectangle>();
            for (int y = 0; y < Rows; y++)
                for (int x = 0; x < Cols; x++)
                    SpriteRects.Add(new Rectangle(x * Width, y * Height, Width, Height));

        }

        public void DrawSprite(SpriteBatch Batch, Rectangle DrawBounds, int SpriteIndex)
        {
            if(SpriteRects.Count <= SpriteIndex)
                return;

            Batch.Draw(Texture, DrawBounds, SpriteRects[SpriteIndex], Color.White);
        }
	}
}
