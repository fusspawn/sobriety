using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SharedCode.Objects;
using Microsoft.Xna.Framework.Graphics;
using Lidgren.Network;
using Lidgren.Network.Xna;
using SharedCode.Network;

namespace Client.Objects
{
    public class PlayerController
    {
        Vector2 MovementDelta;
        int CurrentDelay = 0;
        int MSDelayPerTick = 66;


        public void Update(GameTime time) {
            CurrentDelay += time.ElapsedGameTime.Milliseconds;
            if (CurrentDelay > MSDelayPerTick) {
                CurrentDelay = 0;
                BuildDelta();
                if(MovementDelta.X != 0 || MovementDelta.Y != 0)
                    SendInput();
            }
        }

        private void BuildDelta()
        {

            MovementDelta = Vector2.Zero;
            if (InputManager.IsKeyDown(Keys.W))
                MovementDelta.Y -= 1;
            if (InputManager.IsKeyDown(Keys.S))
                MovementDelta.Y += 1;
            if (InputManager.IsKeyDown(Keys.A))
                MovementDelta.X -= 1;
            if (InputManager.IsKeyDown(Keys.D))
                MovementDelta.X += 1;
        }

        private void SendInput()
        {
            NetOutgoingMessage MovementPacket = GameClient.Network.ClientConnection.CreateMessage();
            MovementPacket.Write((byte)MessageTypes.PlayerMoveInput);
            MovementPacket.Write(MovementDelta);
            GameClient.Network.ClientConnection.SendMessage(MovementPacket, NetDeliveryMethod.Unreliable);
        }
    }
}
