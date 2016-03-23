using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlatformerGame.GameObjects;
using Microsoft.Xna.Framework;
using PlatformerGame.GameObjects.CollisionObjects.MovablePhysicsObjects.Actors.StatsActors.Impl;
using PlatformerGame.GameObjects.CollisionObjects.MovablePhysicsObjects;
using PlatformerGame.GameObjects.CollisionObjects.MovablePhysicsObjects.NonActors.Interactables;
using PlatformerGame.GameObjects.CollisionObjects.Impl;
using PlatformerGame.GameObjects.CollisionObjects.MovablePhysicsObjects.NonActors.Interactables.Impl;
using PlatformerGame.Utils;
using PlatformerGame.Utils.Pools;

namespace PlatformerGame.Physics
{
    class CollisionHandler
    {
        public const int HORIZONTAL_CHECK = 0;
        public const int VERTICAL_CHECK = 1;
        public const float THRESHOLD = .005f;

        /* Object Pools */
        private FloatRectanglePool floatRectanglePool = new FloatRectanglePool(2);
        private PairPool<CollisionType, float> pairPool = new PairPool<CollisionType, float>();
        private Interactable interactableTouchingPlayer = null;

        public Interactable getInteractableTouchingPlayer()
        {
            return interactableTouchingPlayer;
        }

        public int handlePhysicsObjectCollisions(GameObjectContainer container, int axis)
        {
            return handleWallCollisionsForAxis(container, container.physicsObjects, axis);
        }

        public void handleSetpeiceCollisions(List<Interactable> interactables, Player player)
        {
            //Reset the list of actors and set peices that are close to each other, in case some of them moved
            this.interactableTouchingPlayer = null;

            foreach (Interactable peice in interactables)
            {
                if (getSpeedyCollision(player, peice) != CollisionType.NONE)
                {
                    //Add the actor->setpeice pair to the list of objects that are in proximity to each other
                    interactableTouchingPlayer = peice;
                }
            }
        }

        protected List<Tile> sortTiles(GameObjectContainer container, Rectangle boundingTileBox)
        {
            List<Tile> tiles = new List<Tile>();
            for (int tx = boundingTileBox.Left; tx <= boundingTileBox.Right; tx++)
            {
                for (int ty = boundingTileBox.Top; ty <= boundingTileBox.Bottom; ty++)
                {
                    Tile wall = container.getTileAt(tx, ty);
                    if (wall != null)
                    {
                        if (wall.isSloped)
                            tiles.Insert(0, wall);
                        else
                            tiles.Add(wall);
                    }
                }
            }
            return tiles;
        }

        /**
         * This method handles collisions against actors and walls. It stops characters from going through objects they should not.
         **/
        private int handleWallCollisionsForAxis(GameObjectContainer gameObjectContainer, List<MovablePhysicsObject> actors, int axis)
        {
            int numCollisionsChecked = 0;
            for (int i = 0; i < actors.Count; i++)
            {
                Rectangle tileCollideBox = gameObjectContainer.getBoundingBoxForPossibleCollisions(actors[i]);
                numCollisionsChecked += handleSlopedCollisions(gameObjectContainer, actors[i], tileCollideBox, axis);
                numCollisionsChecked += handleRegularCollision(gameObjectContainer, actors[i], tileCollideBox, axis);    
            }
            
            return numCollisionsChecked;
        }

        private int handleRegularCollision(GameObjectContainer gameObjectContainer, MovablePhysicsObject actor, Rectangle tileCollideBox, int axis)
        {
            int numCollisionsChecked = 0;
            for (int tx = tileCollideBox.Left; tx <= tileCollideBox.Right; tx++)
            {
                for (int ty = tileCollideBox.Top; ty <= tileCollideBox.Bottom; ty++)
                {
                    Tile wall = gameObjectContainer.getTileAt(tx, ty);
                    if (shouldCheckForRegularCollision(actor, wall, axis))
                    {
                        numCollisionsChecked++;
                        resolveRegularCollision(actor, wall, axis);
                    }

                }
            }
            return numCollisionsChecked;
        }

        private int handleSlopedCollisions(GameObjectContainer gameObjectContainer, MovablePhysicsObject actor, Rectangle tileCollideBox, int axis)
        {
            int numCollisionsChecked = 0;
            for (int tx = tileCollideBox.Left; tx <= tileCollideBox.Right; tx++)
            {
                for (int ty = tileCollideBox.Top; ty <= tileCollideBox.Bottom; ty++)
                {
                    Tile wall = gameObjectContainer.getTileAt(tx, ty);
                    if (wall != null && wall.isSloped)
                    {
                        numCollisionsChecked++;
                        handleSlopedCollision(actor, wall, axis);
                    }
                }
            }
            return numCollisionsChecked;
        }

        private bool shouldCheckForRegularCollision(MovablePhysicsObject actor, Tile wall, int axis)
        {
            //If we are on a slope, do not check for a regular collision at all.
            if (!actor.isOnSlope && wall != null && !wall.isSloped && wall.isSolid)
            {
                //If we are not on a slope, and are checking vertical collisions, do it up
                if (axis == VERTICAL_CHECK)
                    return true;

                //If we are checking horizontal collisions, make sure we are not touching an edge. This ensures that
                //the actor does not clip the edge of the slope.
                else if (!actor.isTouchingSlope)
                    return true;
            }
            return false;
        }

