using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharedCode.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Client.Objects;
using Client.Graphics;

namespace Client.Objects
{
    public class AnimatedActor : CActor
    {
        public AnimatedActor(SpriteSheet Sprites) {
            Sheet = Sprites;
            Animations = new Dictionary<String, SpriteAnimation>();
        }

        public void RegisterAnimation(string _Name, List<int> _Frames) {
            Animations.Add(_Name, new SpriteAnimation() { 
                Name = _Name,
                Frames = _Frames
            });
        }

        Dictionary<String, SpriteAnimation> Animations;
        SpriteSheet Sheet;
        String CurrentAnim;

        int CurrentFrameShowTime = 0;
        int CurrentFrame = 0;
        int FrameCount = 0;
        bool PlayAnim = false;
        bool LoopAnim = false;
        public int Scale = 1;

        public void PlayAnimation(string Name, bool Loop = false) 
        {
            if (Name == "Stop")
            {
                PlayAnim = false;
                LoopAnim = false;
                return;
            }

            if (Animations.ContainsKey(Name))
            {
                CurrentFrame = 0;
                FrameCount = Animations[Name].Frames.Count;
                CurrentAnim = Name;
                PlayAnim = true;
                LoopAnim = Loop;
            }
        }

        public override void Update(GameTime Time) {
            CurrentFrameShowTime += Time.ElapsedGameTime.Milliseconds;
            if (!PlayAnim)
                return;

            if (CurrentFrameShowTime >= Animations[CurrentAnim].FrameDelay) {
                CurrentFrameShowTime = 0;
                CurrentFrame++;
                if (CurrentFrame >= FrameCount) {
                    CurrentFrame = 0;
                    if (!LoopAnim)
                        PlayAnim = false;
                }
            }
        }

        public override void Draw(SpriteBatch Batch) {
            Sheet.DrawSprite(Batch, new Rectangle((int)Location.X, (int)Location.Y, Sheet.Width * Scale, Sheet.Height * Scale), Animations[CurrentAnim].Frames[CurrentFrame]);
        }
    }
}
