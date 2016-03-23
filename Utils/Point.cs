using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformerGame.Utils
{
    public class Point
    {
        public float X { get; set; }
        public float Y { get; set; }

        public Point(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        public float getSlope(Point p2)
        {
            return (p2.Y - Y) / (p2.X - X);
        }

        public static Point setPoint(Point p, float x, float y)
        {
            p.X = x;
            p.Y = y;

            return p;
        }
    }
}
