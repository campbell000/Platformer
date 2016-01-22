using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using PlatformerGame.GameObjects.CollisionObjects.Impl;

namespace PlatformerGame
{
    public class PhysicsHandler
    {
        private const float ONE_TILE_SLOPE_VEL = .09f;
        private const float TWO_TILE_SLOPE_VEL = .05f;
        private const float THREE_TILE_SLOPE_VEL = 0;

        //DO WE ALSO NEED TO AUGMENT THE PLAYER's VELOCITY IF THEY ARE ON A SLOPE????????
        private static readonly Dictionary<float, float> angleVelocityDictionary = new Dictionary<float, float>
        {
            {0, 0},
            {Tile.ONE_TILE_SLOPE_ANGLE, ONE_TILE_SLOPE_VEL},
            {Tile.TWO_TILE_SLOPE_AMFLE, TWO_TILE_SLOPE_VEL},
            {Tile.THREE_TILE_SLOPE_ANGLE, THREE_TILE_SLOPE_VEL}
        };

        public static float getXVelAdjustmentForSlopedTile(Tile tile)
        {
            float absVelAdjustment = angleVelocityDictionary[Math.Abs(tile.slopeAngle)];
            if (tile.slopeAngle > 0)
                absVelAdjustment = -absVelAdjustment;

            return absVelAdjustment;
        }
                                                    
        public static float calculatePosition(float pos, float speed, float accel, GameTime delta)
        {
            float time = (float)delta.ElapsedGameTime.TotalMilliseconds;
            float newPos = (speed * time) + pos;
            return newPos;
        }

        public static float calculateSpeed(float speed, float accel, float drag, GameTime delta)
        {
            float time = (float)delta.ElapsedGameTime.TotalMilliseconds;
            float newV = (speed * drag) + (accel * time);
            return newV;
        }
    }
}
