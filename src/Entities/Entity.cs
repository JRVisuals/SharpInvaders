
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SharpInvaders
{

    public interface IEntity
    {

        void Update(GameTime gameTime);
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);

    }

    internal class EntityBase
    {
        public Vector2 Position;
        public Vector2 Velocity;

        public Vector2 Origin { get; set; }
        public float Opacity { get; set; }
        public float Rotation { get; set; }
        public Vector2 Scale { get; set; }
        public Texture2D Texture { get; set; }

        // Contain Entity Within Screen Bounds
        public bool isContainedX;
        public bool isContainedY;

        public void Update(GameTime gameTime)
        {
            MoveEntity();
        }

        private void MoveEntity()
        {
            if (isContainedX) { Position.X += Position.X + Velocity.X <= Constants.GAME_WIDTH - Texture.Width / 2 && Position.X + Velocity.X + -Texture.Width / 2 >= 0 ? Velocity.X : 0; } else { Position.X += Velocity.X; }
            if (isContainedY) { Position.Y += Position.Y + Velocity.Y <= Constants.GAME_HEIGHT && Position.Y + Velocity.Y >= Texture.Height ? Velocity.Y : 0; } else { Position.Y += Velocity.Y; }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Color.White * Opacity, Rotation, Origin, Scale, SpriteEffects.None, 0f);

        }

    }

    class Entity : EntityBase, IEntity
    {
        public Entity()
        {

            // Defaults
            Position = new Vector2(0, 0);
            Origin = new Vector2(0, 0);
            Opacity = 1.0f;
            Scale = Vector2.One;
            isContainedX = isContainedY = true;

        }

    }
}