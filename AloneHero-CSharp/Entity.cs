using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using SFML;
using SFML.Audio;

namespace AloneHero_CSharp
{
    abstract class Entity
    {
        public double Speed { get; set; }
        public double Dx { get; set; }
        public double Dy { get; set; }
        public  double X { get; set; }
        public double Y { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }
        protected int bufWidth; // Буферная ширина для вырезания спрайтов 
        protected int bufOfHit;
        protected double currentFrame; // Текущий кадр
        protected int Health { get; set; } // Здоровье
        public int Strength { get; } // Сила
        public int DamagePr { get;  set; } // Полученный урон
        public bool OnGround { get; set; }

        protected int xBeginSprite;
        protected int yBeginSprite;
        protected int widthOfHit;
        protected double heightOfHit;
        protected Dictionary <States, int> countFrames;
        protected double xBeginSpriteHit;
        protected double yBeginSpriteHit;


        protected string directory; // Имя директории, где хранятся анимации
        protected Dictionary<States, Sprite> sprites; // Спрайты
        public States State { set; get; }
        protected Image image; // Картинка для создания спрайтов
        protected Texture texture; // Текстура для создания спрайтов
        public Directions Direction { get; set; } // Направление движения

        public Entity(double x, double y, double speed, int health, int strength)
        {
            X = x;
            Y = y;
            Speed = speed;
            Health = health;
            Strength = strength;
            Direction = Directions.RIGHT;
            currentFrame = 0;
            Dx = 0;
            Dy = 0;
            OnGround = false;
        }

        public abstract void Update(float time, RenderWindow window, Level level);
        public abstract States Fall(float time, int xBeginSprite, int yBeginSprite, int width, int height, int frames, Directions direction, RenderWindow window, Level level);
        public abstract void GetMessage(Message message);
        public Sprite GetSprite(States spriteName)
        {
            switch (spriteName)
            {
                case States.JUMP:
                    return sprites[States.JUMP];
                case States.RUN:
                    return sprites[States.RUN];
                case States.DAMAGE:
                    return sprites[States.DAMAGE];
                case States.HIT:
                    return sprites[States.HIT];
                case States.IDLE:
                    return sprites[States.IDLE];
                case States.FALL:
                    return sprites[States.FALL];
                case States.DEATH:
                    return sprites[States.DEATH];
                default:
                    return sprites[States.IDLE];
            }
        }

        public void SetSprite(String fileName, States spriteName, int xBeginSprite, int yBeginSprite, int width, int height)
        {
            image = new Image("Images\\" + this.directory + fileName);
            texture = new Texture(image);

            switch (spriteName)
            {
                case States.JUMP:
                    sprites[States.JUMP] = new Sprite(texture, new IntRect(xBeginSprite, yBeginSprite, width, height));
                    break;
                case States.RUN:
                    sprites[States.RUN] = new Sprite(texture, new IntRect(xBeginSprite, yBeginSprite, width, height));
                    break;
                case States.DAMAGE:
                    sprites[States.DAMAGE] = new Sprite(texture, new IntRect(xBeginSprite, yBeginSprite, width, height));
                    break;
                case States.HIT:
                    sprites[States.HIT] = new Sprite(texture, new IntRect(xBeginSprite, yBeginSprite, width, height));
                    break;
                case States.IDLE:
                    sprites[States.IDLE] = new Sprite(texture, new IntRect(xBeginSprite, yBeginSprite, width, height));
                    break;
                case States.FALL:
                    sprites[States.FALL] = new Sprite(texture, new IntRect(xBeginSprite, yBeginSprite, width, height));
                    break;
                case States.DEATH:
                    sprites[States.DEATH] = new Sprite(texture, new IntRect(xBeginSprite, yBeginSprite, width, height));
                    break;
            }
        }

        public FloatRect GetRect()
        {
            return new FloatRect((float)X, (float)Y, Width, Height);
        }

        public FloatRect GetHitRect()
        {
            if (Direction == Directions.RIGHT)
            {
                return new FloatRect((float)X, (float)Y, sprites[States.HIT].GetLocalBounds().Width - 20, Height);
            }
            else if (Direction == Directions.LEFT)
            {
                return new FloatRect((float)X - 25, (float)Y, sprites[States.HIT].GetLocalBounds().Width, Height);
            }
            return new FloatRect();
        }

        protected States Hit(float time, int xBeginSprite, int yBeginSprite, int width, int height, int frames, int bufOfHit, Directions direction)
        {
            SetSprite("Hit.png", States.HIT, xBeginSprite, yBeginSprite, width, height);
            currentFrame += time * 0.01;
            if (currentFrame > frames)
            {
                currentFrame -= frames;
                State = States.RUN;
                return States.RUN;
            }

            if (direction == Directions.RIGHT)
            {
                sprites[States.HIT].Origin = new Vector2f(0, 0);
                sprites[States.HIT].Scale = new Vector2f(1, 1);
                State = States.HIT;
            }
            else if (direction == Directions.LEFT)
            {
                sprites[States.HIT].Origin = new Vector2f(sprites[States.HIT].GetLocalBounds().Width / 2, 0);
                sprites[States.HIT].Scale= new Vector2f(-1, 1);
                State = States.HIT;
            }

            sprites[States.HIT].TextureRect = new IntRect(xBeginSprite + (width + bufOfHit) * (int)currentFrame, yBeginSprite, width, height);
            sprites[States.HIT].Position = new Vector2f((float)X, (float)Y);


            return States.HIT;
        }

