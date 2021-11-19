using Microsoft.Xna.Framework;

using SharpInvaders.Entities;
using SharpInvaders.Constants;

namespace SharpInvaders.Processes
{


    class CoreCollisionDetection
    {

        private Core Game;
        private PlayerBulletGroup playerBulletGroup;
        private BunkerGroup bunkerGroup;
        private EnemyGroup enemyGroup;
        private EnemySaucerMind enemySaucerMind;

        public CoreCollisionDetection(Core game, PlayerBulletGroup playerBulletGroup, BunkerGroup bunkerGroup, EnemyGroup enemyGroup, EnemySaucerMind enemySaucerMind)
        {
            this.Game = game;
            this.playerBulletGroup = playerBulletGroup;
            this.bunkerGroup = bunkerGroup;
            this.enemyGroup = enemyGroup;
            this.enemySaucerMind = enemySaucerMind;
        }
        public void CollisionCheck(GameTime gameTime)
        {

            foreach (PlayerBullet b in this.playerBulletGroup.Bullets)
            {
                if (!b.isActive) continue;

                var bX = b.Position.X;
                var bY = b.Position.Y;
                var bH = b.Texture.Height;
                var bW = b.Texture.Width;

                var bTb = b.Texture.Bounds; // TODO: Are there efficiencies pulling bounding rects and checking intersections, etc over the mathy way I'm doing below


                // Saucer
                var saucerRefLocal = this.enemySaucerMind.saucerRef;
                if (saucerRefLocal.AnimatedSprite.isActive && saucerRefLocal.isHittable)
                {

                    var eW = saucerRefLocal.SpriteWidth;
                    var eH = saucerRefLocal.SpriteHeight;
                    var eX = saucerRefLocal.AnimatedSprite.Position.X + eW / 2;
                    var eY = saucerRefLocal.AnimatedSprite.Position.Y + eH / 2;

                    // Check for overlap
                    if (bY > eY - eH / 2 && bY < eY + eH / 2 &&
                        bX > eX - eW / 2 && bX < eX + eW / 2)
                    {

                        var points = 250;

                        Game.PlayerScore += points;
                        if (Game.PlayerScore > Game.PlayerHighScore) Game.PlayerHighScore = Game.PlayerScore;
                        this.enemySaucerMind.KillEnemy(gameTime);
                        this.playerBulletGroup.DequeueBullet(b.BulletIndex);
                        Game.sfxSquish.Play(Global.VOLUME_GLOBAL, 0.0f, 0.0f);
                        return;
                    }
                }


                // Enemies
                foreach (Enemy e in this.enemyGroup.Enemies)
                {

                    if (!e.AnimatedSprite.isActive || !e.isHittable) continue;

                    var eW = e.SpriteWidth;
                    var eH = e.SpriteHeight;
                    var eX = e.AnimatedSprite.Position.X + eW / 2;
                    var eY = e.AnimatedSprite.Position.Y + eH / 2;

                    // Check for overlap
                    if (bY > eY - eH / 2 && bY < eY + eH / 2 &&
                        bX > eX - eW / 2 && bX < eX + eW / 2)
                    {

                        var points = 0;
                        switch (e.enemyType)
                        {
                            case (Enemy.EnemyType.Green):
                                points = 25;
                                break;
                            case (Enemy.EnemyType.Blue):
                                points = 5;
                                break;
                            case (Enemy.EnemyType.Pink):
                                points = 10;
                                break;
                            default:
                                points = 1;
                                break;
                        }
                        Game.PlayerScore += points;
                        if (Game.PlayerScore > Game.PlayerHighScore) Game.PlayerHighScore = Game.PlayerScore;
                        this.enemyGroup.KillEnemy(e.EnemyIndex, gameTime);
                        this.playerBulletGroup.DequeueBullet(b.BulletIndex);
                        Game.sfxSquish.Play(Global.VOLUME_GLOBAL, 0.0f, 0.0f);
                        return;
                    }

                }

                // Bunkers
                foreach (Bunker k in this.bunkerGroup.Bunkers)
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
                            this.playerBulletGroup.DequeueBullet(b.BulletIndex);
                            Game.sfxBoom.Play(Global.VOLUME_GLOBAL, 0.0f, 0.0f);
                            return;
                        }
                    }
                }
            }
        }

    }
}