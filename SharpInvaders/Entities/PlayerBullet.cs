
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using SharpInvaders.Constants;
using SharpInvaders.Entities;

namespace SharpInvaders
{
    class PlayerBullet : Entity
    {


        public int BulletIndex;
        private PlayerBulletGroup BulletGroup;
        public bool isActive;
        private Player Player;

        public PlayerBullet(ContentManager Content, Player player, int bulletIndex, PlayerBulletGroup bulletGroup)
        {
            Texture = Content.Load<Texture2D>("playerBullet");
            Player = player;
            Origin = new Vector2(3, 0);
            Velocity = new Vector2(0, Global.PLAYER_BULLINIT_Y);
            isContainedY = false;

            BulletIndex = bulletIndex;
            BulletGroup = bulletGroup;

        }

        public void Fire()
        {
            this.Velocity = new Vector2((float)(this.Player.Velocity.X * 0.25), Global.PLAYER_BULLINIT_Y);
            this.isActive = true;
            this.Position = new Vector2(this.Player.Position.X, this.Player.Position.Y - 60);
        }

        public new void Update(GameTime gameTime)
        {
            if (!this.isActive) return;
            if (Position.Y < -Texture.Height * 2) BulletGroup.DequeueBullet(BulletIndex);
            base.Update(gameTime);
        }


    }

}