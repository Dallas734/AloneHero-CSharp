using System;
using System.Collections.Generic;
using System.Text;

namespace AloneHero_CSharp
{
    class Message
    {
        public Codes code;
        public int units;
        public double x;
        public double y;
        public double dx;
        public double dy;
        public float time;
        public Entity sender;

        public Message(int units)
        {
            this.units = units;
        }
        public Message(Codes code, int units, Entity sender)
        {
            this.code = code;
            this.units = units;
            this.sender = sender;
        }

        public Message(Codes code, int units, Entity sender, float time)
        {
            this.code = code;
            this.units = units;
            this.sender = sender;
            this.time = time;
        }

        public Message(Codes code, int units, Entity sender, double x, double y, double dx, double dy)
        {
            this.code = code;
            this.units = units;
            this.sender = sender;
            this.x = x;
            this.y = y;
            this.dx = dx;
            this.dy = dy;
        }
    }
}
