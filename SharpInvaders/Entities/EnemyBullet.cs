using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TexturePackerLoader;

using SharpInvaders.Constants;

namespace SharpInvaders.Entities
{
    class EnemyBullet
    {

        public SpriteBatch spriteBatch;
        public SpriteSheet spriteSheet;

        public int BulletIndex;
        private EnemyBulletGroup BulletGroup;
        private BunkerGroup BunkerGroup;
        public bool isActive;
        private Enemy enemy;
        private Player playerRef;

        public enum BulletAnim
        {
            Idle,
        }
        public Dictionary<BulletAnim, Animation[]> Animations { get; set; }
        public AnimatedSprite<BulletAnim> AnimatedSprite;

        public EnemyBullet(ContentManager Content, SpriteBatch spriteBatch, SpriteSheet spriteSheet, Enemy enemy, int bulletIndex, EnemyBulletGroup bulletGroup, BunkerGroup bunkerGroup)
        {

            this.enemy = enemy;



            BulletIndex = bulletIndex;
            BulletGroup = bulletGroup;
            BunkerGroup = bunkerGroup;

            this.playerRef = this.BulletGroup.playerRef;

            this.spriteSheet = spriteSheet;
            this.spriteBatch = spriteBatch;

            this.Animations = this.AnimationDictionary();

            this.AnimatedSprite = new AnimatedSprite<BulletAnim>(
               this.spriteBatch, this.spriteSheet, this.Animations, this.Animations[BulletAnim.Idle], false, false, "bullet");

            this.AnimatedSprite.Origin = new Vector2(3, 0);
            this.AnimatedSprite.Velocity = new Vector2(0, Global.PLAYER_BULLINIT_Y);
            this.AnimatedSprite.isContainedY = false;
            this.AnimatedSprite.isMovable = true;

            this.AnimatedSprite.CurrentAnimationSequence = this.Animations[BulletAnim.Idle];

        }


        public void Fire(Enemy e)
        {
            this.AnimatedSprite.Velocity = new Vector2(0, Global.ENEMY_BULLINIT_Y);
            this.isActive = true;
            // this.Position = new Vector2(e.Position.X, e.Position.Y + 32);
            this.AnimatedSprite.Position = new Vector2(e.Position.X + e.EnemyGroup.Position.X + 16, e.Position.Y + e.EnemyGroup.Position.Y + 16);
        }

        private bool CheckHitPlayer()
        {
            if (this.playerRef.reSpawning || !this.playerRef.isActive) return false;


            if (this.AnimatedSprite.Position.X > this.playerRef.Position.X - 32 && this.AnimatedSprite.Position.X < this.playerRef.Position.X + 32)
            {
                this.BulletGroup.DequeueBullet(this.BulletIndex);
                return true;
            }

            return false;
        }

        private void CheckBunkers()
        {
            // Bunkers
            var bX = this.AnimatedSprite.Position.X;
            var bY = this.AnimatedSprite.Position.Y;
            var bH = 13; //this.AnimatedSprite.CurrentSprite.Texture.Height;
            var bW = 6; //this.AnimatedSprite.CurrentSprite.Texture.Width;

            foreach (Bunker k in this.BunkerGroup.Bunkers)
            {
                var kX = k.Position.X;
                var kY = k.Position.Y;
                var kW = k.Texture.Width;
                var kH = k.Texture.Height;

                // Check for overlap
                if ((bY > kY - (kH) && bY < kY) &&
                    (bX > kX - kW / 2 && bX < kX + kW / 2))
                {

                    // Calculate World Space to Texture Space
                    var btX = bX + (kW / 2) - kX;
                    var btY = bY + (kH / 2) - kY + (bH * 2);

                    // Check Pixels
                    var bR = new Rectangle(x: (int)btX - bW, y: (int)btY, width: bW * 2, height: bH * 2);
                    if (k.CheckArea(bR))
                    {
                        this.BulletGroup.DequeueBullet(this.BulletIndex);
                        return;
                    }
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            if (!this.isActive) return;

            this.AnimatedSprite.Update(gameTime);

            CheckBunkers();

            if (this.AnimatedSprite.Position.Y > Global.GAME_HEIGHT - Global.PLAYER_OFFSET_Y - 48 && this.AnimatedSprite.Position.Y < Global.GAME_HEIGHT - Global.PLAYER_OFFSET_Y)
            {
                if (CheckHitPlayer())
                {
                    this.playerRef.GotHit();

                }
            }

            if (this.AnimatedSprite.Position.Y > Global.GAME_HEIGHT + this.AnimatedSprite.CurrentSprite.Texture.Height * 2) BulletGroup.DequeueBullet(BulletIndex);

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!this.isActive) return;
            var Anim = this.AnimatedSprite;
            Anim.Draw();
        }

        private Dictionary<BulletAnim, Animation[]> AnimationDictionary()
        {
            Animation idle = null;

            string[] idleFrames = null;

            idleFrames = new[] {
                        TexturePackerMonoGameDefinitions.tpSprites.EnemyBullet_idle_0,
                        TexturePackerMonoGameDefinitions.tpSprites.EnemyBullet_idle_1,
                    };

            idle = new Animation(timePerFrame: TimeSpan.FromSeconds(1f / 10f), SpriteEffects.None, idleFrames);

            Dictionary<BulletAnim, Animation[]> AnimationDictionary =
                new Dictionary<BulletAnim, Animation[]>();

            if (idle != null) AnimationDictionary.Add(BulletAnim.Idle, new[] { idle });

            return AnimationDictionary;
        }

    }

}