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
	/// This is a game component that implements IUpdateable.
	/// </summary>
	public class ModelManager : DrawableGameComponent
	{
		public List<BasicModel> models = new List<BasicModel>();
        public List<BasicModel> AImodels = new List<BasicModel>();
		public List<Projectile> shots = new List<Projectile>();
        //List<BasicEnemy> enemy = new List<BasicEnemy>();
		private Game game;
		private GraphicsDevice graphicsDevice;

		//Bullet variables
		private int shotCooldown = 0;

		//Explosions
		List<Particles.ParticleExplosion> explosions = new List<Particles.ParticleExplosion>();
		Particles.ParticleExplosionSettings particleExplosionSettings = new Particles.ParticleExplosionSettings();
		Particles.ParticleSettings particleSettings = new Particles.ParticleSettings();
		Texture2D explosionTexture;
		Texture2D explosionColorTexture;
		Effect explosionEffect;

		//Stars
		private Particles.ParticleBackground stars;
		Effect starEffect;
		Texture2D starTexture;

		public ModelManager(Game inputGame, GraphicsDevice inputGraphicsDevice)
			: base(inputGame)
		{
			game = inputGame;
			graphicsDevice = inputGraphicsDevice;
		}

		/// <summary>
		/// Allows the game component to perform any initialization it needs to before starting
		/// to run.  This is where it can query for any required services and load content.
		/// </summary>
		public override void Initialize()
		{
			base.Initialize();
           
		}

		protected override void LoadContent()
		{
			explosionTexture = Game.Content.Load<Texture2D>(@"Textures\Particle");
			explosionColorTexture = Game.Content.Load<Texture2D>(@"Textures\ParticleColors");
			explosionEffect = Game.Content.Load<Effect>(@"Misc\Particle");

			explosionEffect.CurrentTechnique = explosionEffect.Techniques["Technique1"];
			explosionEffect.Parameters["theTexture"].SetValue(explosionTexture);

			starTexture = Game.Content.Load<Texture2D>(@"Textures\Stars");
			starEffect = explosionEffect.Clone();
			starEffect.CurrentTechnique = starEffect.Techniques["Technique1"];
			starEffect.Parameters["theTexture"].SetValue(explosionTexture);

			stars = new Particles.ParticleBackground(graphicsDevice, new Vector3(200, 200, Misc.Settings.Z_REGION_SPAWN_BOUNDARY), 3000, starTexture, particleSettings, starEffect);
		}

		/// <summary>
		/// Allows the game component to update itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		public override void Update(GameTime gameTime)
		{

            for (int l = 0; l < AImodels.Count; l++)
            {




                    float XX = AImodels[l].position.X;
                    float YY = AImodels[l].position.Y;
                    float rate = .9f;
                   

                    AImodels[l].position = new Vector3(XX + (((Game1)Game).playerShip.position.X - XX) * rate, YY + (((Game1)Game).playerShip.position.Y - YY) * rate, AImodels[l].position.Z);

                    //models[l].position = new Vector3(0, 0, 0);
                
            }
            
            
            
            
            Random randomValue = new Random();

			for (int i = 0; i < models.Count; i++)
			{
				
                models[i].Update();
				if (models[i].position.Z > 1)
				{
					models.RemoveAt(i);
				}
				else
				{
					if (((Game1)Game).playerShip.CollidesWith(models[i].model, models[i].GetWorld()))
					{
						((Game1)Game).playerShip.DamagePlayer(models[i].damageToPlayer);
						models.RemoveAt(i);
						
						explosions.Add(new Particles.ParticleExplosion(graphicsDevice, models[i].position, randomValue.Next(particleExplosionSettings.minLife, particleExplosionSettings.maxLife), randomValue.Next(particleExplosionSettings.minRoundTime, particleExplosionSettings.maxRoundTime), randomValue.Next(particleExplosionSettings.minParticlesPerRound, particleExplosionSettings.maxParticlesPerRound), randomValue.Next(particleExplosionSettings.minParticles, particleExplosionSettings.maxParticles), explosionColorTexture, particleSettings, explosionEffect));

						i--;

						if (((Game1)Game).playerShip.health <= 0)
						{
							explosions.Add(new Particles.ParticleExplosion(graphicsDevice, ((Game1)Game).playerShip.position, randomValue.Next(3000, 5000), randomValue.Next(particleExplosionSettings.minRoundTime, particleExplosionSettings.maxRoundTime), randomValue.Next(particleExplosionSettings.minParticlesPerRound, particleExplosionSettings.maxParticlesPerRound), randomValue.Next(particleExplosionSettings.minParticles, particleExplosionSettings.maxParticles), explosionColorTexture, particleSettings, explosionEffect));
						}
					}
				}
			}


			for (int i = 0; i < shots.Count; i++)
			{
				shots[i].Update();
				if (shots[i].position.Z < -1.0f * Misc.Settings.CAMERA_DISTANCE)
				{
					shots.RemoveAt(i);
					i--;
				}
				else
				{
					for (int j = 0; j < models.Count; j++)
					{
						if (shots[i].CollidesWith(models[j].model, models[j].GetWorld()))
						{
							models[j].BulletHit();
							if (models[j].health <= 0)
							{
								((Game1)Game).playerShip.playerScore += models[j].score;
								explosions.Add(new Particles.ParticleExplosion(graphicsDevice, shots[i].position, randomValue.Next(particleExplosionSettings.minLife, particleExplosionSettings.maxLife), randomValue.Next(particleExplosionSettings.minRoundTime, particleExplosionSettings.maxRoundTime), randomValue.Next(particleExplosionSettings.minParticlesPerRound, particleExplosionSettings.maxParticlesPerRound), randomValue.Next(particleExplosionSettings.minParticles, particleExplosionSettings.maxParticles), explosionColorTexture, particleSettings, explosionEffect));
								models.RemoveAt(j);
							}
							shots.RemoveAt(i);

							i--;
							break;
						}
					}
				}
			}

           


			for (int i = 0; i < explosions.Count; i++)
			{
				explosions[i].Update(gameTime);
				if (explosions[i].IsDead)
				{
					explosions.RemoveAt(i);
					i--;
				}
			}


            for (int i = 0; i < AImodels.Count; i++)
            {

                AImodels[i].Update();
                if (AImodels[i].position.Z > 1)
                {
                    AImodels.RemoveAt(i);
                }
                else
                {
                    if (((Game1)Game).playerShip.CollidesWith(AImodels[i].model, AImodels[i].GetWorld()))
                    {
                        ((Game1)Game).playerShip.DamagePlayer(AImodels[i].damageToPlayer);
                        AImodels.RemoveAt(i);

                        explosions.Add(new Particles.ParticleExplosion(graphicsDevice, AImodels[i].position, randomValue.Next(particleExplosionSettings.minLife, particleExplosionSettings.maxLife), randomValue.Next(particleExplosionSettings.minRoundTime, particleExplosionSettings.maxRoundTime), randomValue.Next(particleExplosionSettings.minParticlesPerRound, particleExplosionSettings.maxParticlesPerRound), randomValue.Next(particleExplosionSettings.minParticles, particleExplosionSettings.maxParticles), explosionColorTexture, particleSettings, explosionEffect));

                        i--;

                        if (((Game1)Game).playerShip.health <= 0)
                        {
                            explosions.Add(new Particles.ParticleExplosion(graphicsDevice, ((Game1)Game).playerShip.position, randomValue.Next(3000, 5000), randomValue.Next(particleExplosionSettings.minRoundTime, particleExplosionSettings.maxRoundTime), randomValue.Next(particleExplosionSettings.minParticlesPerRound, particleExplosionSettings.maxParticlesPerRound), randomValue.Next(particleExplosionSettings.minParticles, particleExplosionSettings.maxParticles), explosionColorTexture, particleSettings, explosionEffect));
                        }
                    }
                }
            }





            for (int i = 0; i < shots.Count; i++)
            {
                shots[i].Update();
                if (shots[i].position.Z < -1.0f * Misc.Settings.CAMERA_DISTANCE)
                {
                    shots.RemoveAt(i);
                    i--;
                }
                else
                {
                    for (int j = 0; j < AImodels.Count; j++)
                    {
                        if (shots[i].CollidesWith(AImodels[j].model, AImodels[j].GetWorld()))
                        {
                            AImodels[j].BulletHit();
                            if (models[j].health <= 0)
                            {
                                ((Game1)Game).playerShip.playerScore += AImodels[j].score;
                                explosions.Add(new Particles.ParticleExplosion(graphicsDevice, shots[i].position, randomValue.Next(particleExplosionSettings.minLife, particleExplosionSettings.maxLife), randomValue.Next(particleExplosionSettings.minRoundTime, particleExplosionSettings.maxRoundTime), randomValue.Next(particleExplosionSettings.minParticlesPerRound, particleExplosionSettings.maxParticlesPerRound), randomValue.Next(particleExplosionSettings.minParticles, particleExplosionSettings.maxParticles), explosionColorTexture, particleSettings, explosionEffect));
                                AImodels.RemoveAt(j);
                            }
                            shots.RemoveAt(i);

                            i--;
                            break;
                        }
                    }
                }
            }









			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
			foreach (BasicModel model in models)
			{
				model.Draw(((Game1)Game).camera);
			}

			foreach (Projectile shot in shots)
			{
				shot.Draw(((Game1)Game).camera);
			}

			foreach (Particles.ParticleExplosion explosion in explosions)
			{
				explosion.Draw(((Game1)Game).camera);
			}

			stars.Draw(((Game1)Game).camera);

			base.Draw(gameTime);
		}

		public void FireShot(Player.PlayerShip ship, GameTime gameTime)
		{
			if (shotCooldown <= 0)
			{
				Model projectileModel = ((Game1)Game).Content.Load<Model>(@"Models/PlayerProjectile");
				Projectile shot = new Projectile(game, projectileModel, ship.position);
				shots.Add(shot);

				shotCooldown = Misc.Settings.SHOT_DELAY;
			}
			else
			{
				shotCooldown -= gameTime.ElapsedGameTime.Milliseconds;
			}
		}

		public void GenerateAsteroids(int amountToGenerate)
        {
            List<Asteroid> createdAsteroids = new List<Asteroid>();
            Model asteroidModel = ((Game1)Game).Content.Load<Model>(@"Models/AsteroidModel");

            Random randomValue = new Random();

            for (int i = 0; i < amountToGenerate; i++)
            {
                //Creates an asteroid
                Asteroid temp = new Asteroid(game, asteroidModel, new Vector3((float)randomValue.Next((int)Misc.Settings.LEFT_REGION_SPAWN_BOUNDARY, (int)Misc.Settings.RIGHT_REGION_SPAWN_BOUNDARY), (float)randomValue.Next((int)Misc.Settings.BOTTOM_REGION_SPAWN_BOUNDARY, (int)Misc.Settings.TOP_REGION_SPAWN_BOUNDARY), (float)randomValue.Next(Misc.Settings.Z_REGION_SPAWN_BOUNDARY, (int)((Game1)Game).playerShip.position.Z) - 50), ref randomValue, "a");

                //Checks to make sure this asteroid does not conflict with existing asteroids.
                for (int j = 0; j < models.Count; j++)
                {
                    //If it conflicts, generate a new one and rescan the list of models.
                    if (temp.CollidesWith(models[j].model, models[j].GetWorld()))
                    {
                        temp = new Asteroid(game, asteroidModel, new Vector3((float)randomValue.Next((int)Misc.Settings.LEFT_REGION_SPAWN_BOUNDARY, (int)Misc.Settings.RIGHT_REGION_SPAWN_BOUNDARY), (float)randomValue.Next((int)Misc.Settings.BOTTOM_REGION_SPAWN_BOUNDARY, (int)Misc.Settings.TOP_REGION_SPAWN_BOUNDARY), (float)randomValue.Next(Misc.Settings.Z_REGION_SPAWN_BOUNDARY, (int)((Game1)Game).playerShip.position.Z) - 50), ref randomValue, "a");
                        j = 0;
                    }
                }

                //Success!  Add it to the list.
                models.Add((temp));
            }
        }

        public void GenerateEnemy(int amountToGenerate)
        {

            
            Model enemyModel = ((Game1)Game).Content.Load<Model>(@"Models/Enemy");

            Random randomValue = new Random();

            for (int i = 0; i < amountToGenerate; i++)
            {
                //Creates an asteroid
                BasicEnemy temp = new BasicEnemy(game, enemyModel, new Vector3((float)randomValue.Next((int)Misc.Settings.LEFT_REGION_SPAWN_BOUNDARY, (int)Misc.Settings.RIGHT_REGION_SPAWN_BOUNDARY), (float)randomValue.Next((int)Misc.Settings.BOTTOM_REGION_SPAWN_BOUNDARY, (int)Misc.Settings.TOP_REGION_SPAWN_BOUNDARY), (float)randomValue.Next(Misc.Settings.Z_REGION_SPAWN_BOUNDARY, (int)((Game1)Game).playerShip.position.Z) - 50), ref randomValue, "");

                //Checks to make sure this asteroid does not conflict with existing asteroids.
                for (int j = 0; j < models.Count; j++)
                {
                    //If it conflicts, generate a new one and rescan the list of models.
                    if (temp.CollidesWith(models[j].model, models[j].GetWorld()))
                    {
                        temp = new BasicEnemy(game, enemyModel, new Vector3((float)randomValue.Next((int)Misc.Settings.LEFT_REGION_SPAWN_BOUNDARY, (int)Misc.Settings.RIGHT_REGION_SPAWN_BOUNDARY), (float)randomValue.Next((int)Misc.Settings.BOTTOM_REGION_SPAWN_BOUNDARY, (int)Misc.Settings.TOP_REGION_SPAWN_BOUNDARY), (float)randomValue.Next(Misc.Settings.Z_REGION_SPAWN_BOUNDARY, (int)((Game1)Game).playerShip.position.Z) - 50), ref randomValue, "");
                        j = 0;
                    }
                }

                //Success!  Add it to the list.
                models.Add((temp));
                AImodels.Add((temp));
            }
        }

		public void Reset()
		{
			models.RemoveRange(0, models.Count);
			shots.RemoveRange(0, shots.Count);
			explosions.RemoveRange(0, explosions.Count);
		}

		public void LoadLevel(Levels.Level inputLevel)
		{
			GenerateAsteroids(inputLevel.AsteroidsToSpawn);
            GenerateEnemy(inputLevel.EnemiesToSpawn);
		}
	}
}
