﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AloneHero_CSharp
{
    class OrderEventArgs : EventArgs
    {
        public OrderEventArgs (Codes code, int intUnits, double x, double y, double dx, double dy, object recipient)
        {
            Code = code;
            IntUnits = intUnits;
            X = x;
            Y = y;
            Dx = dx;
            Dy = dy;
            Recipient = recipient;
        }

        public OrderEventArgs(Codes code, int units, object recipient)
        {
            Code = code;
            IntUnits = units;
            Recipient = recipient;
        }

        public OrderEventArgs(Codes code, double units, object recipient)
        {
            Code = code;
            DoubleUnits = units;
            Recipient = recipient;
        }
        public Codes Code { get; set; }
        public int IntUnits { get; set; }
        public double DoubleUnits { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Dx { get; set; }
        public double Dy { get; set; }
        public float Time { get; set; }

        public object Recipient { get; set; }
    }
}