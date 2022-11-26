using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.Window;
using SFML.System;

namespace AloneHero_CSharp
{
    abstract class SupportItem
    {
        protected double x, y;
        protected double beginY;
        protected Image image;
        protected Texture texture;
        public Sprite Sprite { get; protected set; }
        protected string fileName;
        protected int width, height;
        protected int xBeginSprite, yBeginSprite;
        protected double improveUnits;
        protected bool up;
        public bool Used { get; protected set; }

        public SupportItem(double x, double y, double improveUnits)
        {
            this.x = x;
            this.y = y;
            beginY = y;
            this.improveUnits = improveUnits;
            fileName = "Images\\Potions\\PotionsPack1.png";
            up = true;
            Used = false;
            image = new Image(fileName);
            texture = new Texture(image);
            Sprite = new Sprite(texture);   
        }

        public void Update(float time, RenderWindow window)
        {
            Sprite.TextureRect = new IntRect(xBeginSprite, yBeginSprite, width, height);

            if (y <= beginY + 10 && up == false)
            {
                y += 0.01 * time;
            }
            else if (y >= beginY + 10) up = true;

            if (y >= beginY - 10 && up == true)
            {
                y += -0.01 * time;
            }
            else if (y <= beginY - 10) up = false;

            Sprite.Position = new Vector2f((float)x, (float)y);
        }

        public abstract void Improve(Entity entity);

        public FloatRect GetRect()
        {
            return new FloatRect((float)x, (float)y, width, height);
        }

        public void GetMessage(Message message)
        {
            if (message.code == Codes.IMPROVE_STATS)
            {
                Improve(message.sender);
            }
        }
    }
}
