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
        public delegate void OrderEventHandler(object sender, OrderEventArgs args);
        public event OrderEventHandler SomeActionEvent;
        public double Speed { get; protected set; }
        public double Dx { get; protected set; }
        public double Dy { get; protected set; }
        public  double X { get; protected set; }
        public double Y { get; protected set; }

        public int Counter { get; protected set; }

        public int Width { get; protected set; }
        public int Height { get; protected set; }
        protected int bufWidth; // Буферная ширина для вырезания спрайтов 
        protected int bufOfHit;
        protected double currentFrame; // Текущий кадр
        public int Health { get; protected set; } // Здоровье
        public int Strength { get; protected set; } // Сила
        public int DamagePr { get;  protected set; } // Полученный урон
        public bool OnGround { get; protected set; }

        protected int xBeginSprite;
        protected int yBeginSprite;
        protected int widthOfHit;
        protected int heightOfHit;
        protected Dictionary <States, int> countFrames;
        protected double xBeginSpriteHit;
        protected double yBeginSpriteHit;


        protected string directory; // Имя директории, где хранятся анимации
        protected Dictionary<States, Sprite> sprites; // Спрайты
        public States State { protected set; get; }

        protected Image image; // Картинка для создания спрайтов
        protected Texture texture; // Текстура для создания спрайтов
        public Directions Direction { get; set; } // Направление движения

        public Entity(double x, double y, double speed, int health, int strength)
        {
            countFrames = new Dictionary<States, int>();
            sprites = new Dictionary<States, Sprite>();
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
            Counter = 0;
        }

        public abstract void Update(float time, RenderWindow window, Level level);
        public abstract States Fall(float time, int xBeginSprite, int yBeginSprite, int width, int height, int frames);
        // public abstract void GetMessage(Message message);
        public abstract void GetMessageEventHandler(object sender, OrderEventArgs args);
      
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

        protected void SetSprite(String fileName, States spriteName, int xBeginSprite, int yBeginSprite, int width, int height)
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
            //SetSprite("Damage.png", States.DAMAGE, xBeginSprite, yBeginSprite, width, height);
            Health -= DamagePr;
            DamagePr = 0;
            currentFrame += time * 0.005;
            if (currentFrame > frames)
            {
                currentFrame -= frames;
                //Health -= DamagePr;
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
            sprites[States.DAMAGE].Position = new Vector2f((float)X, (float)Y);

            //if (Health <= 0)
            //{
            //    State = States.DEATH;
            //    return States.DEATH;
            //}
            //else 
            return States.DAMAGE;
        }

        protected States Move(float time, int xBeginSprite, int yBeginSprite, int width, int height, int frames, Directions direction, Level level)
        {
            Message message;

            //SetSprite("Run.png", States.RUN, xBeginSprite, yBeginSprite, width, height);
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


            SomeActionEvent?.Invoke(this, new OrderEventArgs(Codes.RUN_C, 0, X, Y, Dx, 0, level));
            //message = new Message(Codes.RUN_C, 0, this, X, Y, Dx, 0);
            //level.GetMessage(message);
            
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
            //SetSprite("Idle.png", States.IDLE, xBeginSprite, yBeginSprite, width, height);
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
            //SetSprite("Death.png", States.DEATH, xBeginSprite, yBeginSprite, width, height);

            currentFrame += time * 0.01;
            if (currentFrame > frames)
            {
                Counter++;
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

        // Вызывает события
        protected void RaiseSomeActionEvent(OrderEventArgs args)
        {
            SomeActionEvent?.Invoke(this, args);
        }
    }
}
