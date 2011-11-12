using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SharedCode.TileSystem;
using WorldServer.Objects;
using WorldServer.Database;
using MongoDB;
using WorldServer.World;

namespace WorldServer.Region
{
    public class WorldChunk
    {
        public Rectangle BoundingBox;
        public List<TerrainTile> GroundTiles;
        public List<GameObject> Objects;

        public WorldChunk(Rectangle _BoundingBox) {
            BoundingBox = _BoundingBox;
            GroundTiles = new List<TerrainTile>();
            Objects = new List<GameObject>();
        }

        public void Insert(TerrainTile Tile) {
            GroundTiles.Add(Tile);
        }

        public void Insert(GameObject Object) {
            Objects.Add(Object);
        }

        public bool IsPointInChunk(Vector2 Location) {
            if (new Rectangle((int)Location.X, (int)Location.Y, 1, 1).Intersects(BoundingBox))
                return true;
            return false;
        }

        public void LoadFromDB() {
            int X_Min = (BoundingBox.X * BoundingBox.Width) / 40;
            int X_Max = ((BoundingBox.X * BoundingBox.Width) + BoundingBox.Width) / 40;
            int Y_Min = (BoundingBox.Y * BoundingBox.Height) / 40;
            int Y_Max = ((BoundingBox.Y * BoundingBox.Height) + BoundingBox.Height) / 40;

            var Tiles = from Tile in DatabaseManager.GetCollection("test", "world_terrain").Linq()
                        where (int)Tile["x"] >= X_Min && (int)Tile["x"]  <= X_Max &&
                      (int)Tile["y"] >= Y_Min && (int)Tile["y"] <= Y_Max
                        select Tile;

            GroundTiles = new List<TerrainTile>();
            foreach (Document D in Tiles) {
                GroundTiles.Add(TerrainManager.TileFromDocument(D));
            }

            Console.WriteLine("Loaded {0} tiles from db for fresh chunk", GroundTiles.Count);
        }
    }
}
