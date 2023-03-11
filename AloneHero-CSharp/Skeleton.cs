using System;
using System.Collections.Generic;
using System.Text;

namespace AloneHero_CSharp
{
    class Skeleton : Enemy
    {
        public Skeleton(double x, double y, double speed, int health, int strenght, Level level) : base(x, y, speed, health, strenght, level)
        {
            directory = "Enemies\\Skeleton\\";
            xBeginSprite = 57;
            yBeginSprite = 49;
            Width = 49;
            widthOfHit = 111;
            Height = 54;
            bufWidth = 102;
            countFrames[States.RUN] = 4;
            countFrames[States.IDLE] = 4;
            countFrames[States.HIT] = 8;
            countFrames[States.DAMAGE] = 4;
            countFrames[States.DEATH] = 4;
            bufOfHit = 40;
            heightOfHit = 61;
            xBeginSpriteHit = 49;
            yBeginSpriteHit = 42;
            SetSprite("Damage.png", States.DAMAGE, xBeginSprite, yBeginSprite, Width, Height);
            SetSprite("Death.png", States.DEATH, xBeginSprite, yBeginSprite, Width, Height);
            SetSprite("Hit.png", States.HIT, xBeginSprite, yBeginSprite, widthOfHit, Height);
            SetSprite("Run.png", States.RUN, xBeginSprite, yBeginSprite, Width, Height);
            SetSprite("Idle.png", States.IDLE, xBeginSprite, yBeginSprite, Width, Height);
            // Подписка на событие
            level.ChangeParamEvent += GetMessageEventHandler;
            level.LoadEnemy += GetMessageEventHandler;
        }
    

        public override void AdditionalFeatures(Entity entity)
        {
            //Message message = new Message(Codes.BLEED_C, 5, this);
            //entity.GetMessage(message);
            int a = 0;
        }
    }
}
