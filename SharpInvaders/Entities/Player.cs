
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using TexturePackerLoader;
using SharpInvaders.Constants;
using SharpInvaders.Entities;

namespace SharpInvaders
{
    class Player
    {

        private ContentManager Content;
        private Core coreRef;

        public SpriteBatch spriteBatch;
        public SpriteSheet spriteSheet;

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


        public enum PlayerAnim
        {
            Idle,
            Fire,
            Moving,
            Hit
        }
        public Dictionary<PlayerAnim, Animation[]> Animations { get; set; }
        public AnimatedEntity<PlayerAnim> AnimatedEntity;

        public Player(ContentManager content, Core core, SpriteBatch spriteBatch, SpriteSheet spriteSheet)
        {
            Content = content;
            coreRef = core;

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


            this.spriteSheet = spriteSheet;
            this.spriteBatch = spriteBatch;

            this.Animations = this.AnimationDictionary();

            this.AnimatedEntity = new AnimatedEntity<PlayerAnim>(this.spriteBatch, this.spriteSheet, this.Animations, this.Animations[PlayerAnim.Idle], false, false, false, "player");

            this.AnimatedEntity.Position = new Vector2(Global.GAME_WIDTH / 2, Global.GAME_HEIGHT - Global.PLAYER_OFFSET_Y);
            this.AnimatedEntity.Origin = new Vector2(32, 64);
            this.AnimatedEntity.Velocity = new Vector2(0, 0);
            this.AnimatedEntity.Opacity = 0.75f;

            this.AnimatedEntity.CurrentAnimationSequence = this.Animations[PlayerAnim.Idle];

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
            this.AnimatedEntity.Velocity.X = 0;
            this.AnimatedEntity.Opacity = 0.25f; // doesn't seem to work after updating to animated

            this.AnimatedEntity.CurrentAnimationSequence = this.Animations[PlayerAnim.Hit];

            var s = new PlayerSmokePuff(Content, this, new Vector2(this.AnimatedEntity.Position.X, this.AnimatedEntity.Position.Y - 32));
            Smokes.Add(s);
            Task.Delay(100).ContinueWith((task) =>
            {
                var s1 = new PlayerSmokePuff(Content, this, new Vector2(this.AnimatedEntity.Position.X, this.AnimatedEntity.Position.Y - 16));
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
            this.AnimatedEntity.Opacity = 0.75f;
        }

        public void FireBullet(GameTime gameTime)
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
                    this.AnimatedEntity.CurrentAnimationSequence = this.Animations[PlayerAnim.Fire];
                    this.AnimatedEntity.shouldPlayOnceAndIdle = true;
                    this.AnimatedEntity.previousFrameChangeTime = gameTime.TotalGameTime;
                }
            }
        }

        public void HorizontalFriction(float timeMultiplier)
        {

            this.AnimatedEntity.Velocity.X += this.AnimatedEntity.Velocity.X >= Global.PLAYER_ACCEL_X ? -Global.PLAYER_ACCEL_X * Global.PLAYER_FRICMULT_X : 0;
            this.AnimatedEntity.Velocity.X += this.AnimatedEntity.Velocity.X <= -Global.PLAYER_ACCEL_X ? Global.PLAYER_ACCEL_X * Global.PLAYER_FRICMULT_X : 0;
        }

        public void MoveLeft(float deltaTime)
        {
            if (!isActive) return;
            if (this.AnimatedEntity.Velocity.X > -Global.PLAYER_MAXVEL_X) this.AnimatedEntity.Velocity.X -= Global.PLAYER_ACCEL_X * deltaTime;
        }

        public void MoveRight(float deltaTime)
        {
            if (!isActive) return;
            if (this.AnimatedEntity.Velocity.X < Global.PLAYER_MAXVEL_X) this.AnimatedEntity.Velocity.X += Global.PLAYER_ACCEL_X * deltaTime;
        }

