using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformerGame.GameObjects.VisualObjects
{
    /**
     * This class defines a visual effect that is simply placed on screen (or follows apath determined entirely by something else).
     */
    public class VisualObject : GameObject
    {
        public bool repeats;

        public VisualObject() : base(null, 0, 0, 0, 0, 0, 0)
        {
            this.repeats = false;
        }

        public VisualObject(bool repeats, Texture2D texture, int rows, int columns, float x, float y, float width, float height) :
            base(texture, rows, columns, x, y, width, height)
        {
            this.repeats = repeats;
        }

        public override void updateState()
        {
            base.updateState();
            
            if (!repeats && currentFrame >= totalFrames)
            {
                this.active = false;
            }
        }
    }
}
