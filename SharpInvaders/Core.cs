﻿using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

using TexturePackerLoader;
using TexturePackerMonoGameDefinitions;

using SharpInvaders.Constants;
using SharpInvaders.Entities;

namespace SharpInvaders
{
    public class Core : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private Player player;
        private Entity ground;
        private Entity logo;
        private BunkerGroup bunkers;
        private SpriteFont spriteFont;
        private FrameCounter frameCounter = new FrameCounter();

        // TODO: Bullet stuff might want to be in a controller
        private PlayerBulletGroup PlayerBullets;
        private DateTime LastTimeCheck;
        private DateTime NextBulletFireTime;
        private SoundEffect sfxFire;
        private SoundEffect sfxDryfire;
        private SoundEffect sfxReload;
        private SoundEffectInstance sfxReloadI;
        private bool didPlayReload;

        // TP Sprite Sheets
        //private SpriteRender tpSpriteRender;
        private SpriteSheet tpSpriteSheet;
        private Enemy testEnemy;

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
            graphics.ApplyChanges();

            player = new Player(Content);
            bunkers = new BunkerGroup(Content);


            LastTimeCheck = DateTime.Now;
            NextBulletFireTime = DateTime.Now;

            sfxFire = Content.Load<SoundEffect>("laser");
            sfxReload = Content.Load<SoundEffect>("reload");
            sfxReloadI = sfxReload.CreateInstance();
            didPlayReload = false;
            sfxDryfire = Content.Load<SoundEffect>("dryfire");

            PlayerBullets = new PlayerBulletGroup(Content, player);


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
            // tpSpriteRender = new SpriteRender(spriteBatch);

            this.testEnemy = new Enemy(spriteBatch, tpSpriteSheet);

        }

        public void FireBullet()
        {

            if (NextBulletFireTime < LastTimeCheck)
            {
                NextBulletFireTime = DateTime.Now.AddSeconds(Global.PLAYER_BULLETDELAY);
                if (PlayerBullets.Bullets.Count < Global.PLAYER_BULLETMAX)
                {
                    PlayerBullets.AddBullet();

                    sfxFire.Play(Global.VOLUME_GLOBAL, 0.0f, 0.0f);
                    didPlayReload = false;

                    // Reload Sound
                    if (PlayerBullets.Bullets.Count == Global.PLAYER_BULLETMAX && !didPlayReload && sfxReloadI.State == SoundState.Stopped)
                    {
                        didPlayReload = true;
                        //   sfxReloadI.Play();

                    }
                }
                else
                {
                    //Dry fire sound
                    sfxDryfire.Play();
                }
            }

        }

        protected void CollisionCheck()
        {

            // Check bullets on bunkers

            foreach (PlayerBullet b in this.PlayerBullets.Bullets)
            {
                var bX = b.Position.X;
                var bY = b.Position.Y;
                var bH = b.Texture.Height;
                var bW = b.Texture.Width;

                foreach (Bunker k in this.bunkers.Bunkers)
                {
                    var kX = k.Position.X;
                    var kY = k.Position.Y;
                    var kW = k.Texture.Width;
                    var kH = k.Texture.Height;

                    // Check for overlap
                    if (bY > kY - kH / 2 && bY < kY + kH / 2 &&
                        bX > kX - kW / 2 && bX < kX + kW / 2)
                    {
                        // Calculate World Space to Texture Space
                        var btX = bX + kW / 2 - kX;
                        var btY = bY + kH / 2 - kY;
                        // Console.WriteLine($"btX: {btX}");
                        // Console.WriteLine($"btY: {btY}");

                        // Check Pixels
                        var bR = new Rectangle(x: (int)btX, y: (int)btY, width: bW, height: bH);
                        if (k.CheckArea(bR))
                        {
                            k.DestroyArea(bR);
                            PlayerBullets.KillBullet(b.BulletIndex);
                            return;
                        }


                    }
                }

            }

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
                this.FireBullet();

            player.Update(gameTime);

            LastTimeCheck = DateTime.Now;
            PlayerBullets.Update(gameTime);

            base.Update(gameTime);

            CollisionCheck();

            testEnemy.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            GraphicsDevice.Clear(new Color(30, 30, 40));

            // Static Stuff
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend);
            ground.Draw(gameTime, spriteBatch);
            logo.Draw(gameTime, spriteBatch);
            spriteBatch.End();


            // Game Stuff
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend);
            bunkers.Draw(gameTime, spriteBatch);
            player.Draw(gameTime, spriteBatch);
            PlayerBullets.Draw(gameTime, spriteBatch);


            // Single Sprite (not animated)
            // tpSpriteRender.Draw(
            //     tpSpriteSheet.Sprite(
            //         tpSprites.EnemyEyes_idle1
            //     ),
            //     new Vector2(350, 530),
            //     Color.White
            // );

            // Animated Sprite
            testEnemy.Draw();


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