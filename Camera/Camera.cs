using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlatformerGame.GameObjects;
using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using PlatformerGame.Utils;

namespace PlatformerGame.Cameras
{
    /**
     * NOTE: The camera's position is at the top-left-most position of it's view, NOT the center.
     **/
    public class Camera
    {
        private GameObjectContainer container;
        private GameObject objectToFollow;
        public float x { get; set; }
        public float y { get; set; }

        private static float WIDTH = Resolution.getInternalWidth();
        private static float HEIGHT = Resolution.getInternalHeight();
        private static float BOUNDARY_WIDTH = (WIDTH / 6) * 2;
        private static float BOUNDARY_HEIGHT = (HEIGHT / 6) * 2;

        public Camera(GameObjectContainer container)
        {
            this.container = container;
        }

        public void centerAndFollowObject(GameObject o)
        {
            objectToFollow = o;
            Vector2 middleOfObject = o.getCenter();
            x = middleOfObject.X - (WIDTH / 2);
            y = middleOfObject.Y - (HEIGHT / 2);
        }

        public void updateCameraPosition()
        {
            float currentLeftCameraBoundary = x + BOUNDARY_WIDTH;
            float currentRightCameraBoundary = x + WIDTH - BOUNDARY_WIDTH;
            float currentTopCameraBoundary = y + BOUNDARY_HEIGHT;
            float currentBottomCameraBoundary = y + HEIGHT - BOUNDARY_HEIGHT;

            //If the object we're following is outside the camera's bounds, move the camera
            if (objectToFollow.x < currentLeftCameraBoundary)
                x -= (currentLeftCameraBoundary - objectToFollow.x);
            else if (objectToFollow.x > currentRightCameraBoundary)
                x += (objectToFollow.x - currentRightCameraBoundary);

            if (objectToFollow.y < currentTopCameraBoundary)
                y -= (currentTopCameraBoundary - objectToFollow.y);
            else if (objectToFollow.y > currentBottomCameraBoundary)
                y += (objectToFollow.y - currentBottomCameraBoundary);
        }
    }
}
