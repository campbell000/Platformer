using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlatformerGame.GameObjects;
using PlatformerGame.GameStates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformerGame.GameStateManagers
{
    public abstract class StateManager
    {
        protected GameObjectContainer gameObjects;
        protected GameState stateToEnter;
        protected bool isExitingState;
        protected GameState thisState;

        public StateManager(GameObjectContainer goc)
        {
            this.gameObjects = goc;
        }

        public virtual bool willExitState()
        {
            return isExitingState;
        }

        public virtual GameStates.GameState getStateToEnter()
        {
            return stateToEnter;
        }

        public abstract GameState getManagerType();

        public abstract void drawStateSpecificObjects(GameTime t, SpriteBatch batch);

        public GameState getThisState()
        {
            return thisState;
        }

        public abstract void updateState(GameTime delta, InputState inputState);

        public abstract void prepareToEnterState(GameObject objectToPass);
    }
}
