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

namespace SpaceGame.Particles
{
	/// <summary>
	/// This ParticleSettings class should contain the parameters and boundaries for generating particles such as their maximum life
	/// and the texture that they use to create the textures.
	/// </summary>
	public class ParticleSettings
	{
		// Size of particle
		public int maxSize = 1;
	}

	public class ParticleExplosionSettings
	{
		// Life of particles
		public int minLife = 100;
		public int maxLife = 200;

		// Particles per round
		public int minParticlesPerRound = 100;
		public int maxParticlesPerRound = 600;

		// Round time
		public int minRoundTime = 25;
		public int maxRoundTime = 50;

		// Number of particles
		public int minParticles = 2000;
		public int maxParticles = 3000;
	}
}
