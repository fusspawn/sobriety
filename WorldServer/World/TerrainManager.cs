using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharedCode.TileSystem;
using Microsoft.Xna.Framework;
using Lidgren.Network;
using WorldServer.Network;
using SharedCode.Network;
using System.Collections;
using WorldServer.Data.GameObjects;
using WorldServer.Objects;
using WorldServer.Attributes;
using WorldServer.Region;
using WorldServer.Character;
using WorldServer.Database;

namespace WorldServer.World
{
    public class TerrainManager
    {
        public List<TerrainTile> Terrain;
        public int TileWidth = 40;
        public int TileHeight = 40;
        
        public TerrainManager()
        {
            Terrain = new List<TerrainTile>();
            GenerateTerrain();
        }

        public virtual void GenerateTerrain()
        {
            Random R = new Random();
            var TileCol = new Vector3(0, R.Next(0, 255), 0);
            var TerrainCollection = DatabaseManager.GetCollection("test", "world_terrain");


            for (int i = 0; i < 1000; i++)
            {
                Tree.Spawn(new Vector2(R.Next(0, 40 * 1500), R.Next(0, 40 * 750)));
            }
            for (int r = 0; r < 500; r++) 
            { 
                Rock.Spawn(new Vector2(R.Next(0, 40 * 1500), R.Next(0, 40 * 750)));
            }
        }


        public static MongoDB.Document TileToDocument(TerrainTile Tile) {
            var Doc = new MongoDB.Document();
            Doc["x"] = Tile.X;
            Doc["y"] = Tile.Y;
            Doc["tile_type"] = (int)Tile.TileID;
            return Doc;
        }

        public static TerrainTile TileFromDocument(MongoDB.Document Doc) {
            return new TerrainTile()
            {
                X = (int)Doc["x"],
                Y = (int)Doc["y"],
                TileID = (byte)(int)Doc["tile_type"],
                Passable = true,
            };
        }

        public List<TerrainTile> TilesWithinDistance(Vector2 Loc, int Distance) {
            var TileList = new List<TerrainTile>();
            Console.WriteLine("Grabbing Local Terrain");
            foreach (TerrainTile T in Terrain) { 
                if(DistanceTo(Loc, new Vector2(T.X, T.Y)) <= Distance) {
                    TileList.Add(T);
                }
            }
            Console.WriteLine("Local Terrain Generated for Client");

            return TileList;
        }       
        public static float DistanceTo(Vector2 world_vect, Vector2 tile_target){
            float distance = (float)Math.Sqrt(Math.Pow(world_vect.X-(tile_target.X * 40), 2) + Math.Pow(world_vect.Y - (tile_target.Y * 40), 2));
            return distance;
        } 
        public void SetTile(int X, int Y, TerrainTile T) {
            TerrainTile Tile = (from t in Terrain where t.X == X && t.Y == Y select t).Single();
            if (Tile != null)
            {
                int Index = Terrain.IndexOf(Tile);
                Terrain[Index] = T;
            }
            else {
                Terrain.Add(Tile);
            }
            BroadcastTileUpdate(T);
            Console.WriteLine("Synced Terrain Update to ALL");
        }
        public void BroadcastTileUpdate(TerrainTile T) {
            NetOutgoingMessage Message = NetworkManager.Server.CreateMessage();
            Message.Write((byte)MessageTypes.MapDataChange);
            Message.Write(1);
            Message.Write(T.X);
            Message.Write(T.Y);
            Message.Write(T.Passable);
            NetworkManager.Server.SendToAll(Message, NetDeliveryMethod.ReliableOrdered);
        }
        public static IEnumerator SyncTerrainTo(NetConnection Connection, Vector2 Location)
        {
            Console.WriteLine("Starting Chunks Terrain Update");
            List<WorldChunk> Chunks = ChunkManager.GetLocalChunks(ChunkManager.GetChunk(Location));
            foreach (WorldChunk C in Chunks)
            {

                NetOutgoingMessage Message = NetworkManager.Server.CreateMessage();
                Message.Write((byte)MessageTypes.IncomingMapData);

                var Ter = C.GroundTiles;
                Message.Write(Ter.Count);

                int I = 0;
                foreach (TerrainTile Tile in Ter)
                {
                    Message.Write(Tile.X);
                    Message.Write(Tile.Y);
                    Message.Write(Tile.TileID);
                    Message.Write(Tile.Passable);

                    if (I > 500)
                    {
                        I = 0;
                        yield return null;
                    }

                    I++;
                }

                yield return TimeSpan.FromSeconds(0.2);
                NetworkManager.Server.SendMessage(Message, Connection, NetDeliveryMethod.ReliableOrdered);
            }

            Console.WriteLine("Finished Chunks Terrain Update");
        }
    }
}
