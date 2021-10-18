using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

using TexturePackerLoader;
using TexturePackerMonoGameDefinitions;

using SharpInvaders.Constants;
using SharpInvaders.Entities;
using SharpInvaders.Processes;

namespace SharpInvaders
{
    public class Core : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private SpriteSheet tpSpriteSheet;
        private CoreCollisionDetection CoreCollisionDetection;

        // TODO: Bullet stuff might want to be in a controller
        private PlayerBulletGroup playerBulletGroup;
        private DateTime LastBulletFireTime;
        private DateTime NextBulletFireTime;

        private BunkerGroup bunkerGroup;
        private EnemyGroup enemyGroup;

        private Player player;
        private Entity ground;
        private Entity logo;
        private SpriteFont spriteFont;
        private FrameCounter frameCounter = new FrameCounter();

        public SoundEffect sfxFire;
        public SoundEffect sfxBoom;
        public SoundEffect sfxDryfire;
        public SoundEffect sfxSquish;


        public Core()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                SynchronizeWithVerticalRetrace = false
            };
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            if (Global.USE_FIXED_STEP) IsFixedTimeStep = true;
            if (Global.USE_FIXED_STEP) TargetElapsedTime = TimeSpan.FromMilliseconds(Global.FIXED_STEP_MS);
        }

        protected override void Initialize()
        {

            Window.Title = "SharpInvaders";

            graphics.PreferredBackBufferWidth = Global.GAME_WIDTH;
            graphics.PreferredBackBufferHeight = Global.GAME_HEIGHT;
            //graphics.IsFullScreen = true;
            graphics.ApplyChanges();

            player = new Player(Content);
            bunkerGroup = new BunkerGroup(Content);


            LastBulletFireTime = DateTime.Now;
            NextBulletFireTime = DateTime.Now;

            sfxBoom = Content.Load<SoundEffect>("boom");
            sfxSquish = Content.Load<SoundEffect>("squish");
            sfxFire = Content.Load<SoundEffect>("laser2");
            sfxDryfire = Content.Load<SoundEffect>("dryfire");

            playerBulletGroup = new PlayerBulletGroup(Content, player);

            Console.WriteLine($"gp: {GamePad.GetCapabilities(PlayerIndex.One).IsConnected}");
            Console.WriteLine($"js sup: {Joystick.IsSupported}");
            Console.WriteLine($"js lci: {Joystick.LastConnectedIndex}");
            Console.WriteLine($"js lci: {Joystick.GetCapabilities(Joystick.LastConnectedIndex)}");


            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteFont = Content.Load<SpriteFont>("Arial");
            ground = new Entity();
            ground.Texture = Content.Load<Texture2D>("groundHighres");
            ground.Origin = new Vector2(ground.Texture.Width / 2, ground.Texture.Height);
            ground.Position = new Vector2(Global.GAME_WIDTH / 2, Global.GAME_HEIGHT);

            logo = new Entity();
            logo.Texture = Content.Load<Texture2D>("logo");
            logo.Origin = new Vector2(logo.Texture.Width / 2, 0);
            logo.Position = new Vector2(Global.GAME_WIDTH / 2, 50);

            var spriteSheetLoader = new SpriteSheetLoader(Content, GraphicsDevice);
            tpSpriteSheet = spriteSheetLoader.Load("tpSpriteSheet.png");

            this.enemyGroup = new EnemyGroup(spriteBatch, tpSpriteSheet);

            CoreCollisionDetection = new CoreCollisionDetection(this, this.playerBulletGroup, this.bunkerGroup, this.enemyGroup);

        }

        public void FireBullet()
        {

            if (NextBulletFireTime < LastBulletFireTime)
            {
                NextBulletFireTime = DateTime.Now.AddSeconds(Global.PLAYER_BULLETDELAY);

                var b = playerBulletGroup.EnqueueBullet();
                if (b == null)
                {
                    //Dry fire
                    sfxDryfire.Play();
                }
                else
                {
                    sfxFire.Play(Global.VOLUME_GLOBAL, 0.0f, 0.0f);
                }


            }

        }



        protected override void Update(GameTime gameTime)
        {

            var deltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var keyboardState = Keyboard.GetState();

            var joystickLci = Joystick.LastConnectedIndex;
            var joystickState = Joystick.GetState(joystickLci);
            var isJoystickPresent = joystickLci > -1 && Joystick.GetCapabilities(Joystick.LastConnectedIndex).IsConnected;

            // 8BitDo SN30 Pro Bluetooth
            var jsHatPressLeft = isJoystickPresent ? joystickState.Hats[0].Left == ButtonState.Pressed : false;
            var jsHatPressRight = isJoystickPresent ? joystickState.Hats[0].Right == ButtonState.Pressed : false;
            var jsButtonPressA = isJoystickPresent ? joystickState.Buttons[0] == ButtonState.Pressed : false;

            if (keyboardState.IsKeyDown(Keys.A) || jsHatPressLeft) player.MoveLeft(deltaTime);
            if (keyboardState.IsKeyDown(Keys.D) || jsHatPressRight) player.MoveRight(deltaTime);
            if (keyboardState.IsKeyDown(Keys.I) || jsButtonPressA) this.FireBullet();

            player.Update(gameTime);

            LastBulletFireTime = DateTime.Now;
            playerBulletGroup.Update(gameTime);
            enemyGroup.Update(gameTime);

            base.Update(gameTime);

            CoreCollisionDetection.CollisionCheck(gameTime);

        }

        protected override void Draw(GameTime gameTime)
        {

            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            GraphicsDevice.Clear(new Color(30, 30, 40));

            // Static Stuff
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend);
            ground.Draw(gameTime, spriteBatch);
            logo.Draw(gameTime, spriteBatch);
            //spriteBatch.End();


            // Game Stuff
            // spriteBatch.Begin(samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend);
            bunkerGroup.Draw(gameTime, spriteBatch);
            player.Draw(gameTime, spriteBatch);
            playerBulletGroup.Draw(gameTime, spriteBatch);
            enemyGroup.Draw(gameTime, spriteBatch);

            spriteBatch.End();

            // Debug Stuff
            if (Global.DEBUG)
            {
                frameCounter.Update(deltaTime);
                var fps = string.Format("FPS: {0}", frameCounter.AverageFramesPerSecond);

                spriteBatch.Begin(samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend);
                spriteBatch.DrawString(spriteFont, fps, new Vector2(10, 10), Color.Black);
                spriteBatch.End();

            }

            base.Draw(gameTime);
        }


    }
}


