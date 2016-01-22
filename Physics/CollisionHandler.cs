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

namespace PlatformerGame.Physics
{
    class CollisionHandler
    {
        public const int HORIZONTAL_CHECK = 0;
        public const int VERTICAL_CHECK = 1;
        public const float THRESHOLD = .005f;

        private List<Interactable> interactiblesCloseToPlayer = null;

        public List<Interactable> getInteractablesTouchingPlayer()
        {
            return interactiblesCloseToPlayer;
        }

        public int handlePhysicsObjectCollisions(GameObjectContainer container, int axis)
        {
            return handleWallCollisionsForAxis(container, container.physicsObjects, axis);
        }

        /**
         * This method handles "collisions" with actors and setpeices (NPCs, items, things that actors can interact with)
         **/
        public void handleSetpeiceCollisions(ISet<Interactable> interactables, Player player)
        {
            //Reset the list of actors and set peices that are close to each other, in case some of them moved
            this.interactiblesCloseToPlayer = new List<Interactable>();

            foreach (Interactable peice in interactables)
            {
                if (getSpeedyCollision(player, peice) != CollisionType.NONE)
                {
                    //Add the actor->setpeice pair to the list of objects that are in proximity to each other
                    interactiblesCloseToPlayer.Add(peice);
                }
            }
        }

        public void handleParticleCollisions()
        {

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
        private int handleWallCollisionsForAxis(GameObjectContainer gameObjectContainer, ISet<MovablePhysicsObject> actors, int axis)
        {
            int numCollisionsChecked = 0;
            foreach (MovablePhysicsObject actor in actors)
            {
                Rectangle tileCollideBox = gameObjectContainer.getBoundingBoxForPossibleCollisions(actor);
                List<Tile> collidableTiles = sortTiles(gameObjectContainer, tileCollideBox);
                foreach (Tile wall in collidableTiles)
                {
                    if (wall != null && wall.isSolid)
                    {
                        numCollisionsChecked++;
                        if (wall.isSloped)
                        {
                            handleSlopedCollision(actor, wall, axis);
                        }
                        else if (shouldCheckForRegularCollision(actor, wall, axis))
                        {
                            handleRegularCollision(actor, wall, axis);
                        }
                    }
                }
            }
            return numCollisionsChecked;
        }

        private bool shouldCheckForRegularCollision(MovablePhysicsObject actor, Tile wall, int axis)
        {
            //If we are on a slope, do not check for a regular collision at all.
            if (!actor.isOnSlope)
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

        private void handleRegularCollision(MovablePhysicsObject actor, Tile wall, int axis)
        {
            KeyValuePair<CollisionType, float> collision = getCollision(actor, wall, axis);
            if (collision.Key != CollisionType.NONE)
            {
                Console.WriteLine(actor.isOnSlope);
                switch (collision.Key)
                {
                    case CollisionType.LEFT:
                        handleLeftCollision(actor, wall, collision.Value);
                        break;
                    case CollisionType.RIGHT:
                        handleRightCollision(actor, wall, collision.Value);
                        break;
                    case CollisionType.TOP:
                        handleTopCollision(actor, wall, collision.Value);
                        break;
                    case CollisionType.BOTTOM:
                        handleBottomCollision(actor, wall, collision.Value);
                        break;
                }
            }
        }

        private void handleLeftCollision(MovablePhysicsObject actor, GameObject wall, float displacement)
        {
            if (Math.Abs(displacement) > THRESHOLD)
            { 
                actor.x += displacement;
                if (actor.vx < 0) // Only stops the actor's horizontal movement if the actor is actually moving left.
                    actor.stopHorizontalMovement();
            }
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
           // Console.WriteLine(displacement);
            actor.y -= displacement;
            if (actor.vy >= 0) //By checking to see if the object is actually falling, it prevents "snapping" to objects
                actor.vy = 0;
            actor.isOnGround = true;
        }

        private void handleSlopedCollision(MovablePhysicsObject actor, Tile wall, int axis)
        {
            float middleXPosOfActor = (actor.getBoundingBox().Left + (actor.getBoundingBox().width / 2));
            FloatRectangle actorBox = new FloatRectangle(middleXPosOfActor, actor.getBoundingBox().Top, 1, actor.getBoundingBox().height);

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
        }

        public KeyValuePair<CollisionType, float> getCollision(MovablePhysicsObject obj1, Tile obj2, int axis)
        {
            Dictionary<CollisionType, float> oppositeDists = new Dictionary<CollisionType, float>();
            KeyValuePair<CollisionType, float> minCollision = new KeyValuePair<CollisionType, float>(CollisionType.NONE, 0);

            //If the objects are colliding, get the minimum displacement needed to make the object NOT collide
            if (obj1.getBoundingBox().Intersects(obj2.getBoundingBox()))
            {
                if (axis == HORIZONTAL_CHECK && !obj2.isNextToSlope)
                {
                    oppositeDists.Add(CollisionType.LEFT, Math.Abs(obj1.getBoundingBox().Left - obj2.getBoundingBox().Right));
                    oppositeDists.Add(CollisionType.RIGHT, Math.Abs(obj1.getBoundingBox().Right - obj2.getBoundingBox().Left));   
                }
                else if (axis == VERTICAL_CHECK)
                {
                    oppositeDists.Add(CollisionType.TOP, Math.Abs(obj1.getBoundingBox().Top - obj2.getBoundingBox().Bottom));
                    oppositeDists.Add(CollisionType.BOTTOM, Math.Abs(obj1.getBoundingBox().Bottom - obj2.getBoundingBox().Top));
                }
                //Get minimum dist
                CollisionType minimumCollisionType = getMinimumDisplacement(oppositeDists);
                if (minimumCollisionType != CollisionType.NONE)
                {
                    minCollision = new KeyValuePair<CollisionType, float>(minimumCollisionType, oppositeDists[minimumCollisionType]);
                }
            }
            return minCollision;
        }

        private CollisionType getMinimumDisplacement(Dictionary<CollisionType, float> oppositeDists)
        {
            CollisionType minimumKey = CollisionType.NONE;
            float minimum = Int32.MaxValue;
            foreach (CollisionType type in oppositeDists.Keys)
            {
                float displacement = oppositeDists[type];
                if (displacement < minimum && displacement > 0)
                {
                    minimumKey = type;
                    minimum = displacement;
                }
            }

            return minimumKey;
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
