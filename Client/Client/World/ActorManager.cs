using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharedCode.Objects;
using Microsoft.Xna.Framework.Graphics;
using Lidgren.Network;
using Lidgren.Network.Xna;
using Client.Graphics;
using Microsoft.Xna.Framework;
using Client.Objects;

namespace Client.World
{
    class ActorManager
    {
        public static Dictionary<long, CActor> DrawableActors = new Dictionary<long, CActor>();
        public static void UpdateAnimationFromPacket(NetIncomingMessage Message) {
            long ActorID = Message.ReadInt64();
            string Animation = Message.ReadString();
            bool Loop = Message.ReadBoolean();
            var Act = DrawableActors[ActorID] as AnimatedActor;
            Act.PlayAnimation(Animation, Loop);
        }
        public static void UpdateFromPacket(NetIncomingMessage Message) {
            long ActorID = Message.ReadInt64();

            if (!DrawableActors.ContainsKey(ActorID))
                return;

            DrawableActors[ActorID].Location = Message.ReadVector2();
        }
        public static void SpawnActor(NetIncomingMessage Message) {
            ObjectTypes ActorType = (ObjectTypes)Message.ReadInt32();
            switch (ActorType) { 
                case ObjectTypes.PlayerActor:
                    #region Player
                    var ID = Message.ReadInt64();
                    var Sex = Message.ReadByte();
                    var Location = Message.ReadVector2();
                    
                    switch(Sex) {
                        case 0:
                            AnimatedActor Actor = new AnimatedActor(new SpriteSheet(
                                GameClient.ContentManager.Load<Texture2D>("Natkuma"),
                                40, 60));

                            Actor.RegisterAnimation("WalkNorth", new List<int>() { 23, 24, 25, 26, 27, 26, 25, 24, 23, 32, 31, 30, 29, 28, 29, 30, 31, 32 });
                            Actor.RegisterAnimation("WalkEast", new List<int>() { 10, 3, 4, 5, 6, 7, 8, 9 });
                            Actor.RegisterAnimation("WalkSouth", new List<int>() { 34, 35, 36, 37, 38, 37, 36, 35, 34, 43, 42 ,41, 40, 39, 40, 41, 42, 43 });
                            Actor.RegisterAnimation("WalkWest", new List<int>() { 11, 18, 17, 16, 15, 14, 13, 12 });
                            Actor.PlayAnimation("WalkEast", true);

                            Actor.ID = ID;
                            Actor.Sex = Sex;
                            Actor.Location = Location;
                            Actor.Scale = 1;
                            if (!DrawableActors.ContainsKey(Actor.ID))
                                DrawableActors.Add(Actor.ID, Actor);
                            else 
                                Console.WriteLine("Error: Avatar Already Added for Key {0}", Actor.ID);
                            break;
                        case 1:
                            AnimatedActor ActorFem = new AnimatedActor(new SpriteSheet(
                                GameClient.ContentManager.Load<Texture2D>("Malice"),
                                40, 60));

                            ActorFem.RegisterAnimation("WalkNorth", new List<int>() { 35, 36, 37, 38, 39, 40, 39, 38, 37, 36, 35, 41, 42, 43, 44, 45, 44, 43, 42, 41 });
                            ActorFem.RegisterAnimation("WalkEast", new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
                            ActorFem.RegisterAnimation("WalkSouth", new List<int>() { 23, 24, 25, 26, 27, 28, 27, 26, 25, 24, 23, 29, 30, 31, 32, 33 });
                            ActorFem.RegisterAnimation("WalkWest", new List<int>() { 12, 13, 14, 15, 16, 17, 18, 19, 20, 21 });
                            ActorFem.PlayAnimation("WalkEast", true);
                            
                            ActorFem.ID = ID;
                            ActorFem.Sex = Sex;
                            ActorFem.Location = Location;
                            ActorFem.Scale = 1;
                            if (!DrawableActors.ContainsKey(ActorFem.ID))
                                DrawableActors.Add(ActorFem.ID, ActorFem);
                            else {
                                DrawableActors[ActorFem.ID] = ActorFem;
                            }
                          break;
                        default:
                            Console.WriteLine("Error: Invalid Sex Type: {0}", Sex);
                            break;
                    }
                    break;
                    #endregion
                case ObjectTypes.Tree:
                    #region Tree
                    Tree T = new Tree(Message);
                    if (!DrawableActors.ContainsKey(T.ID))
                        DrawableActors.Add(T.ID, T);
                    else {
                        DrawableActors[T.ID] = T;
                    }
                    break;
                    #endregion
                case ObjectTypes.Rock:
                    Rock R = new Rock(Message);
                    if (!DrawableActors.ContainsKey(R.ID))
                        DrawableActors.Add(R.ID, R);
                    else
                        DrawableActors[R.ID] = R;

                    break;
                default:
                    Console.WriteLine("Unknown ActorType Creation: {0}", ActorType);
                    break;
            }
        }
        public static void Update(GameTime Time) {
            foreach (CActor Act in DrawableActors.Values)
                Act.Update(Time);
        }
        public static void Draw(SpriteBatch Batch) {
            Batch.Begin(SpriteSortMode.Deferred,
                null,
                null,
                null,
                null,
                null,
                GameClient.Camera.GetTransform(Batch.GraphicsDevice));

            foreach (CActor TexAct in DrawableActors.Values) {
                TexAct.Draw(Batch);
            }
            Batch.End();
        }
    }
}
