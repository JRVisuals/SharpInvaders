using System;

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
        private Player playerRef;

        public EnemyBullet(ContentManager Content, Enemy enemy, int bulletIndex, EnemyBulletGroup bulletGroup)
        {
            Texture = Content.Load<Texture2D>("enemyBullet");
            this.enemy = enemy;
            Origin = new Vector2(3, 0);
            Velocity = new Vector2(0, Global.PLAYER_BULLINIT_Y);
            isContainedY = false;

            BulletIndex = bulletIndex;
            BulletGroup = bulletGroup;

            this.playerRef = this.BulletGroup.playerRef;
        }

        public void Fire(Enemy e)
        {
            this.Velocity = new Vector2(0, Global.ENEMY_BULLINIT_Y);
            this.isActive = true;
            // this.Position = new Vector2(e.Position.X, e.Position.Y + 32);
            this.Position = new Vector2(e.Position.X + e.EnemyGroup.Position.X + 16, e.Position.Y + e.EnemyGroup.Position.Y + 16);
        }

        private bool CheckHitPlayer()
        {
            if (this.playerRef.reSpawning || !this.playerRef.isActive) return false;


            if (this.Position.X > this.playerRef.Position.X - 32 && this.Position.X < this.playerRef.Position.X + 32)
            {
                this.BulletGroup.DequeueBullet(this.BulletIndex);
                return true;
            }

            return false;
        }

        public new void Update(GameTime gameTime)
        {
            if (!this.isActive) return;

            if (Position.Y > Global.GAME_HEIGHT - Global.PLAYER_OFFSET_Y - 48 && Position.Y < Global.GAME_HEIGHT - Global.PLAYER_OFFSET_Y)
            {
                if (CheckHitPlayer())
                {
                    this.playerRef.GotHit();

                }
            }

            if (Position.Y > Global.GAME_HEIGHT + Texture.Height * 2) BulletGroup.DequeueBullet(BulletIndex);


            base.Update(gameTime);
        }

    }

}