using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorldServer.Character;
using WorldServer.Objects;
using WorldServer.Attributes;
using WorldServer.World;
using Microsoft.Xna.Framework;
using Lidgren.Network;
using System.Collections;
using WorldServer.Network;
using SharedCode.Network;

namespace WorldServer.Tasks.TaskImplimentations
{
    public class WorldDataSync
    {
        Vector2 PlayerSpawnPoint = new Vector2(750 * 40, 750 * 40);
        NetConnection Connect;
        public WorldDataSync(NetConnection Connection) {
            Connect = Connection;

            NetOutgoingMessage Message = NetworkManager.Server.CreateMessage();
            Message.Write((byte)MessageTypes.ConnectionOkay);
            Connection.SendMessage(Message, NetDeliveryMethod.ReliableUnordered, 0);


            NetOutgoingMessage NameRequest = NetworkManager.Server.CreateMessage();
            Message.Write((byte)MessageTypes.RequestPlayerName);
            Connection.SendMessage(NameRequest, NetDeliveryMethod.ReliableUnordered, 0);
        }

        public IEnumerator RunInitial() {
            CharacterManager.CreateController(Connect);
            GameServer.TaskScheduler.AddTask(CharacterManager.BroadCastLocalCharacterState(Connect));
            GameServer.TaskScheduler.AddTask(ObjectManager.SendObjectState(Connect));
            GameServer.TaskScheduler.AddTask(AttributeManager.SyncFullStateToConnection(Connect));
            GameServer.TaskScheduler.AddTask(TerrainManager.SyncTerrainTo(Connect, PlayerSpawnPoint));
            yield return TimeSpan.FromSeconds(60);
            NetOutgoingMessage Message = NetworkManager.Server.CreateMessage();
            Message.Write((byte)MessageTypes.ChatMessage);
            Message.Write("MOTD: Welcome to the sobriety alpha");
            NetworkManager.Server.SendMessage(Message, Connect, NetDeliveryMethod.ReliableUnordered);
            yield return false;
        }

        public IEnumerator RunUpdate() {
            PlayerClient Client = CharacterManager.GetByConnection(Connect);
            GameServer.TaskScheduler.AddTask(TerrainManager.SyncTerrainTo(Connect, Client.Location));
            GameServer.TaskScheduler.AddTask(ObjectManager.SendObjectState(Connect).GetEnumerator());
            yield return false;
        }
    }
}
