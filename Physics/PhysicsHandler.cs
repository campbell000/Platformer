using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PlatformerGame
{
    public class PhysicsHandler
    {
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
