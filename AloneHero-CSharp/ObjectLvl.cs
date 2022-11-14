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
        public string name;
        public string type;
        public FloatRect rect;
        public Dictionary<string, string> properties;
        public Sprite sprite;
    }
}
