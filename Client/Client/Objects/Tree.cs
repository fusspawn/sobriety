using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Lidgren.Network;
using Lidgren.Network.Xna;
using SharedCode.Network;

namespace Client.Objects
{
    public class Tree : CActor
    {
        Rectangle BoundingBox;
        Texture2D Texture;
        String CurrentTexPath;
        bool Interactable = true;




        public Tree(NetIncomingMessage Message) : base() {
            Texture = GameClient.ContentManager.Load<Texture2D>("Tree");
            ID = Message.ReadInt64();
            BoundingBox = Message.ReadRectangle();
        }

        public override void Draw(SpriteBatch Batch)
        {
            Batch.Draw(Texture, BoundingBox, Color.White);
           
            base.Draw(Batch);
        }

        public override void Update(GameTime Time)
        {
            if (InputManager.IsRightButtonClick()) 
            {

                if (!Interactable)
                    return;

                if (BoundingBox.Intersects(new Rectangle((int)InputManager.MouseWorldLocation().X,
                   (int)InputManager.MouseWorldLocation().Y, 2, 2)))
                {
                    DoInteract();
                }
            }

            base.Update(Time);
        }

        public override void OnAttributeUpdate(string Key, object Data)
        {
            switch (Key)
            {
                case "TextureName":
                    Texture = GameClient.ContentManager.Load<Texture2D>((string)Data);
                    BoundingBox.Height = Texture.Height;
                    BoundingBox.Width = Texture.Width;
                    break;
                case "Interactable":
                    Interactable = (bool)Data;
                    break;
                default:
                    Console.WriteLine("Invalid Attribute Key: {0}", Key);
                    break;
            }
        }

        private void DoInteract()
        {
            NetOutgoingMessage DataPacket = GameClient.Network.ClientConnection.CreateMessage();
            DataPacket.Write((byte)MessageTypes.ObjectInteract);
            DataPacket.Write(ID);
            GameClient.Network.ClientConnection.SendMessage(DataPacket, NetDeliveryMethod.ReliableUnordered);
        }
    }
}
