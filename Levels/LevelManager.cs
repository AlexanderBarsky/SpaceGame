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
	/// The LevelManager class should contain all the predefined Level objects and their relevant values.  In here you should 
	/// define how many levels the game contains, how to transitition between levels, and define each individual level using 
	/// the Level class' constructor.  Add them all to a List<Level> so that you can iterate between levels easily.
	/// </summary>
	public class LevelManager
	{
		private List<Level> levels = new List<Level>();
		public int currentSetLevel;
		public Level currentLevel;

		public LevelManager()
		{
			levels.Add(new Level(50, 15));
			levels.Add(new Level(75, 20));
			levels.Add(new Level(100, 25));
			levels.Add(new Level(125, 30));
			levels.Add(new Level(150, 35));
			levels.Add(new Level(175, 40));

			currentSetLevel = 0;
			currentLevel = levels[currentSetLevel];
		}

		public void SelectLevel(int levelNumber)
		{
			currentSetLevel = levelNumber - 1;
			currentLevel = levels[currentSetLevel];
		}

		public void NextLevel()
		{
			currentSetLevel++;
			if (currentSetLevel >= levels.Count)
			{
				currentSetLevel = -1;
			}
			else
			{
				Misc.Settings.GAME_SPEED += Misc.Settings.SPEED_INCREASE_BETWEEN_LEVELS;
				currentLevel = levels[currentSetLevel];
			}
		}

		internal void Reset()
		{
			currentSetLevel = 0;
		}
	}
}
