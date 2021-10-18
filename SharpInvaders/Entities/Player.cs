
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using System;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

using SharpInvaders.Constants;
using SharpInvaders.Entities;

namespace SharpInvaders
{
    class Player : Entity
    {

        private ContentManager Content;

        public Player(ContentManager content)
        {
            Content = content;
            Texture = Content.Load<Texture2D>("playerTank");
            Position = new Vector2(Global.GAME_WIDTH / 2, Global.GAME_HEIGHT - 18);
            Origin = new Vector2(32, 64);
            Velocity = new Vector2(0, 0);

        }


        public new void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
        }

        public new void Update(GameTime gameTime, bool isInputControlled)
        {
            if (!isInputControlled) HorizontalFriction((float)gameTime.ElapsedGameTime.TotalSeconds);
            base.Update(gameTime);
        }

        public void HorizontalFriction(float timeMultiplier)
        {
            Velocity.X += Velocity.X >= Global.PLAYER_ACCEL_X ? -Global.PLAYER_ACCEL_X * Global.PLAYER_FRICMULT_X : 0;
            Velocity.X += Velocity.X <= -Global.PLAYER_ACCEL_X ? Global.PLAYER_ACCEL_X * Global.PLAYER_FRICMULT_X : 0;
        }

        public void MoveLeft(float deltaTime)
        {
            if (Velocity.X > -Global.PLAYER_MAXVEL_X) Velocity.X -= Global.PLAYER_ACCEL_X * deltaTime;
        }

        public void MoveRight(float deltaTime)
        {
            if (Velocity.X < Global.PLAYER_MAXVEL_X) Velocity.X += Global.PLAYER_ACCEL_X * deltaTime;
        }

    }

}