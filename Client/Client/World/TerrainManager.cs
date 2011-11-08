using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharedCode.TileSystem;
using Lidgren.Network;
using Lidgren.Network.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Client.Graphics;

namespace Client.World
{
    public class TerrainManager
    {
        public List<TerrainTile> Terrain;
        public int TileWidth = 40;
        public int TileHeight = 40;
        public SpriteSheet TerrainTiles;

        public TerrainManager() {
            Terrain = new List<TerrainTile>();
        }

        public void LoadTerrainFromNetwork(NetIncomingMessage MapDataMessage) {
            TerrainTiles = new SpriteSheet(GameClient.ContentManager.Load<Texture2D>("Terrain2"), 40, 40);
            int TerrainCount = MapDataMessage.ReadInt32();
            for (int i = 0; i < TerrainCount; i++) {
                Terrain.Add(new TerrainTile()  {
                    X = MapDataMessage.ReadInt32(),
                    Y = MapDataMessage.ReadInt32(),
                    TileID = MapDataMessage.ReadByte(),
                    Passable = MapDataMessage.ReadBoolean(),
                });
            }

            Console.WriteLine("TerrainData Loaded");
        }

        public void UpdateTerrainTiles(NetIncomingMessage MapData) {
            int TerrainCount = MapData.ReadInt32();
            for (int i = 0; i < TerrainCount; i++)
            {
                var Terr = new TerrainTile()
                {
                    X = MapData.ReadInt32(),
                    Y = MapData.ReadInt32(),
                    TileID = MapData.ReadByte(),
                    Passable = MapData.ReadBoolean(),
                };

                var T = TileAt(Terr.X, Terr.Y);
                if (T != null)
                    Terrain[Terrain.IndexOf(T)] = Terr;
                else
                    Terrain.Add(Terr);
            }

            Console.WriteLine("Synced {0} Tiles with server", TerrainCount);
        }

        public TerrainTile TileAt(int X, int Y) {
            return (from t in Terrain where t.X == X && t.Y == Y select t).Single();
        }

        private Rectangle ViewRect;
        public void Draw(GameTime time, SpriteBatch Batch) {
            Batch.Begin(SpriteSortMode.Deferred,
                null,
                null,
                null,
                null,
                null,
                GameClient.Camera.GetTransform(Batch.GraphicsDevice));

            foreach (TerrainTile T in Terrain) {
                TerrainTiles.DrawSprite(Batch,
                    new Rectangle((T.X * TileWidth),
                        T.Y * TileHeight
                        , TileWidth, TileHeight),
                    T.TileID);
            }

            Batch.End();
        }
    }
}
