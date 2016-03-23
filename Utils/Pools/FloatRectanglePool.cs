using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformerGame.Utils.Pools
{
    class FloatRectanglePool : Pool<FloatRectangle>
    {
        public FloatRectanglePool(int size) : base(size) { }

        public FloatRectangle get(float x, float y, float width, float height)
        {
            FloatRectangle o = null;

            if (pool.Count == 0)
                throw new Exception("POOL IS DRAINED!");
            else
            {
                o = pool.Dequeue();
                o.x = x;
                o.y = y;
                o.width = width;
                o.height = height;
            }

            return o;
        }
    }
}
