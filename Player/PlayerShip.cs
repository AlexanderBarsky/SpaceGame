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

namespace SpaceGame.Player
{
	/// <summary>
	/// This PlayerShip class will contain all of the relevant properties and methods to allow the player to control the ship and 
	/// additionally control the camera since the game is designed in the 3rd person.
	/// </summary>
	public class PlayerShip : Models.BasicModel
	{
		public int playerScore { get; set; }

		public PlayerShip(Game inputGame, Model inputModel)
			: base(inputGame, inputModel)
		{
			model = inputModel;
			position = Vector3.Zero;
			health = 100;
			score = 0;
		}

		public override void Update()
		{
			if (Keyboard.GetState().IsKeyDown(Keys.W) || Keyboard.GetState().IsKeyDown(Keys.Up))
			{
				if (position.Y <= (Misc.Settings.TOP_REGION_BOUNDARY - Misc.Settings.BOUNDARY_OFFSET))
				{
					position += Misc.Settings.GAME_SPEED * Misc.Settings.MOVEMENT_RATIO * Vector3.Up;
				}
				else
				{
					position = new Vector3(position.X, Misc.Settings.TOP_REGION_BOUNDARY, position.Z);
				}
			}
			if (Keyboard.GetState().IsKeyDown(Keys.S) || Keyboard.GetState().IsKeyDown(Keys.Down))
			{
				if (position.Y >= (Misc.Settings.BOTTOM_REGION_BOUNDARY + Misc.Settings.BOUNDARY_OFFSET))
				{
					position += Misc.Settings.GAME_SPEED * Misc.Settings.MOVEMENT_RATIO * Vector3.Down;
				}
				else
				{
					position = new Vector3(position.X, Misc.Settings.BOTTOM_REGION_BOUNDARY, position.Z);
				}
			}
			if (Keyboard.GetState().IsKeyDown(Keys.A) || Keyboard.GetState().IsKeyDown(Keys.Left))
			{
				if (position.X >= (Misc.Settings.LEFT_REGION_BOUNDARY + Misc.Settings.BOUNDARY_OFFSET))
				{
					position += Misc.Settings.GAME_SPEED * Misc.Settings.MOVEMENT_RATIO * Vector3.Left;
				}
				else
				{
					position = new Vector3(Misc.Settings.LEFT_REGION_BOUNDARY, position.Y, position.Z);
				}
			}
			if (Keyboard.GetState().IsKeyDown(Keys.D) || Keyboard.GetState().IsKeyDown(Keys.Right))
			{
				if (position.X <= (Misc.Settings.RIGHT_REGION_BOUNDARY - Misc.Settings.BOUNDARY_OFFSET))
				{
					position += Misc.Settings.GAME_SPEED * Misc.Settings.MOVEMENT_RATIO * Vector3.Right;
				}
				else
				{
					position = new Vector3(Misc.Settings.RIGHT_REGION_BOUNDARY, position.Y, position.Z);
				}
			}

			position += Misc.Settings.GAME_SPEED * Vector3.Forward * Misc.Settings.SHIP_SPEED;

			world = Matrix.CreateWorld(position, Vector3.Forward, Vector3.Up);

			base.Update();
		}

		public void DamagePlayer(int inputAmount)
		{
			health -= inputAmount;
		}

		public void Reset(bool alive)
		{
			position = Vector3.Zero;
			world = Matrix.CreateWorld(position, Vector3.Forward, Vector3.Up);
			health = 100;
			if (!alive)
			{
				playerScore = 0;
			}
		}
	}
}