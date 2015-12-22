using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PlatformerGame.GameObjects;

namespace PlatformerGame
{
    class Drawer
    {
        private static FrameCounter _frameCounter = new FrameCounter();

        public static void drawObject(GameTime deltaTime, SpriteBatch spriteBatch, GameObject gameObject)
        {
            if (gameObject.texture != null && gameObject.rows > 0 && gameObject.columns > 0)
            {
                int width = gameObject.texture.Width / gameObject.columns;
                int height = gameObject.texture.Height / gameObject.rows;
                int row = (int)((float)gameObject.currentFrame / (float)gameObject.columns);
                int column = gameObject.currentFrame % gameObject.columns;

                Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
                Rectangle destinationRectangle = new Rectangle((int)gameObject.x, (int)gameObject.y, width, height);

                spriteBatch.Draw(gameObject.texture, destinationRectangle, sourceRectangle, Color.White);
            }
            else
            {
                Vector2 position = new Vector2(gameObject.x, gameObject.y);
                spriteBatch.Draw(gameObject.texture, position);
            }
        }

        public static void drawDebug(SpriteFont font, GameTime time, SpriteBatch spriteBatch, GameObject objToTrack)
        {
            var pos = "x: " + objToTrack.x + ", y: " + objToTrack.y;
            var v = "vx: " + objToTrack.vx + ", vy: " + objToTrack.vy;
            var a = "ax: " + objToTrack.ax + ", ay: " + objToTrack.ay;
            if (font != null)
            {
                spriteBatch.DrawString(font, pos, new Vector2(1, 40), Color.Black);
                spriteBatch.DrawString(font, v, new Vector2(1, 80), Color.Black);
                spriteBatch.DrawString(font, a, new Vector2(1, 120), Color.Black);
            }

            var fps = string.Format("FPS: {0}", _frameCounter.AverageFramesPerSecond);
            var deltaTime = (float)time.ElapsedGameTime.TotalSeconds;
            _frameCounter.Update(deltaTime);
            spriteBatch.DrawString(font, fps, new Vector2(1, 1), Color.Black);
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
