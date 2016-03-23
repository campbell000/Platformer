using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using PlatformerGame.Utils;

namespace PlatformerGame
{
	public class GameObject
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

        /* Indicates whether or not the object should be in the game world */
        public bool active { get; set; }

        public bool isSolid { get; set; }

        public bool immuneToCamera { get; set; }

        /* Set this variable to a non-zero variable to make this object's bounding box bigger or smaller than
         * it's actual width and height
         */
        public int boundingBoxOffset { get; set; }

        private FloatRectangle boundingBox = new FloatRectangle();

        private Vector2 centerPosition = new Vector2();

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
            active = true;
            isSolid = true;
            immuneToCamera = false;
		}

        public GameObject(Texture2D texture, int rows, int columns, float x, float y, float width, float height, int boundingBoxOffset)
        {
            initFrameVars(columns, rows);
            this.texture = texture;
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.xDrag = 1;
            this.yDrag = 1;
            active = true;
            isSolid = true;
            this.boundingBoxOffset = boundingBoxOffset;
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
            if (frameCounter / 2 == totalFrames)
                frameCounter = 0;

            currentFrame = frameCounter / 2;
        }

        public virtual FloatRectangle getBoundingBox()
        {
            boundingBox.x = x;
            boundingBox.y = y;
            boundingBox.width = width;
            boundingBox.height = height;
            return boundingBox;
        }

        public virtual void updateState()
        {
            this.updateAnimation();
        }

        public void destroy()
        {
            this.active = false;
        }

        public Vector2 getCenter()
        {
            centerPosition.X = (x + (width / 2)); 
            centerPosition.Y = (y + (height / 2));

            return centerPosition;
        }

        public float getXPosToCenterObject(GameObject o)
        {
            Vector2 center = getCenter();
            return center.X - (o.width / 2);
        }

        public float getYPosToCenterObject(GameObject o)
        {
            Vector2 center = getCenter();
            return center.Y - (o.height / 2);
        }

        public void setPos(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
	}
}

