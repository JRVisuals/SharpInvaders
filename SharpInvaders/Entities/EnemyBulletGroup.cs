
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using System.Collections.Generic;
using System;
using System.Diagnostics;

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

        public EnemyBulletGroup(ContentManager contentManager, Enemy enemy, Player player)
        {
            this.content = contentManager;
            this.bullets = new List<EnemyBullet>(Global.ENEMY_BULLETMAX);

            this.enemyRef = enemy;
            this.playerRef = player;

            // create finite pool
            for (int i = 0; i < Global.ENEMY_BULLETMAX; i++)
            {
                var b = new EnemyBullet(this.content, this.enemyRef, i, this);
                b.isContainedX = false;
                b.isContainedY = false;
                b.Velocity.X = 0;
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