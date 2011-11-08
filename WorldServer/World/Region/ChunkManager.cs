using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Collections;
using SharedCode.TileSystem;

namespace WorldServer.Region
{
    public class ChunkManager
    {
        private static int ChunkWidth = 40 * 25;
        private static int ChunkHeight = 40 * 25;
        private static Dictionary<Vector2, WorldChunk> LoadedChunks = new Dictionary<Vector2, WorldChunk>();



        public static Vector2 North = new Vector2(0, -1);
        public static Vector2 NorthEast = new Vector2(1, -1);
        public static Vector2 East = new Vector2(1, 0);
        public static Vector2 SouthEast = new Vector2(1, 1);
        public static Vector2 South = new Vector2(0, 1);
        public static Vector2 SouthWest = new Vector2(-1, 1);
        public static Vector2 West = new Vector2(-1, 0);
        public static Vector2 NorthWest = new Vector2(-1, -1);
       
        public static List<WorldChunk> GetLocalChunks(WorldChunk CurrentChunk) {
            Vector2 Location = new Vector2(CurrentChunk.BoundingBox.X,
                CurrentChunk.BoundingBox.Y);
            List<WorldChunk> RetVal = new List<WorldChunk>();
            RetVal.Add(CurrentChunk);
            if (LoadedChunks.ContainsKey(Location + North))
                RetVal.Add(LoadedChunks[Location + North]);
            if (LoadedChunks.ContainsKey(Location + NorthEast))
                RetVal.Add(LoadedChunks[Location + NorthEast]);
            if (LoadedChunks.ContainsKey(Location + East))
                RetVal.Add(LoadedChunks[Location + East]);
            if (LoadedChunks.ContainsKey(Location + SouthEast))
                RetVal.Add(LoadedChunks[Location + SouthEast]);
            if (LoadedChunks.ContainsKey(Location + South))
                RetVal.Add(LoadedChunks[Location + South]);
            if (LoadedChunks.ContainsKey(Location + SouthWest))
                RetVal.Add(LoadedChunks[Location + SouthWest]);
            if (LoadedChunks.ContainsKey(Location + West))
                RetVal.Add(LoadedChunks[Location + West]);
            if (LoadedChunks.ContainsKey(Location + NorthWest))
                RetVal.Add(LoadedChunks[Location + NorthWest]);
            return RetVal;
        }

        public static WorldChunk GetChunk(Vector2 WorldLocation) {
            Vector2 ChunkVector = new Vector2();
            ChunkVector.X = (int)WorldLocation.X / ChunkWidth;
            ChunkVector.Y = (int)WorldLocation.Y / ChunkHeight;

            if (LoadedChunks.ContainsKey(ChunkVector))
                return LoadedChunks[ChunkVector];

            LoadedChunks.Add(ChunkVector, new WorldChunk(new Rectangle(
                (int)ChunkVector.X,
                (int)ChunkVector.Y,
                ChunkWidth,
                ChunkHeight)));

            return LoadedChunks[ChunkVector];
        }
    }
}
