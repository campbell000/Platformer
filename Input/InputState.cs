using System;
using Microsoft.Xna.Framework.Input;
namespace PlatformerGame
{
	/**
	 * This class processes the state of the keyboard and maps actions in the game to each input. It is meant to abstract the logic of detecting specific keys.
	 **/
	public class InputState
	{
		private bool [] inputs {get;set;}
		public const int UP = 0;
		public const int DOWN = 1;
		public const int LEFT = 2;
		public const int RIGHT = 3;
		public const int JUMP = 4;
		public const int ATTACK = 5;
        public const int JUMP_RELEASED = 6;

		public InputState ()
		{
			KeyboardState state = Keyboard.GetState();
			bool [] inputs = new bool[7];

			inputs[UP] = state.IsKeyDown(Keys.Up);
			inputs[DOWN] = state.IsKeyDown(Keys.Down);
			inputs[LEFT] = state.IsKeyDown(Keys.Left);
			inputs[RIGHT] = state.IsKeyDown(Keys.Right);
			inputs[JUMP] = state.IsKeyDown(Keys.Up);
			inputs[ATTACK] = state.IsKeyDown(Keys.Space);
            inputs[JUMP_RELEASED] = state.IsKeyUp(Keys.Up);

			this.inputs = inputs;
		}

		public bool jumpWasPressed()
		{
			return inputs[JUMP];
		}

		public bool attackWasPressed()
		{
			return inputs[ATTACK];
		}

		public bool upWasPressed()
		{
			return inputs[UP];
		}

		public bool downWasPressed()
		{
			return inputs[DOWN];
		}

		public bool leftWasPressed()
		{
			return inputs[LEFT];
		}

		public bool rightWasPressed()
		{
			return inputs[RIGHT];
		}

        public bool jumpWasReleased()
        {
            return inputs[JUMP_RELEASED];
        }
	}
}

