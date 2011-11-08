using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorldServer.Objects;
using Microsoft.Xna.Framework;
using Lidgren.Network;
using WorldServer.Network;
using SharedCode.Network;
using SharedCode.Objects;
using System.Collections;
using WorldServer.Attributes;
using SharedCode.Attributes;
using WorldServer.Region;

namespace WorldServer.Data.GameObjects
{
    public class Tree : GameObject
    {
        public Tree(Rectangle Rect) 
            : base(Rect) {
                Interactable = true;
                AttributeManager.RegisterAttribute<string>(ID, "TextureName", "Tree", AttributeTypeID.String);
                AttributeManager.RegisterAttribute<bool>(ID, "Interactable", true, AttributeTypeID.Bool);
        }

        public override void OnSpawn()
        {
           
            NetOutgoingMessage Message = NetworkManager.Server.CreateMessage();
            Message.Write((byte)MessageTypes.ActorCreate);
            Message.Write((int)ObjectTypes.Tree);
            Message.Write(ID);
            Message.WriteRectangle(BoundingBox);
            NetworkManager.Server.SendToAll(Message, NetDeliveryMethod.ReliableUnordered);
            base.OnSpawn();
        }

        public static void Spawn(Vector2 Location) {
            Tree T = new Tree(new Rectangle((int)Location.X, (int)Location.Y, 41, 65));
            ObjectManager.RegisterObject(T);
            
        }

        public override IEnumerator Interact(NetIncomingMessage Connection)
        {
            Console.WriteLine("Tree Interaction..");
            AttributeManager.SetAttribute(ID, "TextureName", "TreeStump");
            AttributeManager.SetAttribute(ID, "Interactable", false);
            
            yield return TimeSpan.FromSeconds(5);

            AttributeManager.SetAttribute(ID, "TextureName", "Tree");
            AttributeManager.SetAttribute(ID, "Interactable", true);
            Console.WriteLine("Tree Respawned");
        }
    }
}
