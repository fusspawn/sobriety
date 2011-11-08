using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SharedCode.TileSystem;
using WorldServer.Objects;

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
    }
}
