using System;
using System.Collections.Generic;
using System.Text;

namespace AloneHero_CSharp
{
    class GreenPotion : SupportItem
    {
        public GreenPotion(double x, double y, double improveUnits) : base(x, y, improveUnits)
        {
            xBeginSprite = 102;
            yBeginSprite = 231;
            width = 20;
            height = 22;
        }

        public override void Improve(Entity entity)
        {
            Message message = new Message(Codes.SPEED_UP, (int)improveUnits, null);
            entity.GetMessage(message);
            Used = true;
        }
    }
}
