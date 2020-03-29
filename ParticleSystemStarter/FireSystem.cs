using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ParticleSystemStarter
{
    /// <summary>
    /// Represts a particle system for the sparkle effect
    /// </summary>
    /// 
    /// <summary>
    /// A delegate for spawning particles
    /// </summary>
    /// <param name="particle">The particle to spawn</param>
    public delegate void FireSpawner(ref Particle particle);

    /// <summary>
    /// A delegate for updating particles
    /// </summary>
    /// <param name="deltaT">The seconds elapsed between frames</param>
    /// <param name="particle">The particle to update</param>
    public delegate void FireUpdater(float deltaT, ref Particle particle);
    public class FireSystem
    {
        /// <summary>
        /// The collection of particles 
        /// </summary>
        Particle[] particles;

        /// <summary>
        /// The texture this particle system uses 
        /// </summary>
        Texture2D texture;

        /// <summary>
        /// The SpriteBatch this particle system uses
        /// </summary>
        SpriteBatch spriteBatch;

        /// <summary>
        /// A random number generator used by the system 
        /// </summary>
        Random random = new Random();

        /// <summary>
        /// The next index in the particles array to use when spawning a particle
        /// </summary>
        int nextIndex = 0;

        /// <summary>
        /// The emitter location for this particle system 
        /// </summary>
        public Vector2 Emitter { get; set; }

        /// <summary>
        /// The rate of particle spawning 
        /// </summary>
        public int SpawnPerFrame { get; set; }

        /// <summary>
        /// Holds a delegate to use when spawning a new particle
        /// </summary>
        public ParticleSpawner SpawnFire { get; set; }

        /// <summary>
        /// Holds a delegate to use when updating a particle 
        /// </summary>
        /// <param name="particle"></param>
        public ParticleUpdater UpdateFire { get; set; }

        /// <summary>
        /// Constructs a new particle engine 
        /// </summary>
        /// <param name="graphicsDevice">The graphics device</param>
        /// <param name="size">The maximum number of particles in the system</param>
        /// <param name="texture">The texture of the particles</param> 
        public FireSystem(GraphicsDevice graphicsDevice, int size, Texture2D texture)
        {
            this.particles = new Particle[size];
            this.spriteBatch = new SpriteBatch(graphicsDevice);
            this.texture = texture;
        }

        public void Update(GameTime gameTime)
        {
            // Make sure our delegate properties are set
            if (SpawnFire == null || UpdateFire == null) return;

            // Part 1: Spawn new particles 
            for (int i = 0; i < SpawnPerFrame; i++)
            {
                // Create the particle
                SpawnFire(ref particles[nextIndex]);

                // Advance the index 
                nextIndex++;
                if (nextIndex > particles.Length - 1) nextIndex = 0;
            }

            // Part 2: Update Particles
            float deltaT = (float)gameTime.ElapsedGameTime.TotalSeconds;
            for (int i = 0; i < particles.Length; i++)
            {
                // Skip any "dead" particles
                if (particles[i].Life <= 0) continue;

                // Update the individual particle
                UpdateFire(deltaT, ref particles[i]);
            }
        }

        public void Draw()
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);

            // TODO: Draw particles
            // Iterate through the particles
            for (int i = 0; i < particles.Length; i++)
            {
                // Skip any "dead" particles
                if (particles[i].Life <= 0) continue;

                // Draw the individual particles
                spriteBatch.Draw(texture, particles[i].Position, null, particles[i].Color, 0f, Vector2.Zero, particles[i].Scale, SpriteEffects.None, 0);
            }
            spriteBatch.End();
        }
    }
}
