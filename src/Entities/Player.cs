
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using System;

namespace SharpInvaders
{
    class Player : Entity
    {

        private ContentManager Content;
        private PlayerBulletGroup PlayerBullets;
        private DateTime LastTimeCheck;
        private DateTime NextBulletFireTime;

        public Player(ContentManager content)
        {
            Content = content;
            Texture = Content.Load<Texture2D>("playerTank");
            Position = new Vector2(Constants.GAME_WIDTH / 2, Constants.GAME_HEIGHT - 20);
            Origin = new Vector2(32, 64);
            Velocity = new Vector2(0, 0);

            PlayerBullets = new PlayerBulletGroup(Content, this);

            LastTimeCheck = DateTime.Now;
            NextBulletFireTime = DateTime.Now;

        }


        public new void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            base.Draw(gameTime, spriteBatch);
            PlayerBullets.Draw(gameTime, spriteBatch);
        }

        public new void Update(GameTime gameTime)
        {
            LastTimeCheck = DateTime.Now;
            HorizontalFriction();
            base.Update(gameTime);
            PlayerBullets.Update(gameTime);

        }

        public void FireBullet()
        {

            if (NextBulletFireTime < LastTimeCheck && PlayerBullets.Bullets.Count <= Constants.PLAYER_BULLETMAX)
            {
                PlayerBullets.AddBullet();
                NextBulletFireTime = DateTime.Now.AddSeconds(Constants.PLAYER_BULLETDELAY);
            }

        }

        public void HorizontalFriction()
        {
            Velocity.X += Velocity.X > 0 ? -Constants.PLAYER_ACCEL_X / 2 : 0;

            Velocity.X += Velocity.X < 0 ? Constants.PLAYER_ACCEL_X / 2 : 0;


        }

        public void MoveLeft()
        {
            Velocity.X -= Velocity.X - Constants.PLAYER_ACCEL_X >= -Constants.PLAYER_MAXVEL_X ? Constants.PLAYER_ACCEL_X : 0;
        }

        public void MoveRight()
        {
            Velocity.X += Velocity.X + Constants.PLAYER_ACCEL_X <= Constants.PLAYER_MAXVEL_X ? Constants.PLAYER_ACCEL_X : 0;

        }

    }

}