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
    abstract class Enemy : Entity
    {
        public event OrderEventHandler AdditionalFeatEvent;
        public bool CollisionWithPlayer { get; protected set; }
        public double DefaultX { get; set; }
        public double DefaultY { get; set; }

        public AddStates AddFeat { get; protected set; }
        public Enemy(double x, double y, double speed, int health, int strength, Level level) : base(x, y, speed, health, strength)
        {
            directory = "Enemies\\";
            State = States.FALL;
            Direction = Directions.RIGHT;
        }

        public override void Update(float time, RenderWindow window, Level level)
        {
            Message message;

            Y += Dy * time;
            RaiseSomeActionEvent(new OrderEventArgs(Codes.RUN_C, 0, X, Y, 0, Dy, level));
            //message = new Message(Codes.RUN_C, 0, this, X, Y, 0, Dy);
            //level.GetMessage(message);

            if (State == States.FALL)
            {
                State = Fall(time, xBeginSprite, yBeginSprite, Width, Height, countFrames[States.IDLE]);
            }

            if (State == States.RUN)
            {
                Move(time, xBeginSprite, yBeginSprite, Width, Height, countFrames[States.RUN], Direction, level);
                RaiseSomeActionEvent(new OrderEventArgs(Codes.RUN_C, 0, X, Y, 0, Dy, level));
                //message = new Message(Codes.RUN_C, 0, this, X, Y, 0, Dy);
                //level.GetMessage(message);
            }

            if (State == States.HIT)
            {
                Hit(time, xBeginSprite, yBeginSprite, widthOfHit, heightOfHit, countFrames[States.HIT], bufOfHit, Direction);
                message = new Message(Codes.HIT_C, 0, this, X, Y, Dx, 0);
                //level.GetMessage(message);
            }

            if (State == States.DAMAGE && Health > 0)
            {
                Damage(time, xBeginSprite, yBeginSprite, Width, Height, countFrames[States.DAMAGE], DamagePr, Direction);
            }

            if (Health <= 0 || State == States.DEATH)
            {
                Death(time, xBeginSprite, yBeginSprite, Width, Height, countFrames[States.DEATH], Direction);
            }
        }

        public override void GetMessageEventHandler(object sender, OrderEventArgs args)
        {
            if (args.Recipient is Enemy)
            {
                Enemy enemy = (Enemy)args.Recipient;
                // Проводится проверка правильности получателя по его координатам
                if (Equals(this, args.Recipient) && X == enemy.X && Y == enemy.Y)
                {
                    //Entity senderEntity = (Entity)sender;

                    if (args.Code == Codes.DAMAGE_C)
                    {
                        CollisionWithPlayer = true;
                        State = States.DAMAGE;
                        DamagePr = args.IntUnits;
                    }
                    else if (args.Code == Codes.RUN_C)
                    {
                        CollisionWithPlayer = false;
                        State = States.RUN;
                    }
                    else if (args.Code == Codes.HIT_C)
                    {
                        CollisionWithPlayer = true;
                        State = States.HIT;
                        if (sender is Player)
                        {
                            AdditionalFeatures((Player)sender); // с отправкой сообщения игроку
                        }
                    }
                    else if (args.Code == Codes.FALL_C)
                    {
                        Y = args.Y;
                        Dy = args.Dy;
                        State = States.RUN;
                        OnGround = true;
                    }
                    else if (args.Code == Codes.JUMP_C)
                    {
                        Y = args.Y;
                    }
                    else if (args.Code == Codes.CHANGE_X)
                    {
                        X = args.X;
                    }
                    else if (args.Code == Codes.ENEMY_BARIER)
                    {
                        if (Direction == Directions.RIGHT) Direction = Directions.LEFT;
                        else Direction = Directions.RIGHT;
                    }

                }
                else if (args.Code == Codes.STATS_MOVE_LOAD_ENEMY && DefaultX == enemy.DefaultX && DefaultY == enemy.DefaultY)
                {                 
                        X = enemy.X;
                        Y = enemy.Y;
                        Health = enemy.Health;
                }
            }
        }
        //public override void GetMessage(Message message)
        //{
        //    if (message.code == Codes.DAMAGE_C)
        //    {
        //        CollisionWithPlayer = true;
        //        State = States.DAMAGE;
        //        DamagePr = message.intUnits;
        //    }   
        //    else if (message.code == Codes.RUN_C)
        //    {
        //        CollisionWithPlayer = false;
        //        State = States.RUN;
        //    }
        //    else if (message.code == Codes.HIT_C)
        //    {
        //        CollisionWithPlayer = true;
        //        State = States.HIT;
        //        AdditionalFeatures(message.sender); // с отправкой сообщения игроку
        //    }
        //    else if (message.code == Codes.FALL_C)
        //    {
        //        Y = message.y;
        //        Dy = message.dy;
        //        State = States.RUN;
        //        OnGround = true;
        //    }
        //    else if (message.code == Codes.JUMP_C)
        //    {
        //        Y = message.y;
        //    }
        //    else if (message.code == Codes.CHANGE_X)
        //    {
        //        X = message.x;
        //    }
        //    else if (message.code == Codes.ENEMY_BARIER)
        //    {
        //        if (Direction == Directions.RIGHT) Direction = Directions.LEFT;
        //        else Direction = Directions.RIGHT;
        //    }

        //}

        public abstract void AdditionalFeatures(Entity entity);

        public override States Fall(float time, int xBeginSprite, int yBeginSprite, int width, int height, int frames)
        {
            SetSprite("Idle.png", States.FALL, xBeginSprite, yBeginSprite, Width, Height);
            currentFrame += time * 0.005;
            if (currentFrame > frames) currentFrame -= frames;

            if (Direction == Directions.RIGHT)
            {
                sprites[States.FALL].Origin = new Vector2f(0, 0);
                sprites[States.FALL].Scale = new Vector2f(1, 1);
                State = States.FALL;
            }
            else if (Direction == Directions.LEFT)
            {
                sprites[States.FALL].Origin = new Vector2f(sprites[States.FALL].GetLocalBounds().Width, 0);
                sprites[States.FALL].Scale = new Vector2f(-1, 1);
                State = States.FALL;
            }

            if (OnGround)
            {
                Dy = 0;
                return States.IDLE;
            }
            else
            {
                Y += Dy * time;
                Dy += 0.0001 * time;
            }

            sprites[States.FALL].TextureRect = new IntRect(xBeginSprite + (width + bufWidth) * (int)currentFrame, yBeginSprite, width, height);
            sprites[States.FALL].Position = new Vector2f((float)X, (float)Y);

            return States.FALL;
        }

        public void RaiseAdditionalFeatEvent(OrderEventArgs args)
        {
            AdditionalFeatEvent?.Invoke(this, args);
        }
    }
}
