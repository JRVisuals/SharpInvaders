
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SharpInvaders
{
    class Player : Entity
    {


        public Player(ContentManager Content)
        {
            Texture = Content.Load<Texture2D>("playerTank");
            Position = new Vector2(Constants.GAME_WIDTH / 2, Constants.GAME_HEIGHT - 20);
            Origin = new Vector2(32, 64);
            Velocity = new Vector2(0, 0);

        }

        public Player(Texture2D playerTexture, Vector2 position)
        {
            Texture = playerTexture;
            Position = position;

        }

        public new void Update(GameTime gameTime)
        {
            HorizontalFriction();
            base.Update(gameTime);

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