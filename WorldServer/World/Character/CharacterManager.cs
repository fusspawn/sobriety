using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorldServer.Objects;
using Lidgren.Network;
using WorldServer.Network;
using SharedCode.Network;
using SharedCode.Objects;
using System.Collections;

namespace WorldServer.Character
{
    public class CharacterManager
    {
        public static Dictionary<long, PlayerClient> ConnectedControllers = new Dictionary<long, PlayerClient>();
        public static Dictionary<NetConnection, PlayerClient> ConnectionMap = new Dictionary<NetConnection, PlayerClient>();

        public static PlayerClient CreateController(NetConnection Connection) {
            var Controller = new PlayerClient(Connection);
            ConnectedControllers.Add(Controller.ID, Controller);
            ConnectionMap.Add(Connection, Controller);
            BroadCastPlayerCreation(Controller);
            SetClientCameraFollow(Controller.ID, Connection);
            return Controller; 
        }

        private static void SetClientCameraFollow(long ID, NetConnection Connection)
        {
            NetOutgoingMessage Message = NetworkManager.Server.CreateMessage();
            Message.Write((byte)MessageTypes.CameraFocusAt);
            Message.Write(ID);
            NetworkManager.Server.SendMessage(Message, Connection, NetDeliveryMethod.ReliableUnordered);
        }

        private static void BroadCastPlayerCreation(PlayerClient Controller)
        {
            NetOutgoingMessage Message = NetworkManager.Server.CreateMessage();
            Message.Write((byte)MessageTypes.ActorCreate);
            Message.Write((int)ObjectTypes.PlayerActor);
            Message.Write(Controller.ID);
            Message.Write(Controller.Sex);
            Message.WriteVector2(Controller.Location);
            NetworkManager.Server.SendToAll(Message, NetDeliveryMethod.ReliableUnordered);
        }

        public static IEnumerator BroadCastLocalCharacterState(NetConnection Connection) {
            yield return null;
            int SentActors = 0;

            foreach(PlayerClient Client in ConnectedControllers.Values) {
                if (SentActors > 10)
                {
                    yield return null;
                    SentActors = 0;
                }

                SentActors++;
                NetOutgoingMessage Message = NetworkManager.Server.CreateMessage();
                Message.Write((byte)MessageTypes.ActorCreate);
                Message.Write((int)ObjectTypes.PlayerActor);
                Message.Write(Client.ID);
                Message.Write(Client.Sex);
                Message.WriteVector2(Client.Location);
                NetworkManager.Server.SendMessage(Message, Connection, NetDeliveryMethod.ReliableUnordered);
            }
        }

        public static PlayerClient GetByConnection(NetConnection Connection) {
            if (ConnectionMap.ContainsKey(Connection))
                return ConnectionMap[Connection];

            throw new Exception("No Character Found for Connection");
            return default(PlayerClient);    
        }
    }
}
