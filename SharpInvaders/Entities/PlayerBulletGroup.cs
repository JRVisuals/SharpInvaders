
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
    class PlayerBulletGroup
    {

        // TODO: Peep this: https://www.monogameextended.net/docs/features/object-pooling/object-pooling/

        public List<PlayerBullet> Bullets;
        private ContentManager Content;
        private Player PlayerRef;

        public List<SmokePuff> Smokes;


        public PlayerBulletGroup(ContentManager contentManager, Player player)
        {

            Content = contentManager;
            Bullets = new List<PlayerBullet>(Global.PLAYER_BULLETMAX);
            Smokes = new List<SmokePuff>(Global.PLAYER_BULLETMAX);
            PlayerRef = player;

            // create finite pool
            for (int i = 0; i < Global.PLAYER_BULLETMAX; i++)
            {
                var b = new PlayerBullet(Content, PlayerRef, i, this);
                b.isContainedX = false;
                b.isContainedY = false;
                b.Velocity.X = 0;
                b.isActive = false;
                Bullets.Add(b);
            }

        }

        private PlayerBullet BulletFromPool()
        {
            for (int i = 0; i < Global.PLAYER_BULLETMAX; i++)
            {
                var b = Bullets[i];
                if (!b.isActive)
                {
                    return b;
                }
            }
            return null;
        }

        public PlayerBullet EnqueueBullet()
        {

            //var b = new PlayerBullet(Content, PlayerRef, Bullets.Count, this);
            var b = BulletFromPool();
            if (b == null) return null;

            b.Fire();
            var s = new SmokePuff(Content, this, b);
            Smokes.Add(s);

            return b;
        }

        public void DequeueBullet(int index)
        {

            try
            {
                // Inefficient Deque
                var bi = 0;
                foreach (var b in Bullets)
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
                Console.WriteLine($"Tried removing bullet but '{e}'");
            }
        }

        public void KillSmoke(int index)
        {
            Smokes.RemoveAt(0);
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < Global.PLAYER_BULLETMAX; i++)
            {
                if (Bullets[i].isActive) Bullets[i].Update(gameTime);
            }

            for (int i = 0; i < Smokes.Count; i++)
            {
                Smokes[i].Update(gameTime);
            }

        }


        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            for (int i = 0; i < Bullets.Count; i++)
            {
                if (Bullets[i].isActive) Bullets[i].Draw(gameTime, spriteBatch);
            }

            for (int i = 0; i < Smokes.Count; i++)
            {
                Smokes[i].Draw(gameTime, spriteBatch);
            }
        }

    }

}