
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using SharpInvaders.Constants;
using SharpInvaders.Entities;

namespace SharpInvaders
{
    class EnemyBullet : Entity
    {

        public int BulletIndex;
        private EnemyBulletGroup BulletGroup;
        public bool isActive;
        private Enemy enemy;

        public EnemyBullet(ContentManager Content, Enemy enemy, int bulletIndex, EnemyBulletGroup bulletGroup)
        {
            Texture = Content.Load<Texture2D>("enemyBullet");
            this.enemy = enemy;
            Origin = new Vector2(3, 0);
            Velocity = new Vector2(0, Global.PLAYER_BULLINIT_Y);
            isContainedY = false;

            BulletIndex = bulletIndex;
            BulletGroup = bulletGroup;

        }

        public void Fire()
        {
            this.Velocity = new Vector2(0, Global.ENEMY_BULLINIT_Y);
            this.isActive = true;
            this.Position = new Vector2(this.enemy.Position.X, this.enemy.Position.Y - 60);
        }

        public new void Update(GameTime gameTime)
        {
            if (!this.isActive) return;
            if (Position.Y < -Texture.Height * 2) BulletGroup.DequeueBullet(BulletIndex);
            base.Update(gameTime);
        }

    }

}