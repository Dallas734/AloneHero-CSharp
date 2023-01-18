using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;

namespace AloneHero_CSharp
{
    class RedPotion : SupportItem
    {
        //public event OrderEventHandler UsedEventRedPotion;
        public RedPotion(double x, double y, double improveUnits, Level level) : base(x, y, improveUnits)
        {
            xBeginSprite = 8;
            yBeginSprite = 231;
            width = 20;
            height = 22;
        }

        public override void Improve(Entity entity)
        {
            RaiseUsedEvent(new OrderEventArgs(Codes.HEALTH_UP, (int)improveUnits, entity));
            //UsedEventRedPotion?.Invoke(this, new OrderEventArgs(Codes.HEALTH_UP, (int)improveUnits, entity));
            //Message message = new Message(Codes.HEALTH_UP, (int)improveUnits, null);
            //entity.GetMessage(message);
            Used = true;
        }
    }
}
