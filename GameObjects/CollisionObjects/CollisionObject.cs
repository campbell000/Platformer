using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformerGame.GameObjects.CollisionObjects
{
    public class CollisionObject : GameObject
    {
        public CollisionObject(Texture2D texture, int rows, int columns, float x, float y, float width, float height) : 
            base(texture, rows, columns, x, y, width, height)
        {

        }

        protected override void updateAnimation()
        {
            base.updateAnimation();
        }
    }
}
