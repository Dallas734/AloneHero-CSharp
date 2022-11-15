using System;
using System.Collections.Generic;
using System.Text;

namespace AloneHero_CSharp
{
    class RedPotion : SupportItem
    {
        public RedPotion(double x, double y, double improveUnits) : base(x, y, improveUnits)
        {
            xBeginSprite = 8;
            yBeginSprite = 231;
            width = 20;
            height = 22;
        }

        public override void Improve(Entity entity)
        {
            Message message = new Message(Codes.HEALTH_UP, (int)improveUnits, null);
            entity.GetMessage(message);
            Used = true;
        }
    }
}
