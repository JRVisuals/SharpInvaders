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

    public enum GameState
    {
        MainMenu,
        InGame,
        GameOver
    }
    public class Core : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private SpriteSheet tpSpriteSheet;
        private CoreCollisionDetection CoreCollisionDetection;

        private GameState gameState;
        private BunkerGroup bunkerGroup;
        private EnemyGroup enemyGroup;
        private EnemySaucerMind enemySaucerMind;
        private EnemySaucer enemySaucer;

        private Player player;
        private Entity ground;
        private Entity logo;
        private SpriteFont spriteFontArial;
        private SpriteFont spriteFontAtari;
        private FrameCounter frameCounter = new FrameCounter();

        public SoundEffect sfxBoom;
        public SoundEffect sfxSquish;

        public int PlayerHighScore;
        public int PlayerScore;
        public int PlayerLives;

        private Boolean isResetting;


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

            gameState = GameState.MainMenu;

            graphics.PreferredBackBufferWidth = Global.GAME_WIDTH;
            graphics.PreferredBackBufferHeight = Global.GAME_HEIGHT;
            //graphics.IsFullScreen = true;
            graphics.ApplyChanges();

            sfxBoom = Content.Load<SoundEffect>("boom");
            sfxSquish = Content.Load<SoundEffect>("squish");

            // Temp Joystick debug
            // Console.WriteLine($"gp: {GamePad.GetCapabilities(PlayerIndex.One).IsConnected}");
            // Console.WriteLine($"js sup: {Joystick.IsSupported}");
            // Console.WriteLine($"js lci: {Joystick.LastConnectedIndex}");
            // Console.WriteLine($"js lci: {Joystick.GetCapabilities(Joystick.LastConnectedIndex)}");

            var joystickLci = Joystick.LastConnectedIndex;
            var joystickState = Joystick.GetState(joystickLci);
            var isJoystickPresent = joystickLci > -1 && Joystick.GetCapabilities(Joystick.LastConnectedIndex).IsConnected;

            this.player = new Player(Content, this);
            this.bunkerGroup = new BunkerGroup(Content);

            this.PlayerHighScore = 0;
            this.PlayerScore = 0;
            this.PlayerLives = Global.PLAYER_START_LIVES - 1;

            this.isResetting = false;

            base.Initialize();

        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteFontArial = Content.Load<SpriteFont>("Arial");
            spriteFontAtari = Content.Load<SpriteFont>("Atari");
            ground = new Entity();
            ground.Texture = Content.Load<Texture2D>("groundHighres");
            ground.Origin = new Vector2(ground.Texture.Width / 2, ground.Texture.Height);
            ground.Position = new Vector2(Global.GAME_WIDTH / 2, Global.GAME_HEIGHT);

            logo = new Entity();
            logo.Texture = Content.Load<Texture2D>("logo");
            logo.Origin = new Vector2(logo.Texture.Width / 2, 0);
            logo.Position = new Vector2(Global.GAME_WIDTH / 2, 50);

            var spriteSheetLoader = new SpriteSheetLoader(Content, GraphicsDevice);
            tpSpriteSheet = spriteSheetLoader.Load("tpSpriteSheet", Content);

            this.enemyGroup = new EnemyGroup(Content, spriteBatch, tpSpriteSheet, this.player, this.bunkerGroup);
            this.enemySaucer = new EnemySaucer(Content, spriteBatch, tpSpriteSheet, new Vector2(x: Global.GAME_WIDTH + 10, y: Global.ENEMYSAUCER_STARTY), this.player);
            this.enemySaucerMind = new EnemySaucerMind(this.enemySaucer, this.player, this.enemyGroup);


            CoreCollisionDetection = new CoreCollisionDetection(this, this.player.playerBulletGroup, this.bunkerGroup, this.enemyGroup, this.enemySaucerMind);

        }

        public void GameOver()
        {
            Console.WriteLine("GAME OVER");
            this.gameState = GameState.GameOver;
            this.PlayerLives = -1;

        }

        public void GameStart()
        {

            Console.WriteLine("GAME START");
            this.bunkerGroup.Respawn();
            this.PlayerScore = 0;
            this.PlayerLives = Global.PLAYER_START_LIVES - 1;

            this.isResetting = true;
            this.gameState = GameState.InGame;

        }

        protected override void Update(GameTime gameTime)
        {

            if (this.isResetting)
            {
                Console.WriteLine("RESETTING");
                this.isResetting = false;
                this.player.Respawn();
                this.enemyGroup.ReSpawn(gameTime);
            }

            var deltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            var keyboardState = Keyboard.GetState();
            var kbPressLeft = keyboardState.IsKeyDown(Keys.A);
            var kbPressRight = keyboardState.IsKeyDown(Keys.D);
            var kbPressFire = keyboardState.IsKeyDown(Keys.I);
            var kbPressFireAlt = keyboardState.IsKeyDown(Keys.W);

            var joystickLci = Joystick.LastConnectedIndex;
            var joystickState = Joystick.GetState(joystickLci);
            var isJoystickPresent = joystickLci > -1 && Joystick.GetCapabilities(Joystick.LastConnectedIndex).IsConnected;

            // 8BitDo SN30 Pro Bluetooth
            var jsHatPressLeft = isJoystickPresent ? joystickState.Hats[0].Left == ButtonState.Pressed : false;
            var jsHatPressRight = isJoystickPresent ? joystickState.Hats[0].Right == ButtonState.Pressed : false;
            var jsButtonPressA = isJoystickPresent ? joystickState.Buttons[0] == ButtonState.Pressed : false;

            //Console.WriteLine($"jsHatPressLeft: {jsHatPressLeft}  jsHatPressRight: {jsHatPressRight}");

            switch (gameState)
            {

                case GameState.MainMenu: //------------------------ MAIN MENU UPDATE

                    enemyGroup.UpdateMenu(gameTime);
                    if (kbPressFire || kbPressFireAlt || jsButtonPressA) this.GameStart();

                    break;


                case GameState.InGame: //------------------------ IN GAME UPDATE

                    var isInputControlled = false;

                    if (kbPressLeft || jsHatPressLeft) { this.player.MoveLeft(deltaTime); isInputControlled = true; }
                    if (kbPressRight || jsHatPressRight) { this.player.MoveRight(deltaTime); isInputControlled = true; }
                    if (kbPressFire || kbPressFireAlt || jsButtonPressA) this.player.FireBullet();

                    player.Update(gameTime, isInputControlled);
                    enemyGroup.Update(gameTime);
                    enemySaucerMind.Update(gameTime);
                    base.Update(gameTime);
                    CoreCollisionDetection.CollisionCheck(gameTime);

                    break;


                case GameState.GameOver:  //------------------------ GAME OVER UPDATE

                    enemyGroup.UpdateMenu(gameTime);
                    if (kbPressFire || kbPressFireAlt || jsButtonPressA) this.GameStart();

                    break;
            }


        }

        protected override void Draw(GameTime gameTime)
        {

            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            GraphicsDevice.Clear(new Color(30, 30, 40));

            // Static Stuff
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend);
            ground.Draw(gameTime, spriteBatch
            );


            switch (gameState)
            {

                case GameState.MainMenu: //------------------------ MAIN MENU DRAW


                    bunkerGroup.Draw(gameTime, spriteBatch);
                    player.Draw(gameTime, spriteBatch);
                    enemyGroup.DrawMenu(gameTime, spriteBatch);

                    spriteBatch.DrawString(spriteFontAtari, $"WELCOME TO SHARPINVADERS", new Vector2(Global.GAME_WIDTH / 2 - spriteFontAtari.MeasureString("WELCOME TO SHARPINVADERS").X / 2, 300), Color.WhiteSmoke);
                    spriteBatch.DrawString(spriteFontAtari, $"PRESS FIRE TO BEGIN", new Vector2(Global.GAME_WIDTH / 2 - spriteFontAtari.MeasureString("PRESS FIRE TO BEGIN").X / 2, 325), Color.LimeGreen);

                    logo.Draw(gameTime, spriteBatch);
                    break;


                case GameState.InGame: //------------------------ IN GAME DRAW

                    // Game Stuff
                    bunkerGroup.Draw(gameTime, spriteBatch);
                    player.Draw(gameTime, spriteBatch);
                    enemyGroup.Draw(gameTime, spriteBatch);
                    enemySaucerMind.Draw(gameTime, spriteBatch);
                    break;


                case GameState.GameOver:  //------------------------ GAME OVER DRAW


                    bunkerGroup.Draw(gameTime, spriteBatch);
                    player.Draw(gameTime, spriteBatch);
                    enemyGroup.DrawMenu(gameTime, spriteBatch);

                    spriteBatch.DrawString(spriteFontAtari, $"GAME OVER", new Vector2(Global.GAME_WIDTH / 2 - spriteFontAtari.MeasureString("GAME OVER").X / 2, 300), Color.WhiteSmoke);
                    spriteBatch.DrawString(spriteFontAtari, $"PRESS FIRE TO PLAY AGAIN", new Vector2(Global.GAME_WIDTH / 2 - spriteFontAtari.MeasureString("PRESS FIRE TO PLAY AGAIN").X / 2, 325), Color.LimeGreen);

                    logo.Draw(gameTime, spriteBatch);
                    break;
            }


            spriteBatch.DrawString(spriteFontAtari, $"SCORE", new Vector2(10, 10), Color.Gray);
            spriteBatch.DrawString(spriteFontAtari, $"{PlayerScore}", new Vector2(10, 35), Color.White);

            spriteBatch.DrawString(spriteFontAtari, $"HIGHSCORE", new Vector2(Global.GAME_WIDTH / 2 - spriteFontAtari.MeasureString("HIGHSCORE").X / 2, 10), Color.Gray);
            spriteBatch.DrawString(spriteFontAtari, $"{PlayerHighScore}", new Vector2(Global.GAME_WIDTH / 2 - spriteFontAtari.MeasureString($"{PlayerHighScore}").X / 2, 35), Color.White);

            spriteBatch.DrawString(spriteFontAtari, $"LIVES", new Vector2(Global.GAME_WIDTH - 115, 10), Color.Gray);
            spriteBatch.DrawString(spriteFontAtari, $"{PlayerLives + 1}", new Vector2(Global.GAME_WIDTH - 33, 35), Color.White);



            spriteBatch.End();

            // Debug Stuff
            if (Global.DEBUG)
            {
                frameCounter.Update(deltaTime);
                var fps = string.Format("FPS: {0}", frameCounter.AverageFramesPerSecond);

                spriteBatch.Begin(samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend);
                spriteBatch.DrawString(spriteFontArial, fps, new Vector2(300, 10), Color.Black);
                spriteBatch.End();

            }

            base.Draw(gameTime);



        }


    }
}


