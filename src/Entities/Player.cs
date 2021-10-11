
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using System;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;


namespace SharpInvaders
{
    class Player : Entity
    {

        private ContentManager Content;
        private PlayerBulletGroup PlayerBullets;
        private DateTime LastTimeCheck;
        private DateTime NextBulletFireTime;
        private SoundEffect sfxFire;
        private SoundEffect sfxDryfire;
        private SoundEffect sfxReload;
        private SoundEffectInstance sfxReloadI;
        private bool didPlayReload;

        public Player(ContentManager content)
        {
            Content = content;
            Texture = Content.Load<Texture2D>("playerTank");
            Position = new Vector2(Constants.GAME_WIDTH / 2, Constants.GAME_HEIGHT - 20);
            Origin = new Vector2(32, 64);
            Velocity = new Vector2(0, 0);

            PlayerBullets = new PlayerBulletGroup(Content, this);

            LastTimeCheck = DateTime.Now;
            NextBulletFireTime = DateTime.Now;

            sfxFire = Content.Load<SoundEffect>("laser");
            sfxReload = Content.Load<SoundEffect>("reload");
            sfxReloadI = sfxReload.CreateInstance();
            didPlayReload = false;
            sfxDryfire = Content.Load<SoundEffect>("dryfire");



        }


        public new void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            base.Draw(gameTime, spriteBatch);
            PlayerBullets.Draw(gameTime, spriteBatch);
        }

        public new void Update(GameTime gameTime)
        {
            LastTimeCheck = DateTime.Now;
            HorizontalFriction((float)gameTime.ElapsedGameTime.TotalSeconds);
            base.Update(gameTime);
            PlayerBullets.Update(gameTime);

        }

        public void FireBullet()
        {

            if (NextBulletFireTime < LastTimeCheck)
            {
                NextBulletFireTime = DateTime.Now.AddSeconds(Constants.PLAYER_BULLETDELAY);
                if (PlayerBullets.Bullets.Count < Constants.PLAYER_BULLETMAX)
                {
                    PlayerBullets.AddBullet();

                    sfxFire.Play(0.5f, 0.0f, 0.0f);
                    didPlayReload = false;

                    // Reload Sound
                    if (PlayerBullets.Bullets.Count == Constants.PLAYER_BULLETMAX && !didPlayReload && sfxReloadI.State == SoundState.Stopped)
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

        public void HorizontalFriction(float timeMultiplier)
        {
            var keyboardState = Keyboard.GetState();

            if (!keyboardState.IsKeyDown(Keys.A) && !keyboardState.IsKeyDown(Keys.D))
            {

                Velocity.X += Velocity.X >= Constants.PLAYER_ACCEL_X ? -Constants.PLAYER_ACCEL_X * Constants.PLAYER_FRICMULT_X : 0;
                Velocity.X += Velocity.X <= -Constants.PLAYER_ACCEL_X ? Constants.PLAYER_ACCEL_X * Constants.PLAYER_FRICMULT_X : 0;
            }



        }

        public void MoveLeft(float timeMultiplier)
        {
            if (Velocity.X > -Constants.PLAYER_MAXVEL_X) Velocity.X -= Constants.PLAYER_ACCEL_X * timeMultiplier;
        }

        public void MoveRight(float timeMultiplier)
        {
            if (Velocity.X < Constants.PLAYER_MAXVEL_X) Velocity.X += Constants.PLAYER_ACCEL_X * timeMultiplier;
        }

    }

}