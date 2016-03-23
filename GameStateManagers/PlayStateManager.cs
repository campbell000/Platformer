using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlatformerGame.GameObjects;
using Microsoft.Xna.Framework;
using PlatformerGame.Cameras;
using PlatformerGame.Physics;
using PlatformerGame.GameObjects.CollisionObjects.MovablePhysicsObjects.NonActors.Interactables;
using PlatformerGame.GameObjects.CollisionObjects.MovablePhysicsObjects.Actors.StatsActors.Impl;
using PlatformerGame.GameObjects.VisualObjects;
using Microsoft.Xna.Framework.Graphics;
using PlatformerGame.GameStates;

namespace PlatformerGame.GameStateManagers
{
    public class PlayStateManager : StateManager
    {
        public Camera camera { get; set; }
        public int numCollisionsChecked { get; set; }
        private CollisionHandler collisionHandler;
        private Interactable objectPlayerCanInteractWith = null;
        private bool canInteract = true;

        public PlayStateManager(GameObjectContainer container)
            : base(container)
        {
            collisionHandler = new CollisionHandler();
            camera = new Camera(gameObjects);
            camera.centerAndFollowObject(container.getPlayer());
            this.thisState = GameState.PLAY;
        }

        public override void updateState(GameTime delta, InputState inputState)
        {
            this.isExitingState = false;
            gameObjects.removeVisualEffect(gameObjects.interactionIndicator);

            //First, gather inputs and update player accordingly.
            gameObjects.getPlayer().processInputs(inputState, delta);

            if (inputState.interactWasReleased())
                canInteract = true;

            //Secondly, check to see if the user wants to interact with something.
            if (canInteract && inputState.interactWasPressed() && objectPlayerCanInteractWith != null)
            {
                this.stateToEnter = GameStates.GameState.DIALOG;
                this.isExitingState = true;
                canInteract = false;
            }
            else
            {
                //Second, update movement of the game objects
                updateMovement(delta);

                //Third, update the state of all GameObjects based on collisions, time, interactables in proximity, etc.
                for (int i = 0; i < gameObjects.allObjects.Count; i++)
                {
                    if (gameObjects.allObjects[i] != null)
                        gameObjects.allObjects[i].updateState();
                }

                //Fourth, check for collisions between the player and interactive objects
                collisionHandler.handleSetpeiceCollisions(gameObjects.interactables, gameObjects.player);
                objectPlayerCanInteractWith = collisionHandler.getInteractableTouchingPlayer();

                //Fourth, display the indicator if the user can interact with something
                if (objectPlayerCanInteractWith != null)
                    displayIndicator();

                //Fifth, update the camera based on the player's position
                camera.updateCameraPosition();
            }
        }

        private void displayIndicator()
        {
            Player p = gameObjects.player;
            VisualObject indicator = gameObjects.interactionIndicator;
            gameObjects.interactionIndicator.setPos(p.getXPosToCenterObject(indicator), p.getYPosToCenterObject(indicator) - p.height);
            gameObjects.addVisualEffect(gameObjects.interactionIndicator);
        }

        /**
         * We need to update the axes seperately. See http://gamedev.stackexchange.com/questions/69339/2d-aabbs-and-resolving-multiple-collisions
         **/
        private void updateMovement(GameTime delta)
        {
            //First, update the horizontal movement of all physics objects and adjust for any collisions
            for (int i = 0; i < gameObjects.physicsObjects.Count; i++)
            {
                //Reset collision state of the object every update
                gameObjects.physicsObjects[i].resetCollisionState();

                //Update Horizontal Movement
                gameObjects.physicsObjects[i].updateHorizontalMovement(delta);
            }
            int num = collisionHandler.handlePhysicsObjectCollisions(gameObjects, CollisionHandler.HORIZONTAL_CHECK);

            //Second, update the vertical movement of all physic s objects
            for (int i = 0; i < gameObjects.physicsObjects.Count; i++)
            {
                gameObjects.physicsObjects[i].updateVerticalMovement(delta);
            }
            num += collisionHandler.handlePhysicsObjectCollisions(gameObjects, CollisionHandler.VERTICAL_CHECK);

            numCollisionsChecked = num;
        }

        public Interactable getObjectToInteractWith()
        {
            return objectPlayerCanInteractWith;
        }

        public override GameStates.GameState getManagerType()
        {
            return GameStates.GameState.PLAY;
        }

        public override void drawStateSpecificObjects(GameTime t, SpriteBatch batch)
        {
            //throw new NotImplementedException();
        }

        public override void prepareToEnterState(GameObject objectToPass)
        {
            //throw new NotImplementedException();
        }
    }
}