        private void resolveRegularCollision(MovablePhysicsObject actor, Tile wall, int axis)
        {
            Pair<CollisionType, float> collision = getCollision(actor, wall, axis);
            if (collision != null)
            {
                switch (collision.key)
                {
                    case CollisionType.LEFT:
                        handleLeftCollision(actor, wall, collision.value);
                        break;
                    case CollisionType.RIGHT:
                        handleRightCollision(actor, wall, collision.value);
                        break;
                    case CollisionType.TOP:
                        handleTopCollision(actor, wall, collision.value);
                        break;
                    case CollisionType.BOTTOM:
                        if (wall.isSloped)
                         Console.WriteLine("BOTTOM: "+new Random().NextDouble());
                        handleBottomCollision(actor, wall, collision.value);
                        break;
                }
                pairPool.returnToPool(collision);
            }
        }

        private void handleLeftCollision(MovablePhysicsObject actor, GameObject wall, float displacement)
        {
            actor.x += displacement;
            if (actor.vx < 0) // Only stops the actor's horizontal movement if the actor is actually moving left.
                actor.stopHorizontalMovement();
        }

        private void handleRightCollision(MovablePhysicsObject actor, GameObject wall, float displacement)
        {
            actor.x -= displacement;
            if (actor.vx > 0) //Only stops the actor's horizintal movement if the actor is actually moving right.
                actor.stopHorizontalMovement();
        }

        private void handleTopCollision(MovablePhysicsObject actor, GameObject wall, float displacement)
        {
            actor.y += displacement;
            actor.vy = 0;
        }

        private void handleBottomCollision(MovablePhysicsObject actor, GameObject wall, float displacement)
        {
            actor.y -= displacement;
            if (actor.vy >= 0) //By checking to see if the object is actually falling, it prevents "snapping" to objects
                actor.vy = 0;
            actor.isOnGround = true;
        }

        private void handleSlopedCollision(MovablePhysicsObject actor, Tile wall, int axis)
        {
            float middleXPosOfActor = (actor.getBoundingBox().Left + (actor.getBoundingBox().width / 2));
            FloatRectangle actorBox = getSlopedRect(middleXPosOfActor, actor.getBoundingBox().Top, 1, actor.getBoundingBox().height);

            //Check if the actor is touching the slope at least
            if (actor.getBoundingBox().Intersects(wall.getBoundingBox()))
                actor.isTouchingSlope = true;

            if (actorBox.Intersects(wall.getBoundingBox()))
            {
                actor.isOnSlope = true;

                if (axis == VERTICAL_CHECK)
                {
                    float feetOfActor = actorBox.Bottom;
                    float yPosOfFoorAtActorsFeet = (wall.getSlope() * (Math.Abs(middleXPosOfActor - wall.getBoundingBox().Left))) + wall.slopeStart.Y;

                    if (yPosOfFoorAtActorsFeet < feetOfActor)
                    {
                        actor.y -= (Math.Abs(yPosOfFoorAtActorsFeet - feetOfActor));
                        actor.vy = 0;
                        actor.isOnGround = true;
                    }
                }
            }

            floatRectanglePool.returnToPool(actorBox);
        }

        private FloatRectangle getSlopedRect(float x, float y, float width, float height)
        {
            FloatRectangle slopedRect = floatRectanglePool.get(x, y, width, height);

            return slopedRect;
        }

        public Pair<CollisionType, float> getCollision(MovablePhysicsObject obj1, Tile obj2, int axis)
        {
            //If the objects are colliding, get the minimum displacement needed to make the object NOT collide
            if (obj1.getBoundingBox().Intersects(obj2.getBoundingBox()))
            {
                Pair<CollisionType, float> firstAxisCollision;
                Pair<CollisionType, float> secondAxisCollision;

                if (axis == HORIZONTAL_CHECK)
                {
                    float leftDisplacement = Math.Abs(obj1.getBoundingBox().Left - obj2.getBoundingBox().Right);
                    firstAxisCollision = pairPool.get(CollisionType.LEFT, leftDisplacement);

                    float rightDisplacement = Math.Abs(obj1.getBoundingBox().Right - obj2.getBoundingBox().Left);
                    secondAxisCollision = pairPool.get(CollisionType.RIGHT, rightDisplacement);
                }
                else
                {
                    float topDisplacement = Math.Abs(obj1.getBoundingBox().Top - obj2.getBoundingBox().Bottom);
                    firstAxisCollision = pairPool.get(CollisionType.TOP, topDisplacement);

                    float bottomDisplacement = Math.Abs(obj1.getBoundingBox().Bottom - obj2.getBoundingBox().Top);
                    secondAxisCollision = pairPool.get(CollisionType.BOTTOM, bottomDisplacement);
                }

                return getQuickMinimumDisplacement(firstAxisCollision, secondAxisCollision);
            }
            return null;
        }

        private Pair<CollisionType, float> getQuickMinimumDisplacement(Pair<CollisionType, float> collisionOne, Pair<CollisionType, float> collisionTwo)
        {
            if (collisionOne.value > 0 || collisionTwo.value > 0)
            {
                if (collisionOne.value > 0 && collisionOne.value < collisionTwo.value)
                {
                    pairPool.returnToPool(collisionTwo);
                    return collisionOne;
                }
                else if (collisionTwo.value > 0)
                {
                    pairPool.returnToPool(collisionOne);
                    return collisionTwo;
                }
                else
                {
                    pairPool.returnToPool(collisionOne, collisionTwo);
                }
            }
            else
            {
                pairPool.returnToPool(collisionOne, collisionTwo);
            }

            return null;
        }

        /**
         * This method simply checks if the two objects are colliding. If not, then it returns CollisionType.NONE
         **/
        public CollisionType getSpeedyCollision(GameObject obj1, GameObject obj2)
        {
            return obj1.getBoundingBox().Intersects(obj2.getBoundingBox()) ? CollisionType.BOTTOM : CollisionType.NONE;
        }
    }
}
