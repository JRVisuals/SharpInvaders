
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

using System;
using System.Threading.Tasks;

using SharpInvaders.Constants;
using SharpInvaders.Entities;

namespace SharpInvaders
{
    class Player : Entity
    {

        private ContentManager Content;

        public PlayerBulletGroup playerBulletGroup;
        private DateTime LastBulletFireTime;
        private DateTime NextBulletFireTime;
        public SoundEffect sfxFire;
        public SoundEffect sfxDryfire;

        public bool isActive;
        public bool reSpawning;
        private DateTime reSpawnSafeTime;

        private Core coreRef;

        public Player(ContentManager content, Core core)
        {
            Content = content;
            coreRef = core;

            Texture = Content.Load<Texture2D>("playerTank");
            Position = new Vector2(Global.GAME_WIDTH / 2, Global.GAME_HEIGHT - Global.PLAYER_OFFSET_Y);
            Origin = new Vector2(32, 64);
            Velocity = new Vector2(0, 0);

            LastBulletFireTime = DateTime.Now;
            NextBulletFireTime = DateTime.Now;
            sfxFire = Content.Load<SoundEffect>("laser2");
            sfxDryfire = Content.Load<SoundEffect>("dryfire");
            playerBulletGroup = new PlayerBulletGroup(Content, this);

            isActive = true;
            reSpawning = true;
            reSpawnSafeTime = DateTime.Now.AddSeconds(Global.PLAYER_RESPAWN_SEC);
            this.Opacity = 0.5f;

        }


        public void GotHit()
        {
            if (!isActive) return;
            Console.WriteLine("YOU DIED.");
            coreRef.PlayerLives -= 1;
            isActive = false;
            Velocity.X = 0;
            this.Opacity = 0.0f;

            if (coreRef.PlayerLives > -1) Task.Delay(1000).ContinueWith((task) => Respawn());
        }

        public void Respawn()
        {
            isActive = true;
            this.reSpawning = true;
            reSpawnSafeTime = DateTime.Now.AddSeconds(Global.PLAYER_RESPAWN_SEC);
            this.Opacity = 0.5f;
        }

        public void FireBullet()
        {

            if (!isActive) return;
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

        public void HorizontalFriction(float timeMultiplier)
        {

            Velocity.X += Velocity.X >= Global.PLAYER_ACCEL_X ? -Global.PLAYER_ACCEL_X * Global.PLAYER_FRICMULT_X : 0;
            Velocity.X += Velocity.X <= -Global.PLAYER_ACCEL_X ? Global.PLAYER_ACCEL_X * Global.PLAYER_FRICMULT_X : 0;
        }

        public void MoveLeft(float deltaTime)
        {
            if (!isActive) return;
            if (Velocity.X > -Global.PLAYER_MAXVEL_X) Velocity.X -= Global.PLAYER_ACCEL_X * deltaTime;
        }

        public void MoveRight(float deltaTime)
        {
            if (!isActive) return;
            if (Velocity.X < Global.PLAYER_MAXVEL_X) Velocity.X += Global.PLAYER_ACCEL_X * deltaTime;
        }


        public new void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
            playerBulletGroup.Draw(gameTime, spriteBatch);
        }

        public void Update(GameTime gameTime, bool isInputControlled)
        {

            if (isActive && reSpawning)
            {
                if (DateTime.Now > reSpawnSafeTime)
                {
                    reSpawning = false;
                    this.Opacity = 1.0f;
                }
            }

            if (!isInputControlled) HorizontalFriction((float)gameTime.ElapsedGameTime.TotalSeconds);

            LastBulletFireTime = DateTime.Now;
            playerBulletGroup.Update(gameTime);

            base.Update(gameTime);
        }


    }

}