using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PlatformerGame.Utils
{
    class Utils
    {
        public static float getXPositionForCenteredObject(GameObject objToCenter, GameObject referenceObject)
        {
            float length = referenceObject.width - objToCenter.width;
            return referenceObject.x + (length / 2);
        }

        public static float getYPositionForDistanceAbove(GameObject objToGoAbove, GameObject referenceObject, float distanceBetween)
        {
            float yPosToBarelyTouch = referenceObject.y - objToGoAbove.height;
            return yPosToBarelyTouch - distanceBetween;
        }

        public static Rectangle setRectangle(Rectangle rectangle, int x, int y, int width, int height)
        {
            rectangle.X = x;
            rectangle.Y = y;
            rectangle.Width = width;
            rectangle.Height = height;

            return rectangle;
        }

        public static FloatRectangle setRectangle(FloatRectangle rectangle, float x, float y, float width, float height)
        {
            rectangle.x = x;
            rectangle.y = y;
            rectangle.width = width;
            rectangle.height = height;

            return rectangle;
        }

        public static Vector2 setVector(Vector2 vector, int x, int y)
        {
            vector.X = x;
            vector.Y = y;
            return vector;
        }

        public static Vector2 setVector(Vector2 vector, float x, float y)
        {
            vector.X = x;
            vector.Y = y;
            return vector;
        }
    }
}
