using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using PlatformerGame.GameObjects;
using PlatformerGame.GameObjects.CollisionObjects.MovablePhysicsObjects.Actors.StatsActors.Impl;

namespace PlatformerGame.Physics
{
    class CollisionHandler
    {
        private Dictionary<GameObjectType, List<GameObject>> gameObjects;

        public CollisionHandler(Dictionary<GameObjectType, List<GameObject>> gameObjects)
        {
            this.gameObjects = gameObjects;
        }

        public void handleCollisions()
        {
            List<GameObject> walls = gameObjects[GameObjectType.WALLS];
            List<GameObject> actors = gameObjects[GameObjectType.ACTORS];

            foreach (GameObject actor in actors)
            {
                foreach (GameObject wall in walls)
                {
                    KeyValuePair<CollisionType, float> collision = getCollision(actor, wall);
                    if (collision.Key != CollisionType.NONE)
                    {
                        switch(collision.Key)
                        {
                            case CollisionType.LEFT:
                                actor.x += collision.Value;
                                if (actor.vx < 0)
                                    actor.vx = 0;
                                break;
                            case CollisionType.RIGHT:
                                actor.x -= collision.Value;
                                if (actor.vx > 0)
                                    actor.vx = 0;
                                break;
                            case CollisionType.TOP:
                                actor.y += collision.Value;
                                actor.vy = 0;
                                break;
                            case CollisionType.BOTTOM:
                                actor.y -= collision.Value;
                                if(actor.vy >= 0)
                                    actor.vy = 0;
                                break;
                        }
                    }
                }
            }
        }

        public KeyValuePair<CollisionType, float> getCollision(GameObject obj1, GameObject obj2)
        {
            Dictionary<CollisionType, float> oppositeDists = new Dictionary<CollisionType, float>();
            KeyValuePair<CollisionType, float> minCollision = new KeyValuePair<CollisionType, float>(CollisionType.NONE, 0);
            if (obj1.getBoundingBox().Intersects(obj2.getBoundingBox()))
            {
                oppositeDists.Add(CollisionType.TOP, Math.Abs(obj1.getBoundingBox().Top - obj2.getBoundingBox().Bottom));
                oppositeDists.Add(CollisionType.LEFT, Math.Abs(obj1.getBoundingBox().Left - obj2.getBoundingBox().Right));
                oppositeDists.Add(CollisionType.BOTTOM, Math.Abs(obj1.getBoundingBox().Bottom - obj2.getBoundingBox().Top));
                oppositeDists.Add(CollisionType.RIGHT, Math.Abs(obj1.getBoundingBox().Right - obj2.getBoundingBox().Left));

                //Get minimum dist
                minCollision = oppositeDists.Aggregate((l, r) => l.Value < r.Value ? l : r);
            }
            return minCollision;
        }

        public CollisionType getSpeedyCollision(GameObject obj1, GameObject obj2)
        {
            return obj1.getBoundingBox().Intersects(obj2.getBoundingBox()) ? CollisionType.BOTTOM : CollisionType.NONE;
        }
    }
}
