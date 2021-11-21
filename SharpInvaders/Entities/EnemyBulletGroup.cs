using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using TexturePackerLoader;

using SharpInvaders.Constants;
using SharpInvaders.Entities;


namespace SharpInvaders
{
    class EnemyBulletGroup
    {

        // TODO: Peep this: https://www.monogameextended.net/docs/features/object-pooling/object-pooling/

        public List<EnemyBullet> bullets;
        private ContentManager content;
        private Enemy enemyRef;
        public Player playerRef;
        private BunkerGroup BunkerGroup;

        public SpriteBatch spriteBatch;
        public SpriteSheet spriteSheet;

        public EnemyBulletGroup(ContentManager contentManager, SpriteBatch spriteBatch, SpriteSheet spriteSheet, Enemy enemy, Player player, BunkerGroup bunkerGroup)
        {
            this.content = contentManager;
            this.bullets = new List<EnemyBullet>(Global.ENEMY_BULLETMAX);

            this.enemyRef = enemy;
            this.playerRef = player;

            this.spriteSheet = spriteSheet;
            this.spriteBatch = spriteBatch;

            // create finite pool
            for (int i = 0; i < Global.ENEMY_BULLETMAX; i++)
            {
                var b = new EnemyBullet(this.content, this.spriteBatch, this.spriteSheet, this.enemyRef, i, this, bunkerGroup);
                b.AnimatedEntity.isContainedX = false;
                b.AnimatedEntity.isContainedY = false;
                b.AnimatedEntity.Velocity.X = 0;
                b.isActive = false;
                this.bullets.Add(b);
            }
        }

        private EnemyBullet BulletFromPool()
        {
            for (int i = 0; i < Global.ENEMY_BULLETMAX; i++)
            {
                var b = this.bullets[i];
                if (!b.isActive)
                {
                    return b;
                }
            }
            return null;
        }

        public EnemyBullet EnqueueBullet(Enemy e)
        {

            var b = BulletFromPool();
            if (b == null) return null;

            b.Fire(e);

            return b;
        }

        public void DequeueBullet(int index)
        {

            try
            {
                // Inefficient Deque
                var bi = 0;
                foreach (var b in this.bullets)
                {
                    if (!b.isActive) continue;
                    if (b.BulletIndex == index)
                    {
                        b.isActive = false;
                        // Bullets.RemoveAt(bi);
                        return;
                    }
                    bi++;
                }
            }
            catch (System.InvalidOperationException e)
            {
                Console.WriteLine($"Tried removing enemy bullet but '{e}'");
            }
        }


        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < Global.ENEMY_BULLETMAX; i++)
            {
                if (this.bullets[i].isActive) this.bullets[i].Update(gameTime);
            }
        }


        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            for (int i = 0; i < this.bullets.Count; i++)
            {
                if (this.bullets[i].isActive) this.bullets[i].Draw(gameTime, spriteBatch);
            }

        }

    }

}