
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using SharpInvaders.Constants;

namespace SharpInvaders
{
    class PlayerBullet : Entity
    {


        public int BulletIndex;
        private PlayerBulletGroup BulletGroup;

        public PlayerBullet(ContentManager Content, Player player, int bulletIndex, PlayerBulletGroup bulletGroup)
        {
            Texture = Content.Load<Texture2D>("playerBullet");
            Position = new Vector2(player.Position.X, player.Position.Y - 60);
            Origin = new Vector2(3, 0);
            Velocity = new Vector2(0, Global.PLAYER_BULLINIT_Y);
            isContainedY = false;

            BulletIndex = bulletIndex;
            BulletGroup = bulletGroup;

        }

        public new void Update(GameTime gameTime)
        {


            if (Position.Y < -Texture.Height * 2) BulletGroup.KillBullet(BulletIndex);

            base.Update(gameTime);

        }


    }

}