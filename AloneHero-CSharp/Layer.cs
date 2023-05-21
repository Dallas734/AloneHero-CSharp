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
            Tiles = new List<Sprite>();
        }
        public int Opacity { get; set; }
        public List<Sprite> Tiles { get; set; }
    }
}
