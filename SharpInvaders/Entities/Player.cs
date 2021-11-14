
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

using SharpInvaders.Constants;
using SharpInvaders.Entities;

namespace SharpInvaders
{
    class Player : Entity
    {

        private ContentManager Content;
        private Core coreRef;

        public PlayerBulletGroup playerBulletGroup;
        private DateTime LastBulletFireTime;
        private DateTime NextBulletFireTime;
        public SoundEffect sfxFire;
        public SoundEffect sfxDryfire;
        public SoundEffect sfxDeath;

        public List<PlayerSmokePuff> Smokes;
        public bool isActive;
        public bool reSpawning;
        private DateTime reSpawnSafeTime;


        public Player(ContentManager content, Core core)
        {
            Content = content;
            coreRef = core;

            Texture = Content.Load<Texture2D>("playerTank");
            Position = new Vector2(Global.GAME_WIDTH / 2, Global.GAME_HEIGHT - Global.PLAYER_OFFSET_Y);
            Origin = new Vector2(32, 64);
            Velocity = new Vector2(0, 0);

            Smokes = new List<PlayerSmokePuff>(Global.PLAYER_BULLETMAX);

            LastBulletFireTime = DateTime.Now;
            NextBulletFireTime = DateTime.Now;
            sfxFire = Content.Load<SoundEffect>("laser2");
            sfxDryfire = Content.Load<SoundEffect>("dryfire");
            sfxDeath = Content.Load<SoundEffect>("playerdeath");
            playerBulletGroup = new PlayerBulletGroup(Content, this);

            isActive = true;
            reSpawning = true;
            reSpawnSafeTime = DateTime.Now.AddSeconds(Global.PLAYER_RESPAWN_SEC);
            this.Opacity = 0.75f;

        }

        public void KillSmoke(int index)
        {
            Smokes.RemoveAt(0);
        }


        public void GotHit()
        {
            if (!isActive) return;
            sfxDeath.Play();
            coreRef.PlayerLives -= 1;
            isActive = false;
            Velocity.X = 0;
            this.Opacity = 0.25f;

            var s = new PlayerSmokePuff(Content, this, new Vector2(this.Position.X, this.Position.Y - 32));
            Smokes.Add(s);
            Task.Delay(100).ContinueWith((task) =>
            {
                var s1 = new PlayerSmokePuff(Content, this, new Vector2(this.Position.X, this.Position.Y - 16));
                Smokes.Add(s1);
            });

            if (coreRef.PlayerLives > -1) { Task.Delay(Global.PLAYER_DEAD_MS).ContinueWith((task) => Respawn()); }
            else
            {
                Task.Delay(Global.PLAYER_DEAD_MS).ContinueWith((task) => coreRef.GameOver());

            }

        }

        public void Respawn()
        {
            isActive = true;
            this.reSpawning = true;
            reSpawnSafeTime = DateTime.Now.AddSeconds(Global.PLAYER_RESPAWN_SEC);
            this.Opacity = 0.75f;
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

            for (int i = 0; i < Smokes.Count; i++)
            {
                Smokes[i].Update(gameTime);
            }

            LastBulletFireTime = DateTime.Now;
            playerBulletGroup.Update(gameTime);

            base.Update(gameTime);
        }


        public new void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
            playerBulletGroup.Draw(gameTime, spriteBatch);

            for (int i = 0; i < Smokes.Count; i++)
            {
                Smokes[i].Draw(gameTime, spriteBatch);
            }

        }


    }

}