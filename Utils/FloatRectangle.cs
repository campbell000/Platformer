using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PlatformerGame.Utils
{
    /**
     * This class should be used a the bounding box of a GameObject (or at the least, a GameObject's bounding box should be composed of at least
     * ONE FloatRectangle. This class is used over Microsoft's Rectangle class because this game uses floats for positions and velocities. Using
     * Rectangle caused some rounding errors: on a platform, the player's velocity would fluctuate between 0 and 0.0500001. Using floats fixes
     * this problem.
     **/
    public class FloatRectangle
    {
        public float x { get; set; }
        public float y { get; set; }
        public float width { get; set; }
        public float height { get; set; }

        public float Top { get { return y; } }
        public float Bottom { get { return y + height; } }
        public float Left { get { return x; } }
        public float Right { get { return x + width; } }
        private Rectangle internalRect = new Rectangle();

        public FloatRectangle(float x, float y, float width, float height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public FloatRectangle()
        {
            this.x = 0;
            this.y = 0;
            this.width = 0;
            this.height = 0;
        }


        public bool Intersects(FloatRectangle r2)
        {
            return !(
                r2.Left >= this.Right ||
                r2.Right <= this.Left ||
                r2.Top >= this.Bottom ||
                r2.Bottom <= this.Top);
        }

        private Rectangle getRect()
        {
            internalRect.X = (int)x;
            internalRect.Y = (int)y;
            internalRect.Width = (int)width;
            internalRect.Height = (int)height;

            return internalRect;
        }
    }
}
