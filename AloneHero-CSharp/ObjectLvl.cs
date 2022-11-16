using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.Window;
using SFML.System;

namespace AloneHero_CSharp
{
    class ObjectLvl
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public FloatRect Rect { get; set; }
        public Dictionary<string, string> properties;
        public Sprite Sprite { get; set; }
    }
}
