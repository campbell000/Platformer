using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformerGame.GameObjects.CollisionObjects.MovablePhysicsObjects.Actors
{
    public class Actor : MovablePhysicsObject
    {
        public float H_ACCEL_CONST = .019f;
        public float V_ACCEL_CONST = .003f;
        public float H_DRAG_ON_STOP = .85f;
        public float H_DRAG_IN_AIR = .95f;

        public Actor(Texture2D texture, int rows, int columns, float x, float y, float width, float height) : 
            base(texture, rows, columns, x, y, width, height)
        {
            this.ax = 0;
            this.ay = V_ACCEL_CONST;
            this.xDrag = 1f;
        }

        public override void update(Microsoft.Xna.Framework.GameTime delta)
        {
            base.update(delta);
        }

        protected override void updateMovement(Microsoft.Xna.Framework.GameTime delta)
        {
            base.updateMovement(delta);
        }
    }
}
