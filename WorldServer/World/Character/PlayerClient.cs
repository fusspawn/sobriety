using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharedCode.Objects;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using WorldServer.Network;
using SharedCode.Network;
using WorldServer.World;
using WorldServer.Region;
using WorldServer.Character;
using WorldServer.Tasks.TaskImplimentations;

namespace WorldServer.Objects
{
    public class PlayerClient : Actor
    {
        public string Name = "{Unknown}";
        public NetConnection ClientConnection;
        public Vector2 LastDirection = IdleDirection;
        public byte Sex = 0;
        public static byte LASTSEX = 0;
        public static byte LastSex {
            get
            {
                if (LASTSEX == 0)
                {
                    LASTSEX = 1;
                    return 1;
                }
                else
                {
                    LASTSEX = 0;
                    return 0;
                }
            }
        }

        public PlayerClient(NetConnection Connection)
            : base() {
                Location = new Vector2((375 * 40) + 1, (375 * 40) + 1);
                ClientConnection = Connection;
                Sex = LastSex;
        }

        public static Vector2 NorthDirection = new Vector2(0, -1);
        public static Vector2 EastDirection = new Vector2(1, 0);
        public static Vector2 SouthDirection = new Vector2(0, 1);
        public static Vector2 WestDirection = new Vector2(-1, 0);
        public static Vector2 IdleDirection = new Vector2(0, 0);

        public void UpdateFromMovementInput(NetIncomingMessage MovementPacket) {
            Vector2 Vector = MovementPacket.ReadVector2();
            if (Vector != IdleDirection) { //If We Moved Update
                Location += (Vector * 3);
                NetOutgoingMessage MovementBroadCast = NetworkManager.Server.CreateMessage();
                MovementBroadCast.Write((byte)MessageTypes.ActorMovement);
                MovementBroadCast.Write(ID);
                MovementBroadCast.WriteVector2(Location);
                NetworkManager.Server.SendToAll(MovementBroadCast, NetDeliveryMethod.Unreliable);
            }

            if (Vector != LastDirection) {
                NetOutgoingMessage Message = NetworkManager.Server.CreateMessage();
                Message.Write((byte)MessageTypes.ActorAnimate);
                Message.Write(ID);
                Message.Write(GetAnimationForDirection(Vector));
                Message.Write(true);
                NetworkManager.Server.SendToAll(Message, NetDeliveryMethod.ReliableOrdered);
            }

            LastDirection = Vector;
            PostMovement(MovementPacket.SenderConnection);
        }

        private WorldChunk LastUpdateChunk;
        private void PostMovement(NetConnection netConnection)
        {
            PlayerClient Character = CharacterManager.GetByConnection(netConnection);
            if (LastUpdateChunk == null)
                LastUpdateChunk = ChunkManager.GetChunk(Character.Location);

            WorldChunk CurrentChunk = ChunkManager.GetChunk(Character.Location);
            if (LastUpdateChunk != CurrentChunk) {
                WorldDataSync Sync = new WorldDataSync(netConnection);
                GameServer.TaskScheduler.AddTask(Sync.RunUpdate());
                LastUpdateChunk = CurrentChunk;
            }
        }



        public WorldChunk GetPlayerChunk() {
            return ChunkManager.GetChunk(CharacterManager.GetByConnection(ClientConnection).Location);
        }
        private Vector2 GetTileGridLocation() 
        {
            return new Vector2((int)Location.X / 40, (int)Location.Y / 40);
        }
        public string GetAnimationForDirection(Vector2 Vec)
        {
            if (Vec == NorthDirection)
            {
                return ("WalkNorth");
            }
            if (Vec == SouthDirection)
            {
                return ("WalkSouth");
            }
            if (Vec == EastDirection)
            {
                return ("WalkEast");
            }
            if (Vec == WestDirection)
            {
                return ("WalkWest");
            }
    
            return ("WalkNorth");
        }
    }
}
