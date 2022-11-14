using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.Window;
using SFML.System;

namespace AloneHero_CSharp
{
    class Layer
    {
        public Layer()
        {
            tiles = new List<Sprite>();
        }
        public int opacity;
        public List<Sprite> tiles;
    }
}
