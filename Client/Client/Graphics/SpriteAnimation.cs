using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.Graphics
{
    public class SpriteAnimation
    {
        public string Name;
        public List<int> Frames;
        public int FrameDelay = (1000 / 15);
    }
}
