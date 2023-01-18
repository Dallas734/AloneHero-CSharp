using System;
using System.Collections.Generic;
using System.Text;

namespace AloneHero_CSharp
{
    class GreenPotion : SupportItem
    {
        //public event OrderEventHandler UsedEventGreenPotion;
        public GreenPotion(double x, double y, double improveUnits, Level level) : base(x, y, improveUnits)
        {
            xBeginSprite = 102;
            yBeginSprite = 231;
            width = 20;
            height = 22;

        }

        public override void Improve(Entity entity)
        {
            RaiseUsedEvent(new OrderEventArgs(Codes.SPEED_UP, improveUnits, entity));
            //UsedEventGreenPotion?.Invoke(this, new OrderEventArgs(Codes.SPEED_UP, improveUnits, entity));

            //Message message = new Message(Codes.SPEED_UP, improveUnits, null);
            //entity.GetMessage(message);
            Used = true;
        }
    }
}
