using System;
using System.Collections.Generic;
using System.Text;

namespace AloneHero_CSharp
{
    class Goblin : Enemy
    {
        public Goblin (double x, double y, double speed, int health, int strength) : base (x, y, speed, health, strength)
        {
            directory = "Enemies\\Goblin\\";
            xBeginSprite = 53;
            yBeginSprite = 59;
            Width = 45;
            widthOfHit = 50;
            Height = 43;
            bufWidth = 105;
            countFrames[States.RUN] = 8;
            countFrames[States.IDLE] = 4;
            countFrames[States.HIT] = 8;
            countFrames[States.DAMAGE] = 4;
            countFrames[States.DEATH] = 4;
            bufOfHit = 72;
            heightOfHit = 53;
            xBeginSpriteHit = 55;
            yBeginSpriteHit = 52;
            SetSprite("Damage.png", States.DAMAGE, xBeginSprite, yBeginSprite, Width, Height);
            SetSprite("Death.png", States.DEATH, xBeginSprite, yBeginSprite, Width, Height);
            SetSprite("Hit.png", States.HIT, xBeginSprite, yBeginSprite, widthOfHit, Height);
            SetSprite("Run.png", States.RUN, xBeginSprite, yBeginSprite, Width, Height);
            SetSprite("Idle.png", States.IDLE, xBeginSprite, yBeginSprite, Width, Height);
        }

        public override void AdditionalFeatures(Entity entity)
        {
            int a = 3;
        }
    }
}
