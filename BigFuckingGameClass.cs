using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PlatformerGame.GameObjects.CollisionObjects.MovablePhysicsObjects.Actors.StatsActors.Impl;
using PlatformerGame.GameObjects;
using PlatformerGame.Physics;
namespace PlatformerGame
{
	public class BigFuckingGameClass
	{
		private Dictionary<GameObjectType, List<GameObject>> gameObjects;
        public Player player { get; set; }
        private CollisionHandler collisionHandler;
        private static Array gameObjectTypes = Enum.GetValues(typeof(GameObjectType));

		public BigFuckingGameClass ()
		{
            initGameObjectDict();
            collisionHandler = new CollisionHandler(gameObjects);
		}

        public Player getPlayer()
        {
            return player;
        }

        public void initGameObjectDict()
        {
            gameObjects = new Dictionary<GameObjectType, List<GameObject>>();
            foreach (GameObjectType type in Enum.GetValues(typeof(GameObjectType)))
            {
                gameObjects.Add(type, new List<GameObject>());
            }
        }

        public void addPlayerToWorld(Player p)
        {
            gameObjects[GameObjectType.ACTORS].Add(p);
            this.player = p;
        }

        public void addGameObjectToWorld(GameObjectType type, GameObject obj)
        {
            gameObjects[type].Add(obj);
        }

        public List<GameObject> getAllGameObjectsOfType(GameObjectType type)
        {
            return gameObjects[type];
        }

		public void initTextures(Texture2D t)
		{
			player.texture = t;
		}

		public void update(GameTime delta)
		{
			//First, gather inputs and update player accordingly.
			player.processInputs(new InputState());

			//Second, update all other game objects in the game world.
			foreach(List<GameObject> list in gameObjects.Values)
			{
                foreach (IGameObject o in list)
                {
                    o.update(delta);
                }
			}

            //Third, adjust the world's objects due to any collisions
            collisionHandler.handleCollisions();
		}

		public void draw(GameTime t,SpriteBatch batch)
		{
            foreach (List<GameObject> list in gameObjects.Values)
            {
                foreach (GameObject o in list)
                {
                    Drawer.drawObject(t, batch, o);
                }
            }
		}
	}
}

