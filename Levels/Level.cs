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

namespace SpaceGame.Levels
{
	/// <summary>
	/// The Level class contains all the relevant properties in order to handle a typical level.  This should contain all the randomized
	/// boundaries in order to create the levels (this should NOT define an individual level, just contain the methods and a constructor
	/// in order to define a level).  This should also contain properties of the enemies that exist in the game and the constructor can
	/// say how many enemies will be generated throughout play.
	/// </summary>

	public class Level
	{
		public int AsteroidsToSpawn { get; set; }
		public int EnemiesToSpawn { get; set; }

		public Level(int numberOfAsteroids, int numberOfEnemies)
		{
			AsteroidsToSpawn = numberOfAsteroids;
			EnemiesToSpawn = numberOfEnemies;
		}
	}
}
