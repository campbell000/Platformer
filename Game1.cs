﻿#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Storage;
using PlatformerGame.GameObjects.CollisionObjects.MovablePhysicsObjects.Actors.StatsActors.Impl;
using PlatformerGame.GameObjects.CollisionObjects;
using PlatformerGame.GameObjects;

#endregion

namespace PlatformerGame
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Game1 : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		BigFuckingGameClass bigFuckingGameClass;
		private Texture2D _bkg;
		private Vector2 _bkgPos;
        public static SpriteFont font { get; set; }

		public Game1 ()
		{
			graphics = new GraphicsDeviceManager (this);
            Content.RootDirectory = "Content";

            // Change Virtual Resolution 
            Resolution.Init(ref graphics);
            Resolution.SetVirtualResolution(1920, 1080);
            bigFuckingGameClass = new BigFuckingGameClass();
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
        protected override void Initialize()
        {
            Resolution.SetResolution(1920, 1080, false);
            base.Initialize();
        }

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent ()
		{
            //Load Font
            font = Content.Load<SpriteFont>("TestFont");

			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch (GraphicsDevice);

            Texture2D t = Drawer.make2DRect(this.GraphicsDevice, 100, 100, Color.Red);
            Player player = new Player(t, 0, 0, 0, 0, 100, 100);
            bigFuckingGameClass.addPlayerToWorld(player);
            Texture2D wallTexture = Drawer.make2DRect(this.GraphicsDevice, 600, 50, Color.Black);
            CollisionObject wall = new CollisionObject(wallTexture, 0, 0, 900, 900, 600, 50);
            bigFuckingGameClass.addGameObjectToWorld(GameObjectType.WALLS, wall);


			_bkg = Content.Load<Texture2D>("background_large");
			_bkgPos = new Vector2();

		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update (GameTime gameTime)
		{
			// For Mobile devices, this logic will close the Game when the Back button is pressed
			// Exit() is obsolete on iOS
			#if !__IOS__
			if (GamePad.GetState (PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
			    Keyboard.GetState ().IsKeyDown (Keys.Escape)) {
				Exit ();
			}
			#endif

			// TODO: Add your update logic here
			bigFuckingGameClass.update(gameTime);
			base.Update (gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw (GameTime gameTime)
		{
            Resolution.BeginDraw();
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap, DepthStencilState.None, RasterizerState.CullNone, null, Resolution.getTransformationMatrix());
			spriteBatch.Draw(_bkg, _bkgPos, Color.White);
			bigFuckingGameClass.draw(gameTime, spriteBatch);

            Drawer.drawDebug(font, gameTime, spriteBatch, bigFuckingGameClass.getPlayer());
            Console.WriteLine(bigFuckingGameClass.getPlayer().vy);
			spriteBatch.End();

			base.Draw (gameTime);
		}
	}
}

