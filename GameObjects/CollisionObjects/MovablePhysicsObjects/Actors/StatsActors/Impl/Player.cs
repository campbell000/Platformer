using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;

namespace PlatformerGame.GameObjects.CollisionObjects.MovablePhysicsObjects.Actors.StatsActors.Impl
{
	public class Player : StatsActor
	{
		public InputState prevInputState;
        public InputState currInputState;
        public PlayerPhysicsHandler physicsHandler;
        
        public bool jumpWasReleased = false;
        public double millisJumpHeld = 0;
        public float JUMP_FORCE = -1.3f;

        public bool canJumpAgain = true;

		public Player (Texture2D texture, int rows, int columns, float x, float y, float width, float height) : base(texture, rows, columns,
			x, y, width, height)
		{
            this.physicsHandler = new PlayerPhysicsHandler(this);
		}

		public void processInputs(InputState state)
		{
			prevInputState = currInputState;
			currInputState = state;
		}
			
		public override void update(GameTime deltaTime)
		{
            physicsHandler.adjustForInputs(deltaTime);
			base.update(deltaTime);
            physicsHandler.adjustPhysicsCalculations();
		}

        protected override void adjustXVelocity()
        {
            physicsHandler.adjustVelocity();
        }
	}
}

