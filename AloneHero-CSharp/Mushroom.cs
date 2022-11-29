using System;
using System.Collections.Generic;
using System.Text;

namespace AloneHero_CSharp
{
    class Mushroom : Enemy
    {
        private int xBeginSpriteDamage;
        public Mushroom(double x, double y, double speed, int health, int strength) : base(x, y, speed, health, strength)
        {
            directory = "Enemies\\Mushroom\\";
            xBeginSprite = 59;
            yBeginSprite = 58;
            Width = 29;
            widthOfHit = 40;
            Height = 38;
            bufWidth = 122;
            countFrames[States.RUN] = 8;
            countFrames[States.IDLE] = 4;
            countFrames[States.HIT] = 8;
            countFrames[States.DAMAGE] = 4;
            countFrames[States.DEATH] = 4;
            bufOfHit = 114;
            heightOfHit = 46;
            xBeginSpriteHit = 40;
            yBeginSpriteHit = 62;
            SetSprite("Damage.png", States.DAMAGE, xBeginSprite, yBeginSprite, Width, Height);
            SetSprite("Death.png", States.DEATH, xBeginSprite, yBeginSprite, Width, Height);
            SetSprite("Hit.png", States.HIT, xBeginSprite, yBeginSprite, widthOfHit, Height);
            SetSprite("Run.png", States.RUN, xBeginSprite, yBeginSprite, Width, Height);
            SetSprite("Idle.png", States.IDLE, xBeginSprite, yBeginSprite, Width, Height);
        }

        public override void AdditionalFeatures(Entity entity)
        {
            Message message = new Message(Codes.BLEED_C, 5, this);
            entity.GetMessage(message);
            int a = 2;
        }
    }
}