        protected States Damage(float time, int xBeginSprite, int yBeginSprite, int width, int height, int frames, int damage, Directions direction)
        {
            SetSprite("Damage.png", States.DAMAGE, xBeginSprite, yBeginSprite, width, height);
            currentFrame += time * 0.005;
            if (currentFrame > frames)
            {
                currentFrame -= frames;
                Health -= damage;
            }

            if (direction == Directions.RIGHT)
            {
                sprites[States.DAMAGE].Origin = new Vector2f(0, 0); ;
                sprites[States.DAMAGE].Scale = new Vector2f(1, 1);
                State = States.DAMAGE;
            }
            else if (direction == Directions.LEFT)
            {
                sprites[States.DAMAGE].Origin = new Vector2f(sprites[States.DAMAGE].GetLocalBounds().Width, 0);
                sprites[States.DAMAGE].Scale = new Vector2f(-1, 1);
                State = States.DAMAGE;
            }

            sprites[States.DAMAGE].TextureRect = new IntRect(xBeginSprite + (width + bufWidth) * (int)currentFrame, yBeginSprite, width, height);
            sprites[States.HIT].Position = new Vector2f((float)X, (float)Y);

            return States.DAMAGE;
        }

        protected States Move(float time, int xBeginSprite, int yBeginSprite, int width, int height, int frames, Directions direction, Level level)
        {
            Message message;

            SetSprite("Run.png", States.RUN, xBeginSprite, yBeginSprite, width, height);
            if (direction == Directions.RIGHT && OnGround)
            {
                Dx = Speed;
                sprites[States.RUN].Origin = new Vector2f(0, 0);
                sprites[States.RUN].Scale = new Vector2f(1, 1);
                State = States.RUN;
            }
            else if (direction == Directions.LEFT && OnGround)
            {
                Dx = -Speed;
                sprites[States.RUN].Origin = new Vector2f(sprites[States.RUN].GetLocalBounds().Width, 0);
                sprites[States.RUN].Scale = new Vector2f(-1, 1);
                State = States.RUN;
            }

            sprites[States.RUN].TextureRect = new IntRect(xBeginSprite + (width + bufWidth) * (int)currentFrame, yBeginSprite, width, height);

            X += Dx * time;
            message = new Message(Codes.RUN_C, 0, this, X, Y, Dx, 0);
            level.GetMessage(message);

            currentFrame += time * 0.005;
            if (currentFrame > frames)
            {
                currentFrame -= frames;
            }    

            if (OnGround)
            {
                sprites[States.RUN].Position = new Vector2f((float)X, (float)Y);
            }

            Dx = 0;

            return States.RUN;
        }

        protected States Idle(float time, int xBeginSprite, int yBeginSprite, int width, int height, int frames, Directions direction)
        {
            SetSprite("Idle.png", States.IDLE, xBeginSprite, yBeginSprite, width, height);
            currentFrame += time * 0.005;
            if (currentFrame > frames)
            {
                currentFrame -= frames;
            }

            if (direction == Directions.RIGHT)
            {
                sprites[States.IDLE].Origin = new Vector2f(0, 0);
                sprites[States.IDLE].Scale = new Vector2f(1, 1);
                State = States.IDLE;
            }
            else if (direction == Directions.LEFT)
            {
                sprites[States.IDLE].Origin = new Vector2f(sprites[States.IDLE].GetLocalBounds().Width, 0);
                sprites[States.IDLE].Scale = new Vector2f(-1, 1);
                State = States.IDLE;
            }

            sprites[States.IDLE].TextureRect = new IntRect(xBeginSprite + (width + bufWidth) * (int)currentFrame, yBeginSprite, width, height);
            sprites[States.IDLE].Position = new Vector2f((float)X, (float)Y);

            return States.IDLE;
        }

        protected States Death(float time, int xBeginSprite, int yBeginSprite, int width, int height, int frames, Directions direction)
        {
            SetSprite("Death.png", States.DEATH, xBeginSprite, yBeginSprite, width, height);

            currentFrame += time * 0.01;
            if (currentFrame > frames)
            {
                currentFrame -= frames;
                State = States.DEATH;
            }

            if (direction == Directions.RIGHT)
            {
                sprites[States.DEATH].Origin = new Vector2f(0, 0);
                sprites[States.DEATH].Scale = new Vector2f(1, 1);
            }
            else if (direction == Directions.LEFT)
            {
                sprites[States.DEATH].Origin = new Vector2f(sprites[States.DEATH].GetLocalBounds().Width, 0);
                sprites[States.DEATH].Scale = new Vector2f(-1, 1);
            }

            sprites[States.DEATH].TextureRect = new IntRect(xBeginSprite + (width + bufWidth) * (int)currentFrame, yBeginSprite, width, height);
            sprites[States.DEATH].Position = new Vector2f((float)X, (float)Y);

            return States.DEATH;
        }
    }
}
