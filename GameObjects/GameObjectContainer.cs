using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlatformerGame.Utils;
using PlatformerGame.Level;
using PlatformerGame.GameObjects.CollisionObjects.Impl;
using PlatformerGame.GameObjects.CollisionObjects.MovablePhysicsObjects;
using PlatformerGame.GameObjects.CollisionObjects.MovablePhysicsObjects.NonActors.Interactables;
using PlatformerGame.GameObjects.CollisionObjects.MovablePhysicsObjects.Actors.StatsActors.Impl;
using PlatformerGame.GameObjects.CollisionObjects.MovablePhysicsObjects.NonActors.Interactables.NPCS;
using PlatformerGame.Utils.Pools;
using PlatformerGame.GameObjects.VisualObjects;
using Microsoft.Xna.Framework;

namespace PlatformerGame.GameObjects
{
    public class GameObjectContainer
    {
        /* All tiles that things can collide with (walls, floors, etc) */
        public Tile[][] walls { get; set; }

        /* All interactables (things that do not move, and therefore don't need collisions, but can be interacted with */
        public List<Interactable> interactables { get; set; }

        /* All things that adhere to the laws of physics (well, my flawed approximation of them), 
         * like players, enemies, projectiles, etc.
         */
        public List<MovablePhysicsObject> physicsObjects { get; set; }

        /* All visual effects (interaction indicator, dust, etc) */
        public List<GameObject> visualEffects { get; set; }

        public List<VisualObject> HUDObjects { get; set; }

        /* All objects in the game world */
        public List<GameObject> allObjects { get; set; }

        /** The following variables are objects that need to be especially referenced (due to the shitty way that I designed things) **/
        public Player player {get; set; }

        public Sidekick sidekick {get; set; }

        public VisualObject interactionIndicator;

        public VisualObject dialogBox;

        private Rectangle internalBoundingBox = new Rectangle();

        public GameObjectContainer()
        {
            interactables = new List<Interactable>();
            physicsObjects = new List<MovablePhysicsObject>();
            visualEffects = new List<GameObject>();
            allObjects = new List<GameObject>();
            HUDObjects = new List<VisualObject>();
        }

        public void addPlayer(Player p)
        {
            this.player = p;
            allObjects.Add(p);
            physicsObjects.Add(p);
        }

        public void addNPC(NPC npc)
        {
            this.physicsObjects.Add(npc);
            this.interactables.Add(npc);
            this.allObjects.Add(npc);
        }

        public void addInteractable(Interactable s)
        {
            interactables.Add(s);
            allObjects.Add(s);
        }

        public void addPhysicsObject(MovablePhysicsObject m)
        {
            physicsObjects.Add(m);
            allObjects.Add(m);
        }

        public void removePhysicsObject(MovablePhysicsObject m)
        {
            physicsObjects.Remove(m);
            allObjects.Remove(m);
        }

        public void addHUDObject(VisualObject o)
        {
            HUDObjects.Add(o);
        }

        public void removeInteractable(Interactable s)
        {
            interactables.Remove(s);
            allObjects.Remove(s);
        }

        public void addVisualEffect(GameObject o)
        {
            visualEffects.Add(o);
            allObjects.Add(o);
        }

        public void removeVisualEffect(GameObject o)
        {
            visualEffects.Remove(o);
            allObjects.Remove(o);
        }

        public List<GameObject> getAllGameObjects()
        {
            return allObjects;
        }

        public Player getPlayer()
        {
            return player;
        }

        /**
         * This method returns a rectangle representing the dimension of all of the tiles that can possibly 
         * collide with the given object. For example, if the given object can collide with the tiles at 
         * (3,4), (3, 5), (4, 4), and (4, 5), this method will return a Rectangle where x = 3, y = 4,
         * width = 2, and height = 2. This culls the amount of objects we need to check for collisions.
         **/
        public Rectangle getBoundingBoxForPossibleCollisions(GameObject reference)
        {
            //Get the indices of tiles that we need to check for the collision. We don't want to check EVERY tile.
            //We need to be as inclusive as possible, so we need to add/subtract one from the indices.
            FloatRectangle bBox = reference.getBoundingBox();
            int topTileIndex = (int)Math.Round((bBox.Top / LevelGenConst.TILE_HEIGHT), 0) - 1;
            int bottomTileIndex = (int)Math.Round((bBox.Bottom / LevelGenConst.TILE_HEIGHT), 0) + 1;
            int leftTileIndex = (int)Math.Round((bBox.Left / LevelGenConst.TILE_WIDTH), 0) - 1;
            int rightTileIndex = (int)Math.Round((bBox.Right / LevelGenConst.TILE_WIDTH), 0) + 1;

            int width = rightTileIndex - leftTileIndex;
            int height = bottomTileIndex - topTileIndex;

            return getBoundingCollideBox(leftTileIndex, topTileIndex, width, height);
        }

        public Tile getTileAt(int x, int y)
        {
            if (x > -1 && x < walls.Length)
            {
                if (y > -1 && y < walls[x].Length)
                {
                    return walls[x][y];
                }
            }

            return null;
        }

        private Rectangle getBoundingCollideBox(int x, int y, int width, int height)
        {
            internalBoundingBox.X = x;
            internalBoundingBox.Y = y;
            internalBoundingBox.Width = width;
            internalBoundingBox.Height = height;

            return internalBoundingBox;
        }

        public void setTiles(Tile [][] value)
        {
            this.walls = value;
            foreach (Tile[] arr in value)
            {
                foreach (Tile tile in arr)
                {
                    if (tile != null)
                        allObjects.Add(tile);
                }
            }
        }
    }
 
}
