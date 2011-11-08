using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AwesomiumSharp;
using Lidgren.Network;
using SharedCode.Network;

namespace Client.UI
{
    public class UIApi
    {
        public static WebView UIWindow;
        public static void AddChatMessage(string Message) {
            UIWindow.CallJavascriptFunction("NativeClient", "OnChatMessage", null, new JSValue[] { new JSValue(Message) });
            Console.WriteLine("Injected ChatMessage Event");
        }

        public static void SendChatMessage(Object Obj, JSCallbackEventArgs Args) {
            NetOutgoingMessage MessagePack = GameClient.Network.ClientConnection.CreateMessage();
            MessagePack.Write((byte)MessageTypes.ChatMessage);
            MessagePack.Write(Args.Arguments[0].ToString());
            GameClient.Network.ClientConnection.SendMessage(MessagePack, NetDeliveryMethod.ReliableUnordered);
            Console.WriteLine("Chat Event Sent");
        }
    }
}
