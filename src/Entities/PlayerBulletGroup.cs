
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using System.Collections.Generic;

namespace SharpInvaders
{
    class PlayerBulletGroup
    {

        // TODO: Peep this: https://www.monogameextended.net/docs/features/object-pooling/object-pooling/

        public List<PlayerBullet> Bullets;
        private ContentManager Content;
        private Player PlayerRef;

        public PlayerBulletGroup(ContentManager contentManager, Player player)
        {

            Content = contentManager;
            Bullets = new List<PlayerBullet>(Constants.PLAYER_BULLETMAX);
            PlayerRef = player;

        }

        public void AddBullet()
        {

            var b = new PlayerBullet(Content, PlayerRef, Bullets.Count, this);
            b.Velocity.X = (float)(PlayerRef.Velocity.X * 0.25);
            Bullets.Add(b);
        }

        public void Kill(int index)
        {
            Bullets.RemoveAt(0);
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < Bullets.Count; i++)
            {
                Bullets[i].Update(gameTime);
            }

        }



        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            for (int i = 0; i < Bullets.Count; i++)
            {
                Bullets[i].Draw(gameTime, spriteBatch);
            }
        }

    }

}