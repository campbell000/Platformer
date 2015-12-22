using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformerGame.GameObjects.CollisionObjects.MovablePhysicsObjects
{
    public class MovablePhysicsObject : CollisionObject
    {
        public MovablePhysicsObject(Texture2D texture, int rows, int columns, float x, float y, float width, float height) : 
            base(texture, rows, columns, x, y, width, height)
        {

        }

        public override void update(GameTime delta)
        {
            updateMovement(delta);
            base.updateAnimation();
        }

        protected virtual void updateMovement(GameTime delta)
        {
            //Calculate vx (and 
            vx = PhysicsHandler.calculateSpeed(vx, ax, xDrag, delta);
            adjustXVelocity();
            x = PhysicsHandler.calculatePosition(x, vx, ax, delta);
            vy = PhysicsHandler.calculateSpeed(vy, ay, yDrag, delta);
            adjustYVelocity();
            y = PhysicsHandler.calculatePosition(y, vy, ay, delta);
        }

        protected virtual void adjustXVelocity()
        {
            vx = vx;
        }

        protected virtual void adjustYVelocity()
        {
            vy = vy;
        }

        protected override void updateAnimation()
        {
            base.updateAnimation();
        }
    }
}
