using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Lidgren.Network;
using System.Collections;

namespace WorldServer.Objects
{
    public class GameObject : Actor
    {
        public bool Collidable = true;
        public bool Interactable = false;
        public Rectangle BoundingBox;
        public bool Active = true;
        public int InteractDistance = 100;

        public GameObject(Rectangle _BoundingBox) : base() {
            Location = new Vector2(_BoundingBox.X, _BoundingBox.Y);
            BoundingBox = _BoundingBox;
        }

        public virtual IEnumerator Interact(NetIncomingMessage Connection) {
            Console.WriteLine("Interacted With Item: {0}", ID);
            yield return null;
        }
        
        public virtual void OnSpawn() {
            Active = true;
            //TODO: Send Spawn Message
            GameServer.TaskScheduler.AddTask(Update());
        }

        public virtual void OnDespawn() {
            //TODO: Send Despawn Message.
            Active = false;
        }

        public virtual IEnumerator Update() {
            while (Active)  {
                yield return null;
            }
        }
    }
}
