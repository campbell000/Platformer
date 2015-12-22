using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformerGame.GameObjects.CollisionObjects.MovablePhysicsObjects.Actors.StatsActors
{
    public class StatsActor : Actor
    {
        private String name{
            get { return this.GetType().Name; }
        }

        public int attackStat { get; set; }

        public int defenseState { get; set; }

        public int luckStat { get; set; }

        public int speedStat { get; set; }

        public StatsActor(Texture2D texture, int rows, int columns, float x, float y, float width, float height) : 
            base(texture, rows, columns, x, y, width, height)
        {

        }
    }


}
