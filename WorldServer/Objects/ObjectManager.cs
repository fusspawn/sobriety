using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Lidgren.Network;
using WorldServer.Network;
using SharedCode.Network;
using SharedCode.Objects;
using Microsoft.Xna.Framework;
using WorldServer.Region;

namespace WorldServer.Objects
{
    public class ObjectManager
    {
        public static Dictionary<Int64, GameObject> LoadedObjects = new Dictionary<Int64, GameObject>();
        public static IEnumerable SendObjectState(NetConnection Connection) 
        {

            var Player = Character.CharacterManager.GetByConnection(Connection);
            List<WorldChunk> Chunks = ChunkManager.GetLocalChunks(ChunkManager.GetChunk(Player.Location));
            
            foreach (WorldChunk C in Chunks)
            {
                foreach (GameObject GO in C.Objects)
                {
                    NetOutgoingMessage Message = NetworkManager.Server.CreateMessage();
                    Message.Write((byte)MessageTypes.ActorCreate);
                    Message.Write((int)ObjectTypes.Tree);
                    Message.Write(GO.ID);
                    Message.WriteRectangle(GO.BoundingBox);
                    NetworkManager.Server.SendMessage(Message, Connection, NetDeliveryMethod.ReliableUnordered);
                }

                yield return null;
            }

            yield return null;
        }


        public static void Update() { }

        public static void OnInteract(NetIncomingMessage Packet) {
            var ID = Packet.ReadInt64();
            if (!LoadedObjects.ContainsKey(ID))   {
                Console.WriteLine("Invalid Interaction Request for ObjectID: {0}", ID);
                return;
            }

            if (!LoadedObjects[ID].Interactable)
                return;

            GameServer.TaskScheduler.AddTask(LoadedObjects[ID].Interact(Packet));
        }

        internal static void RegisterObject(GameObject T)
        {
            LoadedObjects.Add(T.ID, T);
            WorldChunk Chunk = ChunkManager.GetChunk(T.Location);
            Chunk.Insert(T);
            T.OnSpawn();
        }
    }
}
