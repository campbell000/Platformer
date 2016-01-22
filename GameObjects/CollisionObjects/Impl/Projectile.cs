using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformerGame.GameObjects.CollisionObjects.Impl
{
    public class Projectile : CollisionObject
    {
        public Projectile(Texture2D texture, int rows, int columns, float x, float y, float width, float height) : 
            base(texture, rows, columns, x, y, width, height)
        {
            
        }
    }
}
