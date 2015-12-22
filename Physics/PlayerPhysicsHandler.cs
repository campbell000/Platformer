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
        public float xSpeedLimit = 1.2f;
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

        public void adjustPhysicsCalculations()
        {
            gravity();
            player.vx = preventDrift(player.vx);
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
            if (player.jumpWasReleased && !isInAir())
                player.canJumpAgain = true;
            if (player.currInputState.jumpWasPressed() && !isInAir() && player.canJumpAgain)
            {
                player.vy = player.JUMP_FORCE;
                player.canJumpAgain = false;
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
                player.ax = -player.H_ACCEL_CONST / 6;
            }
            else if (player.currInputState.rightWasPressed())
            {
                player.ax = player.H_ACCEL_CONST / 6;
            }
            else
            {
                player.xDrag = player.H_DRAG_IN_AIR;
                player.ax = player.ax / 6;
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
            return player.vy != 0;
        }
    }
}
