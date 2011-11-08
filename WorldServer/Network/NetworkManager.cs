using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using SharedCode.Network;
using WorldServer.Objects;
using WorldServer.Tasks.TaskImplimentations;
using WorldServer.Character;

namespace WorldServer.Network
{
    public class NetworkManager
    {
        public NetPeerConfiguration Config;
        public static NetServer Server;

        public NetworkManager(int Port, string AppID) {
            Config = new NetPeerConfiguration(AppID);
            Config.Port = Port;
            Server = new NetServer(Config);
            Server.Start();
        }

        public void PumpMessages()
        {
            NetIncomingMessage msg;
            while ((msg = Server.ReadMessage()) != null)
            {
                switch (msg.MessageType)
                {
                    case NetIncomingMessageType.VerboseDebugMessage:
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.WarningMessage:
                    case NetIncomingMessageType.ErrorMessage:
                        Console.WriteLine(msg.ReadString());
                        break;
                    case NetIncomingMessageType.StatusChanged:
                        OnConnectionStatusChange(msg);
                        break;
                    case NetIncomingMessageType.ConnectionApproval:
                        Console.WriteLine("Connection Approved");
                        msg.SenderConnection.Approve();
                        break;
                    default:
                        OnNetworkMessage(msg);
                        break;
                }
                Server.Recycle(msg);
            }
        }
        private void OnConnectionStatusChange(NetIncomingMessage msg)
        {
            NetConnectionStatus Status = (NetConnectionStatus)msg.ReadByte();
            switch (Status) { 
                case NetConnectionStatus.Connected:
                    OnPlayerConnected(msg.SenderConnection);
                    break;
                case NetConnectionStatus.Disconnected:
                    OnPlayerDisconnected(msg.SenderConnection);
                    break;
                default:
                    Console.WriteLine("Connection Status Change: {0}", Status);
                    break;
            }
        }

        private void OnPlayerDisconnected(NetConnection netConnection)
        {
            var Char = Character.CharacterManager.GetByConnection(netConnection);
            Character.CharacterManager.ConnectedControllers.Remove(Char.ID);
            Character.CharacterManager.ConnectionMap.Remove(netConnection);

            Console.WriteLine("Player Disconnected");

            NetOutgoingMessage Message = Server.CreateMessage();
            Message.Write((byte)MessageTypes.ActorDestroy);
            Message.Write(Char.ID);

            Server.SendToAll(Message, NetDeliveryMethod.ReliableUnordered);
        }

        private void OnPlayerConnected(NetConnection netConnection)
        {
            Console.WriteLine("Player Connected From: {0}", netConnection.RemoteEndpoint.Address);
            GameServer.TaskScheduler.AddTask(new WorldDataSync(netConnection).RunInitial());
        }

        private void OnNetworkMessage(NetIncomingMessage msg)
        {
            switch((MessageTypes)msg.ReadByte()) {
                case MessageTypes.PlayerMoveInput:
                    Character.CharacterManager.GetByConnection(msg.SenderConnection).UpdateFromMovementInput(msg);
                    break;
                case MessageTypes.ObjectInteract:
                    ObjectManager.OnInteract(msg);
                    break;
                case MessageTypes.PlayerNameResponse:
                    PlayerClient Client = CharacterManager.GetByConnection(msg.SenderConnection);
                    Client.Name = msg.ReadString();
                    break;
                case MessageTypes.ChatMessage:
                    NetOutgoingMessage Message = Server.CreateMessage();
                    Message.Write((byte)MessageTypes.ChatMessage);
                    Message.Write(CharacterManager.GetByConnection(msg.SenderConnection).Name + " Said: " + msg.ReadString());
                    Server.SendToAll(Message, NetDeliveryMethod.ReliableUnordered);
                    break;
                default:
                    break;
            }
        }
    }
}
