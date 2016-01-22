using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    }
}
