using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PlatformerGame.GameObjects.CollisionObjects.MovablePhysicsObjects.Actors;
using PlatformerGame.GameObjects;
using PlatformerGame.Cameras;

namespace PlatformerGame
{
    class Drawer
    {
        private static FrameCounter _frameCounter = new FrameCounter();
        private static Rectangle drawingRectangle = new Rectangle();

        //SET OBJECTS BEING DRAWN IN TERMS OF THE POSITION OF THE CAMERA
        public static void drawObject(Camera camera, GameTime deltaTime, SpriteBatch spriteBatch, GameObject gameObject)
        {
            if (gameObject.texture != null && gameObject.rows > 0 && gameObject.columns > 0)
            {
                int width = gameObject.texture.Width / gameObject.columns;
                int height = gameObject.texture.Height / gameObject.rows;
                int row = (int)((float)gameObject.currentFrame / (float)gameObject.columns);
                int column = gameObject.currentFrame % gameObject.columns;

                Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
                spriteBatch.Draw(gameObject.texture, getDrawingRectangle(camera, gameObject), sourceRectangle, Color.White);
            }
            else
            {
                Vector2 position = new Vector2(gameObject.x, gameObject.y);
                spriteBatch.Draw(gameObject.texture, position);
            }
        }

        protected static Rectangle getDrawingRectangle(Camera camera, GameObject gameObject)
        {
            //Adjust X and Y for camera position
            int adjustedX = (int)(gameObject.x - camera.x);
            int adjustedY = (int)(gameObject.y - camera.y);
            drawingRectangle.X = adjustedX;
            drawingRectangle.Y = adjustedY;
            drawingRectangle.Width = (int)gameObject.width;
            drawingRectangle.Height = (int)gameObject.height;

            return drawingRectangle;
        }

        public static void drawDebug(SpriteFont font, GameTime time, SpriteBatch spriteBatch, Actor objToTrack, int num, int numCollisions)
        {
            var pos = "x: " + objToTrack.x + ", y: " + objToTrack.y;
            var v = "vx: " + objToTrack.vx + ", vy: " + objToTrack.vy;
            var a = "ax: " + objToTrack.ax + ", ay: " + objToTrack.ay;
            var numObjs = "GameObjects: " + num;
            var numColl = "Number Of Tile Collisions: " + numCollisions;
            var onSlope = "On Slope: " + objToTrack.isOnSlope;
            var canJ = "Is on Ground: " + objToTrack.isOnGround;

            if (font != null)
            {
                spriteBatch.DrawString(font, pos, new Vector2(1, 40), Color.White);
                spriteBatch.DrawString(font, v, new Vector2(1, 80), Color.White);
                spriteBatch.DrawString(font, a, new Vector2(1, 120), Color.White);
                spriteBatch.DrawString(font, numObjs, new Vector2(1, 160), Color.White);
                spriteBatch.DrawString(font, numColl, new Vector2(1, 200), Color.White);
                spriteBatch.DrawString(font, onSlope, new Vector2(1, 240), Color.White);
                spriteBatch.DrawString(font, canJ, new Vector2(1, 280), Color.White);
            }

            var fps = string.Format("FPS: {0}", _frameCounter.AverageFramesPerSecond);
            var deltaTime = (float)time.ElapsedGameTime.TotalSeconds;
            _frameCounter.Update(deltaTime);
            spriteBatch.DrawString(font, fps, new Vector2(1, 1), Color.White);
        }

        public static Texture2D make2DRect(GraphicsDevice device, int width, int height)
        {
            return make2DRect(device, width, height, Color.Black); 
        }

        public static Texture2D make2DRect(GraphicsDevice device, int width, int height, Color color)
        {
            Texture2D t = new Texture2D(device, width, height);
            Color[] d = new Color[width * height];
            for (int i = 0; i < width * height; i++)
                d[i] = color;
            t.SetData<Color>(d);
            return t;
        }
    }
}
