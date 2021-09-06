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

        public static Position operator +(Position p1, Position p2)
        {
            return new Position(p1.X + p2.X, p1.Y + p2.Y);
        }
        public static Position operator -(Position p1, Position p2)
        {
            return new Position(p1.X - p2.X, p1.Y - p2.Y);
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
