using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;


using System.Collections.Generic;
using SharpInvaders.Entities;
using TexturePackerLoader;
using System;

namespace SharpInvaders
{
    using SharpInvaders.Constants;
    using SharpInvaders.Entities;
    class EnemySaucerMind
    {

        private ContentManager content;

        public Vector2 InitialPosition;
        public Vector2 Position;
        private float xSpeed;
        private float xSpeedMax;

        private int startY;

        private int xDir;

        public Player playerRef;
        public EnemySaucer saucerRef;
        public EnemyGroup enemyGroupRef;
        public Core gameRef;

        private DateTime nextSpawnCheckTime;
        private Random random;

        public EnemySaucerMind(
            Core game, EnemySaucer saucer, Player player, EnemyGroup enemyGroup)
        {

            this.startY = Global.ENEMYSAUCER_STARTY;
            this.xSpeed = Global.ENEMYSAUCER_SPEEDX;
            this.xSpeedMax = Global.ENEMYSAUCER_SPEEDX_MAX;

            this.xDir = 1;

            this.gameRef = game;
            this.saucerRef = saucer;
            this.playerRef = player;
            this.enemyGroupRef = enemyGroup;

            this.random = new Random();

            this.nextSpawnCheckTime = DateTime.Now.AddSeconds(this.random.Next(15, 25));
        }

        public void KillEnemy(GameTime gameTime)
        {
            this.saucerRef.Die(gameTime);
            this.nextSpawnCheckTime = DateTime.Now.AddSeconds(this.random.Next(8, 12));
        }

        private double getRandomSeconds()
        {
            double s = this.random.Next(15, 20);
            var egca = this.enemyGroupRef.countAlive;
            var maxca = Global.ENEMY_COLS * Global.ENEMY_ROWS;
            // // Increase frequency based on remaining enemies
            if (egca <= maxca / 2) s = this.random.Next(15, 15);

            return s;
        }

        public void Update(GameTime gameTime)
        {

            if (this.saucerRef.isActive)
            {

                var Anim = this.saucerRef.AnimatedSprite;

                // Handle movement out here
                if (this.saucerRef.isHittable)
                {
                    var waveSpeed = (float)(this.gameRef.PlayerWave * 0.5);
                    var saucerSpeed = this.xSpeed + waveSpeed > this.xSpeedMax ? this.xSpeedMax : this.xSpeed + waveSpeed;
                    Anim.Position.X -= saucerSpeed;
                    this.saucerRef.sfxLoop.Pitch = (float)(saucerSpeed * 0.1);
                    if (Anim.Position.X < -256)
                    {
                        this.saucerRef.sfxLoop.Stop();
                        this.saucerRef.isActive = false;
                        this.nextSpawnCheckTime = DateTime.Now.AddSeconds(this.getRandomSeconds());
                    }
                }
                else
                {
                    // Falling from the sky
                    Anim.Position.Y += 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }


                this.saucerRef.Update(gameTime);

            }
            else
            {
                if (DateTime.Now > nextSpawnCheckTime)
                {
                    this.saucerRef.Respawn(gameTime);
                }
            }


        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            this.saucerRef.Draw(gameTime, spriteBatch);
        }

    }

}