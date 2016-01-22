using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PlatformerGame.GameObjects.CollisionObjects.MovablePhysicsObjects.Actors.StatsActors.Impl;
using PlatformerGame.GameObjects.CollisionObjects.MovablePhysicsObjects;
using PlatformerGame.GameObjects.CollisionObjects.MovablePhysicsObjects.Actors;
using PlatformerGame.GameObjects.CollisionObjects.MovablePhysicsObjects.NonActors.Interactables;
using PlatformerGame.GameObjects.CollisionObjects.Impl;
using PlatformerGame.GameObjects;
using PlatformerGame.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Storage;
using PlatformerGame.GameObjects.Impl;
using PlatformerGame.Utils;
using PlatformerGame.Cameras;

namespace PlatformerGame
{
	public class BigFuckingGameClass
	{
        //Objects that there will only be one of on the entire screen
        InteractIndicator indicator;
        GameObjectContainer gameObjects { get; set; }
        Camera camera;
        public int numCollisionsChecked { get; set; }

        private CollisionHandler collisionHandler;

		public BigFuckingGameClass (Player player)
		{
            gameObjects = new GameObjectContainer(player);
            collisionHandler = new CollisionHandler();
            camera = new Camera(gameObjects);
            camera.centerAndFollowObject(player);
		}

        public BigFuckingGameClass()
        {
            gameObjects = new GameObjectContainer();
            collisionHandler = new CollisionHandler();
            camera = new Camera(gameObjects);
        }

        public BigFuckingGameClass(GameObjectContainer container)
        {
            gameObjects = container;
            collisionHandler = new CollisionHandler();
            camera = new Camera(gameObjects);
            camera.centerAndFollowObject(container.getPlayer());
        }

        public Player getPlayer()
        {
            return gameObjects.getPlayer();
        }

		public void initTextures(ContentManager Content)
		{
            indicator = new InteractIndicator(Content, 1, 1, 0, 0, 50, 50);
		}

		public void update(GameTime delta)
		{
			//First, gather inputs and update player accordingly.
			gameObjects.getPlayer().processInputs(new InputState(), delta);

			//Second, update movement of the game objects
            updateMovement(delta);

            //Third, update the state of all GameObjects based on collisions, time, etc
            foreach (GameObject o in gameObjects.getAllGameObjects())
            {
                if (o != null)
                    o.updateState();
            }

            //Fourth, update the camera based on the player's position
            camera.updateCameraPosition();
		}

        /**
         * We need to update the axes seperately. See http://gamedev.stackexchange.com/questions/69339/2d-aabbs-and-resolving-multiple-collisions
         **/
        private void updateMovement(GameTime delta)
        {
            //First, update the horizontal movement of all physics objects and adjust for any collisions
            foreach (MovablePhysicsObject o in gameObjects.physicsObjects)
            {
                //Reset variables that need to be reset
                o.isOnSlope = false;
                o.isTouchingSlope = false;
                o.isOnGround = false;

                o.updateHorizontalMovement(delta);
            }
            int num = collisionHandler.handlePhysicsObjectCollisions(gameObjects, CollisionHandler.HORIZONTAL_CHECK);

            //Second, update the vertical movement of all physic s objects
            foreach (MovablePhysicsObject o in gameObjects.physicsObjects)
            {
                o.updateVerticalMovement(delta);
            }
            num += collisionHandler.handlePhysicsObjectCollisions(gameObjects, CollisionHandler.VERTICAL_CHECK);

            numCollisionsChecked = num;
        }

		public void draw(GameTime t,SpriteBatch batch)
		{
            foreach(GameObject o in gameObjects.getAllGameObjects())
            {
                if (o != null && o.onScreen)
                    Drawer.drawObject(camera, t, batch, o);
            }

            //Make sure we draw the player last (RIGHT NOW WE ARE DRAWING THE PLAYER TWICE. IS THIS BAD?)
            Drawer.drawObject(camera, t, batch, gameObjects.getPlayer());
		}

        public int getNumGameObjects()
        {
            return gameObjects.getAllGameObjects().Count;
        }

        public void addInteractable(Interactable i)
        {
            gameObjects.addInteractable(i);
        }
	}
}

