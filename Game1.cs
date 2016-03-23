#region Using Statements
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
using PlatformerGame.GameObjects.CollisionObjects.MovablePhysicsObjects.NonActors.Interactables.Impl;
using PlatformerGame.GameObjects.CollisionObjects.Impl;
using PlatformerGame.GameObjects.CollisionObjects.MovablePhysicsObjects.NonActors.Interactables;
using PlatformerGame.Level;
using PlatformerGame.Utils;
using PlatformerGame.Cameras;
using PlatformerGame.Draw;

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
        private Texture2D interact;
        public static SpriteFontContainer fonts { get; set; }
        private bool drawMinDebug = true;

		public Game1 ()
		{
			graphics = new GraphicsDeviceManager (this);
            Content.RootDirectory = "Content";

            // Change Virtual Resolution 
            Resolution.Init(ref graphics);
            Resolution.SetVirtualResolution(1920, 1080);
            
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
        protected override void Initialize()
        {
            Resolution.SetResolution(1280, 720, false);
            base.Initialize();
        }

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent ()
		{
            //Load Font
            fonts = SpriteFontContainer.initialize(Content);

			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch (GraphicsDevice);
            LevelGenerator generator = new LevelGenerator(Content, GraphicsDevice);
            GameObjectContainer container = generator.generateLevel("resources/levels/test.txt");
            bigFuckingGameClass = new BigFuckingGameClass(container, fonts);
			_bkg = Content.Load<Texture2D>("background_large");
			_bkgPos = new Vector2();

            GC.Collect();
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update (GameTime gameTime)
		{
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad2))
            {
                this.TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 30.0f);
                Console.WriteLine("CALLED");
            }
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad1))
                this.TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 60.0f);

			// For Mobile devices, this logic will close the Game when the Back button is pressed
			// Exit() is obsolete on iOS
			#if !__IOS__
			if (GamePad.GetState (PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
			    Keyboard.GetState ().IsKeyDown (Keys.Escape)) {
				Exit ();
			}
			#endif

            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.D1))
            {
                drawMinDebug = false;
            }
            else if (state.IsKeyDown(Keys.D2))
            {
                drawMinDebug = true;
            }
            else if (state.IsKeyDown(Keys.D3))
            {
                GC.Collect();
            }

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
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap, DepthStencilState.None, RasterizerState.CullNone, null,Resolution.getTransformationMatrix());
			spriteBatch.Draw(_bkg, _bkgPos, Color.White);
			bigFuckingGameClass.draw(gameTime, spriteBatch);

            Drawer.drawDebug(fonts.debugFont, gameTime, spriteBatch, bigFuckingGameClass.getPlayer(), bigFuckingGameClass.getNumGameObjects(), bigFuckingGameClass.playStateManager.numCollisionsChecked, bigFuckingGameClass.getNumObjectsDrawn(), drawMinDebug);
			spriteBatch.End();

			base.Draw (gameTime);
		}
	}
}

