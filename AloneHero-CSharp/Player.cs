using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;

namespace AloneHero_CSharp
{
    class Player : Entity
    {
        public Player(double x, double y) : base(x, y, 0.1, 300, 50)
        {
            sprites[States.DAMAGE] = null;
            directory = "Player\\";
            State = States.FALL;
            Width = 42;
            Height = 56;
            bufWidth = 118;
            bufOfHit = 47;
            xBeginSprite = 56;
            yBeginSprite = 44;
            widthOfHit = 109;
            countFrames[States.RUN] = 8;
            countFrames[States.IDLE] = 8;
            countFrames[States.HIT] = 4;
            countFrames[States.DAMAGE] = 4;
            countFrames[States.JUMP] = 2;
            countFrames[States.FALL] = 2;
            countFrames[States.DEATH] = 6;
            SetSprite("Damage.png", States.DAMAGE, xBeginSprite, yBeginSprite, Width, Height);
            SetSprite("Death.png", States.DEATH, xBeginSprite, yBeginSprite, Width, Height);
            SetSprite("Hit.png", States.HIT, xBeginSprite, yBeginSprite, widthOfHit, Height);
            SetSprite("Run.png", States.RUN, xBeginSprite, yBeginSprite, Width, Height);
            SetSprite("Idle.png", States.IDLE, xBeginSprite, yBeginSprite, Width, Height);
            SetSprite("Jump.png", States.JUMP, xBeginSprite, yBeginSprite, Width, Height);
            SetSprite("Fall.png", States.FALL, xBeginSprite, yBeginSprite, Width, Height);
        }

        public void HealthUp(int regenerationUnits)
        {
            Health += regenerationUnits;
        }

        public void StrengthUp()
        {

        }

        public void SpeedUp(double improveUnits)
        {
            Speed += improveUnits;
        }
        public override void Update(float time, RenderWindow window, Level level)
        {
            Message message;

            Console.WriteLine(Dy);

            // Уровень земли и падение
            if (Dy == 0 && State != States.IDLE && State != States.DAMAGE)
            {
                State = States.FALL;
                OnGround = false;
                Dy += 0.0001 * time;
            }

            Y += Dy * time;
            message = new Message(Codes.RUN_C, 0, this, X, Y, 0, Dy);
            level.GetMessage(message);

            if (Keyboard.IsKeyPressed(Keyboard.Key.D))
            {
                if (State == States.IDLE)
                {
                    Direction = Directions.RIGHT;
                    State = States.RUN;
                    Move(time, xBeginSprite, yBeginSprite, Width, Height, countFrames[States.RUN], Direction, level);
                    level.ViewOnPlayer(this);
                }
                if (State == States.JUMP)
                {
                    Direction = Directions.RIGHT;
                    Dx = Speed;
                    X += Dx * time;
                }
            }
            else if (Keyboard.IsKeyPressed(Keyboard.Key.A))
            {
                if (State == States.IDLE)
                {
                    Direction = Directions.LEFT;
                    State = States.RUN;
                    Move(time, xBeginSprite, yBeginSprite, Width, Height, countFrames[States.RUN], Direction, level);
                    level.ViewOnPlayer(this);
                }
                if (State == States.JUMP)
                {
                    Direction = Directions.LEFT;
                    Dx = -Speed;
                    X += Dx * time;
                }
            }
            else if (Keyboard.IsKeyPressed(Keyboard.Key.Right) && OnGround)
            {
                Direction = Directions.RIGHT;
                State = States.HIT;
                Hit(time, xBeginSprite, yBeginSprite, widthOfHit, Height, countFrames[States.HIT], bufOfHit, Direction);
                message = new Message(Codes.HIT_C, 0, this, X, Y, Dx, 0);
                level.GetMessage(message);
                level.ViewOnPlayer(this);
            }
            else if (Keyboard.IsKeyPressed(Keyboard.Key.Left) && OnGround)
            {
                Direction = Directions.LEFT;
                State = States.HIT;
                Hit(time, xBeginSprite, yBeginSprite, widthOfHit, Height, countFrames[States.HIT], bufOfHit, Direction);
                message = new Message(Codes.HIT_C, 0, this, X, Y, Dx, 0);
                level.GetMessage(message);
                level.ViewOnPlayer(this);
            }
            else if (State == States.IDLE)
            {
                State = States.IDLE;
                Idle(time, xBeginSprite, yBeginSprite, Width, Height, countFrames[States.IDLE], Direction);
                level.ViewOnPlayer(this);
            }

            if (State == States.FALL)
            {
                State = Fall(time, xBeginSprite, yBeginSprite, Width, Height, countFrames[States.FALL]);
                level.ViewOnPlayer(this);
            }

            if (State == States.DAMAGE)
            {
                State = States.DAMAGE;
                Damage(time, xBeginSprite, yBeginSprite, Width, Height, countFrames[States.DAMAGE], DamagePr, Direction);
                level.ViewOnPlayer(this);
            }

            if (((Keyboard.IsKeyPressed(Keyboard.Key.Space) && OnGround) || (!OnGround && State == States.JUMP)) && State != States.HIT && State != States.DAMAGE)
            {
                State = States.JUMP;
                Jump(time, xBeginSprite, yBeginSprite, Width, Height, countFrames[States.JUMP], Direction, window, level);
                level.ViewOnPlayer(this);
            }

            if (Health <= 0)
            {
                Death(time, xBeginSprite, yBeginSprite, Width, Height, countFrames[States.DEATH], Direction);
            }
        }

