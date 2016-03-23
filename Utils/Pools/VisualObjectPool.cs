using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlatformerGame.GameObjects.VisualObjects;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformerGame.Utils.Pools
{
    public class VisualObjectPool : Pool<VisualObject>
    {
        public VisualObject get(bool repeats, Texture2D texture, int rows, int columns, float x, float y, float width, float height)
        {
            VisualObject o = null;

            if (pool.Count == 0)
                throw new Exception("POOL IS DRAINED!");
            else
            {
                o = pool.Dequeue();
                o.repeats = repeats;
                o.texture = texture;
                o.rows = rows;
                o.columns = columns;
                o.x = x;
                o.y = y;
                o.width = width;
                o.height = height;
            }

            return o;
        }
    }
}
