using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SpaceGame
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class Game1 : Microsoft.Xna.Framework.Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		SpriteFont displayText;
        public GameTime time = new GameTime();
		public GameState gameState { get;  set; }
		public Models.ModelManager modelManager { get; protected set; }
		public Player.PlayerShip playerShip { get; protected set; }
		public Player.Camera camera { get; protected set; }
		public Levels.LevelManager levelManager { get; protected set; }

		private string display { get; set; }
		public int timeToNextLevel { get; set; }

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			graphics.PreferredBackBufferWidth = 1280;
			graphics.PreferredBackBufferHeight = 960;
			Content.RootDirectory = "Content";

			IsFixedTimeStep = true;
			TargetElapsedTime = TimeSpan.FromMilliseconds(20);
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			//Create the camera 50 units behind Vector3.Zero where playerShip spawns.
			camera = new Player.Camera(this, 50.0f * Vector3.Backward, Vector3.Zero, Vector3.Up);
			Components.Add(camera);

			modelManager = new Models.ModelManager(this, graphics.GraphicsDevice);
			Components.Add(modelManager);

			Model playerShipModel = Content.Load<Model>(@"Models\PlayerShipModel");
			playerShip = new Player.PlayerShip(this, playerShipModel);

			gameState = new GameState();
			levelManager = new Levels.LevelManager();

			levelManager.SelectLevel(1);
			modelManager.LoadLevel(levelManager.currentLevel);

			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);

			displayText = Content.Load<SpriteFont>(@"Misc\DebugText");

			// TODO: use this.Content to load your game content here
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			// Allows the game to exit
			if (Keyboard.GetState().IsKeyDown(Keys.Escape))
				this.Exit();

			if (gameState.currentState == State.MainMenu)
			{
				if (Keyboard.GetState().IsKeyDown(Keys.Enter))
				{
					levelManager.SelectLevel(1);
					modelManager.LoadLevel(levelManager.currentLevel);
					gameState.ChangeState("GamePlay");
				}
			}

			if (gameState.currentState == State.GamePlay)
			{
				playerShip.Update();

				if (Keyboard.GetState().IsKeyDown(Keys.Space))
				{
					modelManager.FireShot(playerShip, gameTime);
				}

				//If the player loses all their health, go to game over.
				if (playerShip.health <= 0)
				{
					gameState.ChangeState("GameOver");
				}

				//If the player passes the goal, move to the next level.
				if (playerShip.position.Z < Misc.Settings.Z_REGION_SPAWN_BOUNDARY)
				{
					gameState.ChangeState("ChangeLevel");
				}

				camera.UpdateCamera(gameTime, playerShip);

				base.Update(gameTime);
			}

			if (gameState.currentState == State.ChangeLevel)
			{
				if (timeToNextLevel == 0)
				{
					levelManager.NextLevel();
					if (levelManager.currentSetLevel == -1)
					{
						gameState.ChangeState("Winner");
					}
					else
					{
						timeToNextLevel = Misc.Settings.TIME_BETWEEN_LEVELS;
					}
				}
				else if (timeToNextLevel == 1)
				{
					playerShip.Reset(true);
					camera.Reset();
					modelManager.LoadLevel(levelManager.currentLevel);
					gameState.ChangeState("GamePlay");
					timeToNextLevel = 0;
				}
				else
				{
					timeToNextLevel--;
				}
			}

			if (gameState.currentState == State.GameOver)
			{
				if (Keyboard.GetState().IsKeyDown(Keys.Enter))
				{
					playerShip.Reset(false);
					camera.Reset();
					modelManager.Reset();
					levelManager.Reset();

					levelManager.SelectLevel(1);
					modelManager.LoadLevel(levelManager.currentLevel);
					Misc.Settings.GAME_SPEED = 1.0f;

					gameState.ChangeState("MainMenu");

				}

				camera.UpdateCamera(gameTime, playerShip);
				base.Update(gameTime);
			}

			if (gameState.currentState == State.Winner)
			{
				if (Keyboard.GetState().IsKeyDown(Keys.Enter))
				{
					this.Exit();
				}
			}
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Black);

			if (gameState.currentState != State.GameOver && gameState.currentState != State.MainMenu)
			{
				playerShip.Draw(camera);
			}

			base.Draw(gameTime);

			//Debug Text Rendering.
			spriteBatch.Begin();

			if (gameState.currentState == State.MainMenu)
			{
                
                display = "Space Blaster!\n\n\n\nPress Enter to Begin";
				spriteBatch.DrawString(displayText, display, new Vector2(GraphicsDevice.Viewport.Width / 2 - 200, GraphicsDevice.Viewport.Height / 2), Color.White);
			}

			if (gameState.currentState == State.GamePlay)
			{
				display =	"Health: " + playerShip.health.ToString() + "\n" +
							"Score: " + playerShip.playerScore.ToString();
				spriteBatch.DrawString(displayText, display, new Vector2(10, 10), Color.White);
			}

			if (gameState.currentState == State.ChangeLevel)
			{
				display = "Warping to the next mission in: " + timeToNextLevel;
				spriteBatch.DrawString(displayText, display, new Vector2(GraphicsDevice.Viewport.Width / 2 - 200, GraphicsDevice.Viewport.Height / 2), Color.White);
			}

			if (gameState.currentState == State.GameOver)
			{
				display = "Game Over! Your score is: " + playerShip.playerScore.ToString();
				spriteBatch.DrawString(displayText, display, new Vector2(GraphicsDevice.Viewport.Width / 2 - 200, GraphicsDevice.Viewport.Height / 2), Color.White);
             
                
               // gameState.ChangeState("MainMenu");
			}

			if (gameState.currentState == State.Winner)
			{
				display = "YOU WON! YOU SAVED THE UNIVERSE!";
				spriteBatch.DrawString(displayText, display, new Vector2(GraphicsDevice.Viewport.Width / 2 - 200, GraphicsDevice.Viewport.Height / 2), Color.White);
			}

			spriteBatch.End();
		}
	}
}
