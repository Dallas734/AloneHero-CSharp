using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;

namespace AloneHero_CSharp
{
    class Coin : SupportItem
    {
        public Coin(double x, double y, double improveUnits, Level level) : base(x, y, improveUnits)
        {
            xBeginSprite = 5;
            yBeginSprite = 0;
            width = 13;
            height = 16;
            FileName = "Images\\Coin\\Coin.png";
            image = new Image(FileName);
            texture = new Texture(image);
            Sprite = new Sprite(texture);
            level.LoadSupportItems += GetMessageEventHandler;
            level.ChangeParamEvent += GetMessageEventHandler;
        }

        public override void Improve(Entity entity)
        {
            RaiseUsedEvent(new OrderEventArgs(Codes.COIN_UP, (int)improveUnits, entity));
            Used = true;
        }
    }
}
