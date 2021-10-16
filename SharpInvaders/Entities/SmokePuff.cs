
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;

namespace SharpInvaders
{
    class SmokePuff : Entity
    {


        private bool isActive;
        private PlayerBulletGroup BulletGroup;

        private int RotDir;

        public SmokePuff(ContentManager Content, PlayerBulletGroup bulletGroup, PlayerBullet bullet)
        {
            Random r = new Random();
            int randomPuff = r.Next(1, 4);

            Texture = Content.Load<Texture2D>($"smallPuff{randomPuff}");
            Position = new Vector2(bullet.Position.X, bullet.Position.Y - 5);
            Origin = new Vector2(16, 16);
            Opacity = 1f;
            Scale = new Vector2(0.5f);

            int randomAngle = r.Next(0, 360);
            Rotation = MathHelper.ToRadians(randomAngle);
            RotDir = randomAngle > 180 ? 1 : -1;

            float randomSmokeY = (float)(-0.01 * (r.Next(-5, 15)));

            Velocity = new Vector2((float)(bullet.Velocity.X * 0.75), (float)(bullet.Velocity.Y * randomSmokeY));
            isContainedY = false;

            isActive = true;
            BulletGroup = bulletGroup;

        }

        public new void Update(GameTime gameTime)
        {
            if (!isActive) return;
            if (Opacity > 0) { Opacity -= 0.025f; Rotation += 0.01f * RotDir; Scale += new Vector2(0.05f); } else { isActive = false; BulletGroup.KillSmoke(0); }

            base.Update(gameTime);

        }


    }

}