using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using PlatformerGame.Utils;

namespace PlatformerGame
{
	public class GameObject : IGameObject
	{
		/* x position of object */
		public float x {get; set;}

		/* y position of object */
        public float y { get; set; }

        public float prevX { get; set; }

        public float prevY { get; set; }

		/* x velocity of object */
        public float vx { get; set; }

		/* y velocity of object */
        public float vy { get; set; }

		/* x acceleration of object */
        public float ax { get; set; }

		/* y acceleration of object */
        public float ay { get; set; }

        public float xDrag { get; set; }
        public float yDrag { get; set; }

		/* width of object */
        public float width { get; set; }

		/* height of object */
        public float height { get; set; }

		/* 2D Texture of the object */
		public Texture2D texture { get; set; }

		/* Number of rows in the sprite sheet */
        public int rows { get; set; }

		/* Number of columns in the sprite sheet */
		public int columns { get; set; }

		/* Current frame counter. This corresponds to the number of frames passed in a loop, NOT the current tile in the spritesheet */
        public int frameCounter;

		/* Total number of tiles in the spritesheet */
        public int totalFrames;

		/* Current tile number in the spritesheet */
        public int currentFrame;

		/* Number of frames that a tile is displayed on screen (default is 1) */
        public int frameScaler { get; set; }

		public GameObject(Texture2D texture, int rows, int columns, float x, float y, float width, float height)
		{
			initFrameVars(columns, rows);
			this.texture = texture;
			this.x = x;
			this.y = y;
			this.width = width;
			this.height = height;
            this.xDrag = 1;
            this.yDrag = 1;
		}

		private void initFrameVars(int columns, int rows)
		{
			frameScaler = 1;
			frameCounter = 0;
			currentFrame = 0;
			this.rows = rows;
			this.columns = columns;
			totalFrames = rows * columns;
		}

		/**
		 * This method updates the current frame of the animation
		 **/
		protected virtual void updateAnimation()
		{
			frameCounter++;
			if (frameCounter/2 == totalFrames)
				frameCounter = 0;

			currentFrame = frameCounter / 2;
		}

		public virtual void update(GameTime deltaTime)
		{
			updateAnimation();
		}

        public FloatRectangle getBoundingBox()
        {
            return new FloatRectangle(x, y, width, height);
        }
	}
}

