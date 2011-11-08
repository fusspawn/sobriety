using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TileTest.TileSystem
{
    public class TileManager
    {
        public List<Tile> Tiles = new List<Tile>();

        public int TileWidth = 40;
        public int TileHeight = 40;

        public int WorldWidth = 10;
        public int WorldHeight = 10;

        public TileManager(GraphicsDevice Device, SpriteBatch Batch) {
            Random R = new Random();
            for (int x = 0; x < TileWidth; x++) {
                for (int y = 0; y < TileHeight; y++) {
                    Tiles.Add(new Tile() { Location = new Vector2(x, y), TileTex = GetTexture(new Color(0, R.Next(0, 255), 0), Device) });
                }
            }
        }

        public void Draw(GameTime Time, SpriteBatch Batch)
        {
            Batch.Begin();
            foreach (Tile T in Tiles) {
                Batch.Draw(T.TileTex, new Rectangle((int)T.Location.X * TileWidth, (int)T.Location.Y * TileHeight, TileHeight, TileWidth), Color.White);
            }
            Batch.End();
        }

        public static Texture2D GetTexture(Color Col, GraphicsDevice Device) {
            Texture2D Tex;
            Tex = new Texture2D(Device, 1, 1);
            Tex.SetData<Color>(new Color[] { Col });
            return Tex;
        }
    }
}
