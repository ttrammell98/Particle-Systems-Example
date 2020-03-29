using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace ParticleSystemStarter
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        ParticleSystem particleSystem;
        Texture2D particleTexture;
        Random random = new Random();
        SparkleSystem sparkleSystem;
        Texture2D sparkleTexture;
        FireSystem fireSystem;
        Texture2D fireTexture;
        Texture2D wood;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

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

            // TODO: use this.Content to load your game content here
            particleTexture = Content.Load<Texture2D>("particle");
            particleSystem = new ParticleSystem(this.GraphicsDevice, 1000, particleTexture);
            //particleSystem.Emitter = new Vector2(0, 0);
            particleSystem.SpawnPerFrame = 4;
            particleSystem.SpawnParticle = (ref Particle particle) =>
            {
                MouseState mouse = Mouse.GetState();
                particle.Position = new Vector2(mouse.X, mouse.Y);
                particle.Velocity = new Vector2(
                    MathHelper.Lerp(-50, 50, (float)random.NextDouble()), // X between -50 and 50
                    MathHelper.Lerp(0, 100, (float)random.NextDouble()) // Y between 0 and 100
                    );
                particle.Acceleration = 0.1f * new Vector2(0, (float)-random.NextDouble());
                particle.Color = Color.Green;
                particle.Scale = 1f;
                particle.Life = 1.0f;
            };

            // Set the UpdateParticle method
            particleSystem.UpdateParticle = (float deltaT, ref Particle particle) =>
            {
                particle.Velocity += deltaT * particle.Acceleration;
                particle.Position += deltaT * particle.Velocity;
                particle.Scale -= deltaT;
                particle.Life -= deltaT;
            };


            sparkleTexture = Content.Load<Texture2D>("star");
            sparkleSystem = new SparkleSystem(this.GraphicsDevice, 200, sparkleTexture);
            sparkleSystem.Emitter = new Vector2(75, 75);
            sparkleSystem.SpawnPerFrame = 2;
            sparkleSystem.SpawnSparkle = (ref Particle particle) =>
            {
                particle.Position = new Vector2(200, 200);
                particle.Velocity = new Vector2(
                    MathHelper.Lerp(-100, 100, (float)random.NextDouble()), // X between -50 and 50
                    MathHelper.Lerp(-20, 0, (float)random.NextDouble()) // Y between 0 and 100
                    );
                particle.Acceleration = new Vector2(0, 0);
                particle.Color = Color.Yellow;
                particle.Scale = 0.5f;
                particle.Life = 1f;
            };

            // Set the UpdateParticle method
            sparkleSystem.UpdateSparkle = (float deltaT, ref Particle particle) =>
            {
                particle.Velocity += deltaT * particle.Acceleration;
                particle.Position += deltaT * particle.Velocity;
                particle.Scale -= deltaT;
                particle.Life -= deltaT;
            };



            //FIRE
            fireTexture = Content.Load<Texture2D>("particle");
            fireSystem = new FireSystem(this.GraphicsDevice, 500, fireTexture);
            fireSystem.Emitter = new Vector2(75, 75);
            fireSystem.SpawnPerFrame = 4;
            fireSystem.SpawnFire = (ref Particle particle) =>
            {
                particle.Position = new Vector2(400, 400);
                particle.Velocity = new Vector2(
                    MathHelper.Lerp(-25, 25, (float)random.NextDouble()), // X between -50 and 50
                    MathHelper.Lerp(-95, 0, (float)random.NextDouble()) // Y between 0 and 100
                    );
                particle.Acceleration = 0.1f * new Vector2(0, (float)-random.NextDouble());
                particle.Color = Color.MonoGameOrange;
                particle.Scale = 1.0f;
                particle.Life = 0.5f;
            };

            // Set the UpdateParticle method
            fireSystem.UpdateFire = (float deltaT, ref Particle particle) =>
            {
                particle.Velocity += deltaT * particle.Acceleration;
                particle.Position += deltaT * particle.Velocity;
                particle.Scale -= deltaT;
                particle.Life -= deltaT;
            };

            wood = Content.Load<Texture2D>("wood");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            particleSystem.Update(gameTime);
            sparkleSystem.Update(gameTime);
            fireSystem.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(wood, new Rectangle(395, 400, 50, 50), Color.White);

            spriteBatch.End();
            // TODO: Add your drawing code here
            particleSystem.Draw();
            sparkleSystem.Draw();
            fireSystem.Draw();
            
            base.Draw(gameTime);
        }
    }
}
