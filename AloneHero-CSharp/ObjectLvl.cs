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
        public ObjectLvl(string name, string type, Sprite sprite, FloatRect rect)
        {
            Name = name;
            Type = type;
            Sprite = sprite;
            Rect = rect;
        }

        public ObjectLvl()
        {
            
        }
        public string Name { get; private set; }
        public string Type { get; private set; }
        public FloatRect Rect { get; private set; }
        public Sprite Sprite { get; private set; }
    }
}
