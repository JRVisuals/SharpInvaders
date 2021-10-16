
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

        }

        public void AddBullet()
        {


            var b = new PlayerBullet(Content, PlayerRef, Bullets.Count, this);
            b.isContainedX = false;
            b.isContainedY = false;
            b.Velocity.X = (float)(PlayerRef.Velocity.X * 0.25);
            Bullets.Add(b);

            var s = new SmokePuff(Content, this, b);
            Smokes.Add(s);
        }

        public void KillBullet(int index)
        {
            Bullets.RemoveAt(0);
        }

        public void KillSmoke(int index)
        {
            Smokes.RemoveAt(0);
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < Bullets.Count; i++)
            {
                Bullets[i].Update(gameTime);
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
                Bullets[i].Draw(gameTime, spriteBatch);
            }

            for (int i = 0; i < Smokes.Count; i++)
            {
                Smokes[i].Draw(gameTime, spriteBatch);
            }
        }

    }

}