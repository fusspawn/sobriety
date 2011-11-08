using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WorldServer.Objects;
using Microsoft.Xna.Framework;
using System.Collections;
using Lidgren.Network;
using WorldServer.Attributes;
using SharedCode.Attributes;
using SharedCode.Objects;
using SharedCode.Network;
using WorldServer.Network;

namespace WorldServer.Data.GameObjects
{
    public class Rock : GameObject
    {
        DateTime LastInteract = DateTime.Now;
        public Rock(Rectangle Rect) 
            : base(Rect) {
                Interactable = true;
                AttributeManager.RegisterAttribute<string>(ID, "TextureName", "Rock", AttributeTypeID.String );
                AttributeManager.RegisterAttribute<int>(ID, "OreCount", 10, AttributeTypeID.Int);
                AttributeManager.RegisterAttribute<bool>(ID, "Interactable", Interactable, AttributeTypeID.Bool);
        }

        public override IEnumerator Interact(NetIncomingMessage Message)
        {
            if (!(DateTime.Now - LastInteract > TimeSpan.FromSeconds(3)))
                yield return false;

            LastInteract = DateTime.Now;

            if (Vector2.Distance(Character.CharacterManager.GetByConnection(Message.SenderConnection).Location, Location) > 4 * 40)
            {
                int OreCount = AttributeManager.KnownAttr[ID].Get<int>("OreCount");
                AttributeManager.SetAttribute(ID, "OreCount", OreCount--);
                UpdateState();
                yield return TimeSpan.FromSeconds(30);
                OreCount = AttributeManager.KnownAttr[ID].Get<int>("OreCount");
                AttributeManager.SetAttribute(ID, "OreCount", OreCount++);
                UpdateState();
            }

            yield return null;
        }

        private void UpdateState()
        {
            switch (AttributeManager.KnownAttr[ID].Get<int>("OreCount")) { 
                case 0:
                    AttributeManager.SetAttribute(ID, "TextureName", "RockEmpty");
                    AttributeManager.SetAttribute(ID, "Interactable", false);
                    Interactable = false;
                    break;
                case 1: case 2: case 3:
                    AttributeManager.SetAttribute(ID, "Interactable", true);
                    Interactable = true;
                    break;
                case 4: case 5: case 6:
                    AttributeManager.SetAttribute(ID, "TextureName", "RockHalfFull");
                    AttributeManager.SetAttribute(ID, "Interactable", true);
                    Interactable = true;
                    break;
                case 7: case 8: case 9: case 10:
                    AttributeManager.SetAttribute(ID, "TextureName", "RockFull");
                    AttributeManager.SetAttribute(ID, "Interactable", true);
                    Interactable = true;
                    break;
                default:
                    Console.WriteLine("Invalid OreCount");
                    break;
            }
        }

        public override void OnSpawn()
        {
            NetOutgoingMessage Message = NetworkManager.Server.CreateMessage();
            Message.Write((byte)MessageTypes.ActorCreate);
            Message.Write((int)ObjectTypes.Rock);
            Message.Write(ID);
            Message.WriteRectangle(BoundingBox);
            NetworkManager.Server.SendToAll(Message, NetDeliveryMethod.ReliableUnordered);
            base.OnSpawn();
        }

        public static void Spawn(Vector2 Location)
        {
            Rock T = new Rock(new Rectangle((int)Location.X, (int)Location.Y, 33, 29));
            ObjectManager.RegisterObject(T);
        }
    }
}
