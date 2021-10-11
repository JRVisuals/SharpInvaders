using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace SharpInvaders
{
    public class Core : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private Player player;
        private Entity ground;
        private BunkerGroup bunkers;
        private SpriteFont spriteFont;
        private FrameCounter _frameCounter = new FrameCounter();


        public Core()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                SynchronizeWithVerticalRetrace = false
            };
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            // IsFixedTimeStep = true;
            // TargetElapsedTime = TimeSpan.FromMilliseconds(15); // 20 milliseconds, or 50 FPS.
        }

        protected override void Initialize()
        {

            Window.Title = "SharpInvaders";

            graphics.PreferredBackBufferWidth = Constants.GAME_WIDTH;
            graphics.PreferredBackBufferHeight = Constants.GAME_HEIGHT;
            graphics.ApplyChanges();

            player = new Player(Content);
            bunkers = new BunkerGroup(Content);



            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteFont = Content.Load<SpriteFont>("Arial");
            ground = new Entity();
            ground.Texture = Content.Load<Texture2D>("ground");
            ground.Position = new Vector2(0, Constants.GAME_HEIGHT - ground.Texture.Height);

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.A))
                player.MoveLeft((float)gameTime.ElapsedGameTime.TotalMilliseconds);

            if (keyboardState.IsKeyDown(Keys.D))
                player.MoveRight((float)gameTime.ElapsedGameTime.TotalMilliseconds);

            if (keyboardState.IsKeyDown(Keys.I) || keyboardState.IsKeyDown(Keys.W))
                player.FireBullet();

            player.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _frameCounter.Update(deltaTime);

            var fps = string.Format("FPS: {0}", _frameCounter.AverageFramesPerSecond);





            GraphicsDevice.Clear(new Color(106, 106, 106));

            // Static Stuff
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend);
            ground.Draw(gameTime, spriteBatch);
            spriteBatch.End();


            // Game Stuff
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend);
            player.Draw(gameTime, spriteBatch);
            bunkers.Draw(gameTime, spriteBatch);
            spriteBatch.End();

            // Debug Stuff
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend);
            spriteBatch.DrawString(spriteFont, fps, new Vector2(10, 10), Color.Black);
            spriteBatch.End();



            base.Draw(gameTime);
        }
    }
}
