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
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Storage;
using PlatformerGame.GameObjects.Impl;
using PlatformerGame.Utils;
using PlatformerGame.Cameras;
using PlatformerGame.GameStates;
using PlatformerGame.Utils.Pools;
using PlatformerGame.GameObjects.VisualObjects;
using PlatformerGame.GameStateManagers;
using PlatformerGame.Draw;

namespace PlatformerGame
{
	public class BigFuckingGameClass
	{
        public SpriteFontContainer fonts;
        public GameObjectContainer gameObjects { get; set; }
        public InputState inputState = new InputState();
        private StateManager currentStateManager;
        private DialogManager dialogManager;
        public PlayStateManager playStateManager { get; set; }
        public Interactable objectToInteractWith;

        public BigFuckingGameClass(GameObjectContainer container, SpriteFontContainer fonts)
        {
            this.fonts = fonts;
            gameObjects = container;
            dialogManager = new DialogManager(gameObjects, fonts.dialogFont);
            playStateManager = new PlayStateManager(gameObjects);
            currentStateManager = playStateManager;
        }

        public Player getPlayer()
        {
            return gameObjects.getPlayer();
        }

		public void update(GameTime delta)
		{
            //First and foremost, update the input state,
            inputState.updateInputState();

            //Update based on the state of the game
            currentStateManager.updateState(delta, inputState);

            //Change State Managers if the state indicates that it wants to exit
            if (currentStateManager.willExitState())
                handleTransition(currentStateManager);
		}

        private void handleTransition(StateManager currentManager)
        {
            GameState targetState = currentManager.getStateToEnter();

            //PLAY => DIALOG
            if (currentManager.getThisState() == GameState.PLAY && currentManager.getStateToEnter() == GameState.DIALOG)
            {
                dialogManager.prepareToEnterState(playStateManager.getObjectToInteractWith());
                currentStateManager = dialogManager;
            }

            //DIALOG => PLAY
            if (currentManager.getThisState() == GameState.DIALOG && currentManager.getStateToEnter() == GameState.PLAY)
            {
                currentStateManager = playStateManager;
            }
        }

		public void draw(GameTime t,SpriteBatch batch)
		{
            Drawer.itemsDrawnInThisFrame = 0;
            for(int i = 0; i < gameObjects.allObjects.Count; i++)
            {
                Drawer.drawObject(playStateManager.camera, t, batch, gameObjects.allObjects[i]);
            }

            //Make sure we draw the player last (RIGHT NOW WE ARE DRAWING THE PLAYER TWICE. IS THIS BAD?)
            Drawer.drawObject(playStateManager.camera, t, batch, gameObjects.getPlayer());

            //Draw the HUD objects last, as they need to be at the forefront of the screen
            for (int i = 0; i < gameObjects.HUDObjects.Count; i++)
            {
                Drawer.drawObject(playStateManager.camera, t, batch, gameObjects.HUDObjects[i]);
            }

            //Draw anything related to the specific state
            currentStateManager.drawStateSpecificObjects(t, batch);
		}

        public int getNumGameObjects()
        {
            return gameObjects.getAllGameObjects().Count;
        }

        public int getNumObjectsDrawn()
        {
            return Drawer.itemsDrawnInThisFrame;
        }
	}
}

