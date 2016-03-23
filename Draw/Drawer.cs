using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PlatformerGame.GameObjects.CollisionObjects.MovablePhysicsObjects.Actors;
using PlatformerGame.GameObjects;
using PlatformerGame.Cameras;
using PlatformerGame.Utils;
using PlatformerGame.Utils.Pools;

namespace PlatformerGame
{
    class Drawer
    {
        private static FrameCounter _frameCounter = new FrameCounter();
        private static RectanglePool rectanglePool = new RectanglePool();
        public static int itemsDrawnInThisFrame = 0;
        private static Vector2 vector = new Vector2(0, 0);

        //SET OBJECTS BEING DRAWN IN TERMS OF THE POSITION OF THE CAMERA
        public static void drawObject(Camera camera, GameTime deltaTime, SpriteBatch spriteBatch, GameObject gameObject)
        {
            if (gameObject.immuneToCamera || camera.canSeeObject(gameObject))
            {
                itemsDrawnInThisFrame++;
                if (gameObject.texture != null && gameObject.rows > 0 && gameObject.columns > 0)
                {
                    int width = gameObject.texture.Width / gameObject.columns;
                    int height = gameObject.texture.Height / gameObject.rows;
                    int row = (int)((float)gameObject.currentFrame / (float)gameObject.columns);
                    int column = gameObject.currentFrame % gameObject.columns;

                    Rectangle sourceRectangle = rectanglePool.get(width * column, height * row, width, height);
                    Rectangle drawingRectangle = getDrawingRectangle(camera, gameObject);
                    spriteBatch.Draw(gameObject.texture, drawingRectangle, sourceRectangle, Color.White);

                    //Cleanup the rects we used
                    rectanglePool.returnToPool(sourceRectangle, drawingRectangle);
                }
                else
                {
                    Vector2 position = getVector(gameObject.x, gameObject.y);
                    spriteBatch.Draw(gameObject.texture, position);
                }
            }
        }

        protected static Rectangle getDrawingRectangle(Camera camera, GameObject gameObject)
        {
            //Adjust X and Y for camera position
            int adjustedX = gameObject.immuneToCamera ? (int)gameObject.x : (int)(gameObject.x - camera.x);
            int adjustedY = gameObject.immuneToCamera ? (int)gameObject.y : (int)(gameObject.y - camera.y);
            Rectangle drawingRectangle =  rectanglePool.get(adjustedX, adjustedY, (int)gameObject.width, (int)gameObject.height);

            return drawingRectangle;
        }

        public static void drawText(SpriteFont font, GameTime time, SpriteBatch spriteBatch, String text, int x, int y)
        {
            spriteBatch.DrawString(font, text, getVector(x, y), Color.White);
        }

        public static void drawText(SpriteFont font, GameTime time, SpriteBatch spriteBatch, StringBuilder text, int x, int y)
        {
            spriteBatch.DrawString(font, text, getVector(x, y), Color.White);
        }

        public static void drawDebug(SpriteFont font, GameTime time, SpriteBatch spriteBatch, Actor objToTrack, int num, int numCollisions, int numDrawn, Boolean minDebug)
        {
            var GC = "Total bytes allocated: " + (System.GC.GetTotalMemory(false) / 1000) + " kb";
            spriteBatch.DrawString(font, GC, getVector(1, 240), Color.White);
            if (!minDebug)
            {
                var pos = "x: " + objToTrack.x + ", y: " + objToTrack.y;
                spriteBatch.DrawString(font, pos, getVector(1, 30), Color.White);

                var v = "vx: " + objToTrack.vx + ", vy: " + objToTrack.vy;
                spriteBatch.DrawString(font, v, getVector(1, 65), Color.White);

                var a = "ax: " + objToTrack.ax + ", ay: " + objToTrack.ay;
                spriteBatch.DrawString(font, a, getVector(1, 100), Color.White);

                var numObjs = "GameObjects: " + num;
                spriteBatch.DrawString(font, numObjs, getVector(1, 135), Color.White);

                var numColl = "Number Of Tile Collisions: " + numCollisions;
                spriteBatch.DrawString(font, numColl, getVector(1, 170), Color.White);

                var drawn = "Objects Drawn: " + numDrawn;
                spriteBatch.DrawString(font, drawn, getVector(1, 205), Color.White);

                var fps = string.Format("FPS: {0}", _frameCounter.AverageFramesPerSecond);
                var deltaTime = (float)time.ElapsedGameTime.TotalSeconds;
                _frameCounter.Update(deltaTime);
                spriteBatch.DrawString(font, fps, Utils.Utils.setVector(vector, 1, 1), Color.White);
            }

            
            
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

        private static Vector2 getVector(int x, int y)
        {
            return Utils.Utils.setVector(vector, x, y);
        }

        private static Vector2 getVector(float x, float y)
        {
            return Utils.Utils.setVector(vector, x, y);
        }
    }
}
