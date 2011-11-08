using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lidgren.Network;
using SharedCode.Network;
using Client.World;
using Client.UI;

namespace Client.Network
{
    public class NetworkClient
    {
        NetPeerConfiguration Config;
        public NetClient ClientConnection;

        public NetworkClient(string Host, int Port) {
            Config = new NetPeerConfiguration("GameServer01");
            ClientConnection = new NetClient(Config);
            ClientConnection.Start();
            ClientConnection.Connect(Host, Port);
        }

        public void Update() {
            NetIncomingMessage msg;
            while ((msg = ClientConnection.ReadMessage()) != null)
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
                    default:
                        OnMessage(msg);
                        break;
                }

                ClientConnection.Recycle(msg);
            }
        }

        public virtual void OnMessage(NetIncomingMessage msg)
        {
            MessageTypes Type = (MessageTypes)msg.ReadByte();
            
            switch (Type) { 
                case MessageTypes.ConnectionOkay:
                    Console.WriteLine("Connected to Remote Server");
                    break;
                case MessageTypes.ChatMessage:
                    UIApi.AddChatMessage(msg.ReadString());
                    break;
                case MessageTypes.RequestPlayerName:
                    Console.WriteLine("Server Requested Our Name");
                    NetOutgoingMessage Message = ClientConnection.CreateMessage();
                    Message.Write((byte)MessageTypes.PlayerNameResponse);
                    Message.Write(GameClient.Playername);
                    ClientConnection.SendMessage(Message, NetDeliveryMethod.ReliableUnordered);
                    Console.WriteLine("Responded to Server Name Request");
                    break;
                case MessageTypes.IncomingMapData:
                    GameClient.Terrain.LoadTerrainFromNetwork(msg);
                    break;
                case MessageTypes.MapDataChange:
                    GameClient.Terrain.UpdateTerrainTiles(msg);
                    break;
                case MessageTypes.ActorCreate:
                    ActorManager.SpawnActor(msg);
                    break;
                case MessageTypes.ActorMovement:
                    ActorManager.UpdateFromPacket(msg);
                    break;
                case MessageTypes.ActorDestroy:
                    ActorManager.DrawableActors.Remove(msg.ReadInt64());
                    break;
                case MessageTypes.ActorAnimate:
                    ActorManager.UpdateAnimationFromPacket(msg);
                    break;
                case MessageTypes.CameraFocusAt:
                    GameClient.Camera.LookAtActor(ActorManager.DrawableActors[msg.ReadInt64()], true);
                    break;
                case MessageTypes.AttributeSync:
                    Attributes.AttributeManager.UpdateAttributes(msg);
                    break;
                default:
                    Console.WriteLine("Unhandled NetMessage Type: {0}", Type);
                    break;
            }
        }

        public virtual void OnConnectionStatusChange(NetIncomingMessage msg)
        {
            Console.WriteLine("Connection Status Change: {0}", (NetConnectionStatus)msg.ReadByte());
        }
    }
}
