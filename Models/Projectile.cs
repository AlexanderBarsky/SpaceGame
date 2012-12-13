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

namespace SpaceGame.Models
{
	/// <summary>
	/// This Projectile class will be a forward moving model that represents a shot fired from a model.  When this model collides
	/// with another model, the ModelManager should destroy those objects.
	/// </summary>
	public class Projectile : BasicModel
	{
		public Projectile(Game inputGame, Model inputModel, Vector3 inputPosition)
			: base(inputGame, inputModel)
		{
			position = inputPosition + 5 * Vector3.Forward;
			world = Matrix.CreateWorld(position, Vector3.Forward, Vector3.Up);
		}

		public override void Update()
		{
			position += Misc.Settings.GAME_SPEED * Misc.Settings.BULLET_SPEED * Vector3.Forward;

			world = Matrix.CreateWorld(position, Vector3.Forward, Vector3.Up);
			
			base.Update();
		}
	}
}