        public void Update(GameTime gameTime, bool isInputControlled)
        {

            if (this.AnimatedEntity.Velocity.X != 0 && this.AnimatedEntity.CurrentAnimationSequence == this.Animations[PlayerAnim.Idle]) this.AnimatedEntity.CurrentAnimationSequence = this.Animations[PlayerAnim.Moving];

            if (isActive && reSpawning)
            {
                if (DateTime.Now > reSpawnSafeTime)
                {
                    reSpawning = false;
                    this.AnimatedEntity.Opacity = 1.0f;
                }
            }

            if (!isInputControlled) HorizontalFriction((float)gameTime.ElapsedGameTime.TotalSeconds);

            for (int i = 0; i < Smokes.Count; i++)
            {
                Smokes[i].Update(gameTime);
            }

            LastBulletFireTime = DateTime.Now;
            playerBulletGroup.Update(gameTime);

            this.AnimatedEntity.Update(gameTime);
        }


        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            this.AnimatedEntity.Draw();
            playerBulletGroup.Draw(gameTime, spriteBatch);

            for (int i = 0; i < Smokes.Count; i++)
            {
                Smokes[i].Draw(gameTime, spriteBatch);
            }

        }

        private Dictionary<PlayerAnim, Animation[]> AnimationDictionary()
        {
            Animation idle = null;
            string[] idleFrames = null;
            Animation moving = null;
            string[] movingFrames = null;
            Animation hit = null;
            string[] hitFrames = null;
            Animation fire = null;
            string[] fireFrames = null;


            idleFrames = new[] {
                        TexturePackerMonoGameDefinitions.tpSprites.Player_idle_0,
                    };
            movingFrames = new[] {
                        TexturePackerMonoGameDefinitions.tpSprites.Player_moving_0,
                        TexturePackerMonoGameDefinitions.tpSprites.Player_moving_1,
                    };
            hitFrames = new[] {
                        TexturePackerMonoGameDefinitions.tpSprites.Player_hit_0,
                        TexturePackerMonoGameDefinitions.tpSprites.Player_hit_1,
                        TexturePackerMonoGameDefinitions.tpSprites.Player_hit_2,
                        TexturePackerMonoGameDefinitions.tpSprites.Player_hit_1,
                        TexturePackerMonoGameDefinitions.tpSprites.Player_hit_0,
                        TexturePackerMonoGameDefinitions.tpSprites.Player_hit_1,
                        TexturePackerMonoGameDefinitions.tpSprites.Player_hit_2,
                        TexturePackerMonoGameDefinitions.tpSprites.Player_hit_1,
                        TexturePackerMonoGameDefinitions.tpSprites.Player_hit_0,
                    };
            fireFrames = new[] {
                        TexturePackerMonoGameDefinitions.tpSprites.Player_fire_0,
                        TexturePackerMonoGameDefinitions.tpSprites.Player_fire_1,
                        TexturePackerMonoGameDefinitions.tpSprites.Player_fire_2,

                    };

            idle = new Animation(timePerFrame: TimeSpan.FromSeconds(1f / 10f), SpriteEffects.None, idleFrames);
            fire = new Animation(timePerFrame: TimeSpan.FromSeconds(1f / 24f), SpriteEffects.None, fireFrames);
            moving = new Animation(timePerFrame: TimeSpan.FromSeconds(1f / 10f), SpriteEffects.None, movingFrames);
            hit = new Animation(timePerFrame: TimeSpan.FromSeconds(1f / 15f), SpriteEffects.None, hitFrames);


            Dictionary<PlayerAnim, Animation[]> AnimationDictionary =
                new Dictionary<PlayerAnim, Animation[]>();

            if (idle != null) AnimationDictionary.Add(PlayerAnim.Idle, new[] { idle });
            if (fire != null) AnimationDictionary.Add(PlayerAnim.Fire, new[] { fire });
            if (moving != null) AnimationDictionary.Add(PlayerAnim.Moving, new[] { moving });
            if (hit != null) AnimationDictionary.Add(PlayerAnim.Hit, new[] { hit });

            return AnimationDictionary;
        }


    }

}