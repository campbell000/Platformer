using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PlatformerGame.Utils.Pools
{
    public class RectanglePool : Pool<Rectangle>
    {
        public Rectangle get(int x, int y, int width, int height)
        {
            Rectangle o;

            if (pool.Count == 0)
                throw new Exception("POOL IS DRAINED!");
            else
            {
                o = pool.Dequeue();
                o.X = x;
                o.Y = y;
                o.Width = width;
                o.Height = height;
            }

            return o;
        }
    }
}
