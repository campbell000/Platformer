using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace PlatformerGame.GameObjects.Impl
{
    class InteractIndicator : GameObject
    {
        public InteractIndicator(ContentManager Content, int rows, int columns, float x, float y, float width, float height) : 
            base(loadTexture(Content), rows, columns, x, y, width, height)
        {

        }

        private static Texture2D loadTexture(ContentManager content)
        {
            return content.Load<Texture2D>("interact");
        }
    }
}
