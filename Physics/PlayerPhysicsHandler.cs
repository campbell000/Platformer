using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using PlatformerGame.GameObjects.CollisionObjects.MovablePhysicsObjects.Actors.StatsActors.Impl;

namespace PlatformerGame
{
    public class PlayerPhysicsHandler
    {
        public float xSpeedLimit = 0.6f;
        Player player;

        public PlayerPhysicsHandler(Player p)
        {
            this.player = p;
        }

        public void adjustForInputs(GameTime delta)
        {
            handleHorizontalMovement();
			handleVerticalMovement();
        }

        public void adjustHorizontalPhysicsCalculations()
        {
            player.vx = preventDrift(player.vx);
        }

        public void adjustVerticalPhysicsCalculations()
        {
            gravity();
        }

        public void gravity()
        {
            if (player.y > Resolution.getInternalHeight() - player.height)
            {
                player.y = Resolution.getInternalHeight() - player.height;
                player.vy = 0;
            }
        }

        private void handleVerticalMovement()
        {
            if (player.currInputState.jumpWasReleased())
            {
                player.millisJumpHeld = 0;
                player.jumpWasReleased = true;
            }

            if (player.currInputState.jumpWasPressed() && !isInAir())
            {
                player.vy = player.JUMP_FORCE;
                player.isOnGround = false;
                player.jumpWasReleased = false;
            }

            if (player.jumpWasReleased && isInAir() && player.vy < player.JUMP_FORCE/2)
            {
                player.vy *= .7f;
            }
        }

        private void handleHorizontalMovement()
        {
            player.xDrag = 1;
            if (isInAir())
            {
                handleAirHorizontalMovement();
            }
            else
            {
                handleGroundHorizontalMovement();
            }
        }
        private void handleGroundHorizontalMovement()
        {
            if (player.currInputState.leftWasPressed())
            {
                player.ax = -player.H_ACCEL_CONST / 2;
            }
            else if (player.currInputState.rightWasPressed())
            {
                player.ax = player.H_ACCEL_CONST / 2;
            }
            else
            {
                player.ax = 0;
                player.xDrag = player.H_DRAG_ON_STOP;
            }
        }

        //TODO: DO NOT ALLOW SPEED UPS IN THE AIR. ONLY SPEED CANCELLING
        private void handleAirHorizontalMovement()
        {
            if (player.currInputState.leftWasPressed())
            {
                player.ax = -player.H_ACCEL_CONST / 8;
            }
            else if (player.currInputState.rightWasPressed())
            {
                player.ax = player.H_ACCEL_CONST / 8;
            }
            else
            {
                player.xDrag = player.H_DRAG_IN_AIR;
                player.ax = player.ax / 8;
            }
        }

        private float preventDrift(float a)
        {
            if (a < .001f && a > -.001f)
                return 0;
            else
                return a;
        }

        public void adjustVelocity()
        {
            if (player.vx > xSpeedLimit)
                player.vx = xSpeedLimit;
            else if (player.vx < -xSpeedLimit)
                player.vx = -xSpeedLimit;
        }

        public bool isInAir()
        {
            return !player.isOnGround && !player.isOnSlope;
        }
    }
}
