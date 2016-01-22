using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformerGame.GameObjects.CollisionObjects.MovablePhysicsObjects.NonActors.Interactables.Impl
{
    public class SetPiece : Interactable
    {
        public SetPiece(Texture2D texture, int rows, int columns, float x, float y, float width, float height) : 
            base(texture, rows, columns, x, y, width, height)
        {

        }
    }
}
