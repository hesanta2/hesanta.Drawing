using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace hesanta.Drawing
{
    public class Position
    {
        public float X { get; set; }
        public float Y { get; set; }

        public Position(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static implicit operator PointF(Position position)
        {
            return new PointF(position.X, position.Y);
        }

        public static implicit operator Position(PointF point)
        {
            return new Position(point.X, point.Y);
        }
    }
}
