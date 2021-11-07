
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using System;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

using SharpInvaders.Constants;
using SharpInvaders.Entities;

namespace SharpInvaders
{
    class Player : Entity
    {

        private ContentManager Content;

        // TODO: Bullet stuff might want to be in a controller
        public PlayerBulletGroup playerBulletGroup;
        private DateTime LastBulletFireTime;
        private DateTime NextBulletFireTime;
        public SoundEffect sfxFire;
        public SoundEffect sfxDryfire;

        public Player(ContentManager content)
        {
            Content = content;
            Texture = Content.Load<Texture2D>("playerTank");
            Position = new Vector2(Global.GAME_WIDTH / 2, Global.GAME_HEIGHT - 18);
            Origin = new Vector2(32, 64);
            Velocity = new Vector2(0, 0);

            LastBulletFireTime = DateTime.Now;
            NextBulletFireTime = DateTime.Now;
            sfxFire = Content.Load<SoundEffect>("laser2");
            sfxDryfire = Content.Load<SoundEffect>("dryfire");
            playerBulletGroup = new PlayerBulletGroup(Content, this);

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


        public new void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
            playerBulletGroup.Draw(gameTime, spriteBatch);
        }

        public void Update(GameTime gameTime, bool isInputControlled)
        {
            if (!isInputControlled) HorizontalFriction((float)gameTime.ElapsedGameTime.TotalSeconds);

            LastBulletFireTime = DateTime.Now;
            playerBulletGroup.Update(gameTime);

            base.Update(gameTime);
        }

        public void HorizontalFriction(float timeMultiplier)
        {
            Velocity.X += Velocity.X >= Global.PLAYER_ACCEL_X ? -Global.PLAYER_ACCEL_X * Global.PLAYER_FRICMULT_X : 0;
            Velocity.X += Velocity.X <= -Global.PLAYER_ACCEL_X ? Global.PLAYER_ACCEL_X * Global.PLAYER_FRICMULT_X : 0;
        }

        public void MoveLeft(float deltaTime)
        {
            if (Velocity.X > -Global.PLAYER_MAXVEL_X) Velocity.X -= Global.PLAYER_ACCEL_X * deltaTime;
        }

        public void MoveRight(float deltaTime)
        {
            if (Velocity.X < Global.PLAYER_MAXVEL_X) Velocity.X += Global.PLAYER_ACCEL_X * deltaTime;
        }

    }

}