        public override void GetMessage(Message message)
        {
            if (message.code == Codes.DAMAGE_C)
            {
                State = States.DAMAGE;
                DamagePr = message.units;
            }
            else if (message.code == Codes.IDLE_C)
            {
                State = States.IDLE;
            }
            else if (message.code == Codes.FALL_C)
            {
                Y = message.y;
                Dy = message.dy;
                State = States.IDLE;
                OnGround = true;
            }
            else if (message.code == Codes.JUMP_C)
            {
                Y = message.y;
            }
            else if (message.code == Codes.CHANGE_X)
            {
                X = message.x;
            }
            else if (message.code == Codes.HEALTH_UP)
            {
                Health += message.units;
            }
            else if (message.code == Codes.SPEED_UP)
            {
                Speed += message.units;
            }
        }

        public override States Fall(float time, int xBeginSprite, int yBeginSprite, int width, int height, int frames)
        {
            // SetSprite("Fall.png", States.FALL, xBeginSprite, yBeginSprite, width, height);
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

        private States Jump(float time, int xBeginSprite, int yBeginSprite, int width, int height, int frames, Directions direction, RenderWindow window, Level level)
        {
            State = States.JUMP;
            // SetSprite("Jump.png", States.JUMP, xBeginSprite, yBeginSprite, width, height);
            currentFrame += time * 0.01;
            if (currentFrame > frames) currentFrame -= frames;

            if (Direction == Directions.RIGHT)
            {
                Message message = new Message(Codes.RUN_C, 0, this, X, Y, Dx, 0);
                level.GetMessage(message);
                sprites[States.JUMP].Origin = new Vector2f(0, 0);
                sprites[States.JUMP].Scale = new Vector2f(1, 1);
            }
            else if (Direction == Directions.LEFT)
            {
                Message message = new Message(Codes.RUN_C, 0, this, X, Y, Dx, 0);
                level.GetMessage(message);
                sprites[States.JUMP].Origin = new Vector2f(sprites[States.JUMP].GetLocalBounds().Width, 0);
                sprites[States.JUMP].Scale = new Vector2f(-1, 1);
            }

            if (OnGround)
            {
                Dy -= 0.1;
                OnGround = false;
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

            Dx = 0;

            sprites[States.JUMP].TextureRect = new IntRect(xBeginSprite + (width + bufWidth) * (int)currentFrame, yBeginSprite, width, height);
            sprites[States.JUMP].Position = new Vector2f((float)X, (float)Y);

            return States.JUMP;
        }

    }
}
