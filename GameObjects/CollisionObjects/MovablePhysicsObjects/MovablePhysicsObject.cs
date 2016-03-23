using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using PlatformerGame.Utils;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformerGame.GameObjects.CollisionObjects.MovablePhysicsObjects
{
    public class MovablePhysicsObject : CollisionObject
    {
        public bool canCollisde { get; set; }
        public bool isOnSlope { get; set; }
        public bool isOnGround { get; set; }
        public bool isTouchingSlope { get; set; }

        public MovablePhysicsObject(Texture2D texture, int rows, int columns, float x, float y, float width, float height) : 
            base(texture, rows, columns, x, y, width, height)
        {
            isOnSlope = false;
            isOnGround = false;
        }

        public virtual void updateHorizontalMovement(GameTime delta)
        {
            vx = PhysicsHandler.calculateSpeed(vx, ax, xDrag, delta);
            adjustXVelocity();
            x = PhysicsHandler.calculatePosition(x, vx, ax, delta);
        }

        public virtual void updateVerticalMovement(GameTime deltaTime)
        {
            vy = PhysicsHandler.calculateSpeed(vy, ay, yDrag, deltaTime);
            adjustYVelocity();
            y = PhysicsHandler.calculatePosition(y, vy, ay, deltaTime);
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

        public void resetCollisionState()
        {
            isOnSlope = false;
            isTouchingSlope = false;
            isOnGround = false;
        }
    }
}
