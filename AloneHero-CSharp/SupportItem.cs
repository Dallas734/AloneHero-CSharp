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
        public delegate void OrderEventHandler(object sender, OrderEventArgs args);
        public event OrderEventHandler UsedEvent;

        protected double x, y;
        public double DefaultX { get; set; }
        public double DeafaultY { get; set; }

        protected double beginY;
        protected Image image;
        protected Texture texture;
        public Sprite Sprite { get; protected set; }
        public string FileName { get; protected set; }
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
            up = true;
            Used = false;  
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
        public void GetMessageEventHandler(object sender, OrderEventArgs args)
        {
            if (args.Recipient is SupportItem)
            {
                SupportItem sp = (SupportItem)args.Recipient;
                //if (args.Recipient is SupportItem)
                //{ 
                //if (DefaultX == args)
                if (sender is Player)
                {
                    Player player = (Player)sender;
                    if (args.Code == Codes.IMPROVE_STATS && sender is Player && args.Recipient is SupportItem && player.GetRect().Intersects(GetRect()))
                    {
                        Improve((Player)sender);
                    }
                }
                if (args.Code == Codes.STATS_MOVE_LOAD && args.Recipient is SupportItem && args.Used && DefaultX == sp.DefaultX && DeafaultY == sp.DeafaultY)
                {
                    Used = args.Used;
                    args.Used = false;
                }
            }
            
        }

        public FloatRect GetRect()
        {
            return new FloatRect((float)x, (float)y, width, height);
        }

        protected void RaiseUsedEvent(OrderEventArgs args)
        {
            UsedEvent?.Invoke(this, args);
        }
    }
}
