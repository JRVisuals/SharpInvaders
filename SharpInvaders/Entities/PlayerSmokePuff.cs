
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;

using SharpInvaders.Entities;
namespace SharpInvaders
{
    class PlayerSmokePuff : Entity
    {


        private bool isActive;
        private Player Container;

        private int RotDir;

        public PlayerSmokePuff(ContentManager Content, Player container, Vector2 position)
        {
            Random r = new Random();
            int randomPuff = r.Next(1, 4);

            Texture = Content.Load<Texture2D>($"smallPuff{randomPuff}");
            Position = position;
            Origin = new Vector2(16, 16);
            Opacity = 1f;
            Scale = new Vector2(0.5f);

            int randomAngle = r.Next(0, 360);
            Rotation = MathHelper.ToRadians(randomAngle);
            RotDir = randomAngle > 180 ? 1 : -1;

            float randomSmokeY = (float)(-0.01 * (r.Next(-5, 15)));

            isContainedY = false;

            isActive = true;
            Container = container;

        }

        public new void Update(GameTime gameTime)
        {
            if (!isActive) return;
            if (Opacity > 0) { Opacity -= 0.025f; Rotation += 0.01f * RotDir; Scale += new Vector2(0.05f); } else { isActive = false; Container.KillSmoke(0); }

            base.Update(gameTime);

        }


    }

}