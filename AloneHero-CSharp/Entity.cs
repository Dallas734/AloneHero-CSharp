using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.Window;
using SFML.System;

namespace AloneHero_CSharp
{
    abstract class Entity
    {
        protected double Speed { get; set; }
        protected double dx, dy;
        protected double x, y;
        protected double width, height;
        protected double bufWidth; // Буферная ширина для вырезания спрайтов 
        protected double bufOfHit;
        protected double currentFrame; // Текущий кадр
        protected int health; // Здоровье
        protected double strength; // Сила
        protected double damage; // Полученный урон
        protected bool onGround;

        protected double xBeginSprite;
        protected double yBeginSprite;
        protected double widthOfHit;
        protected double heightOfHit;
        protected Dictionary <States, int> countFrames;
        double xBeginSpriteHit;
        double yBeginSpriteHit;


        protected string directory; // Имя директории, где хранятся анимации
        protected Dictionary<States, Sprite> sprites; // Спрайты
        protected States state;
        protected Image image; // Картинка для создания спрайтов
        protected Texture texture; // Текстура для создания спрайтов
        protected Directions direction; // Направление движения

        public Entity(double x, double y, double speed, int health, double strength)
        {
            this.x = x;
            this.y = y;
            Speed = speed;
            this.health = health;
            this.strength = strength;
            direction = Directions.RIGHT;
            currentFrame = 0;
            dx = 0;
            dy = 0;
            onGround = false;
        }

        public abstract void Updtate(float time, RenderWindow window, Level level);
        public abstract States Fall(float time, double xBeginSprite, double yBeginSprite, double width, double height, int frames, Directions direction, RenderWindow window, Level level);
        public Sprite GetSprite(States spriteName)
        {
            switch (spriteName)
            {
                case States.JUMP:
                    return sprites[States.JUMP];
                case States.RUN:
                    return sprites[RUN];
                case States.DAMAGE:
                    return sprites[DAMAGE];
                case States.HIT:
                    return sprites[HIT];
                case States.IDLE:
                    return sprites[IDLE];
                case States.FALL:
                    return sprites[FALL];
                case States.DEATH:
                    return sprites[DEATH];
            }
        }
    }
}
