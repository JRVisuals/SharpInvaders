
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SharpInvaders.Constants;

namespace SharpInvaders.Entities
{

    public interface IEntity
    {

        void Update(GameTime gameTime);
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);

    }

    class Entity : EntityBase, IEntity
    {
        public Entity()
        {

            // Defaults
            this.Position = new Vector2(0, 0);
            this.Origin = new Vector2(0, 0);
            this.Opacity = 1.0f;
            this.Scale = Vector2.One;
            this.isContainedX = isContainedY = true;
            this.isMovable = true;

        }

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
        public bool isMovable;


        public virtual void Update(GameTime gameTime)
        {
            if (this.isMovable) MoveEntity((float)gameTime.ElapsedGameTime.TotalSeconds);

        }

        private void MoveEntity(float deltaTime)
        {

            if (Math.Abs(Velocity.X) < 10) Velocity.X = 0;

            if (this.isContainedX)
            {
                Position.X += Velocity.X * deltaTime;
                var rightBound = Global.GAME_WIDTH - Texture.Width / 2;
                if (Position.X > rightBound) { Position.X = rightBound; Velocity.X = (float)(Velocity.X * -.5); }
                var leftBound = Texture.Width / 2;
                if (Position.X < leftBound) { Position.X = leftBound; Velocity.X = (float)(Velocity.X * -.5); }
            }
            else
            {
                Position.X += Velocity.X * deltaTime;
            }

            if (this.isContainedY)
            {
                Position.Y += Position.Y + Velocity.Y <= Global.GAME_HEIGHT &&
                Position.Y + Velocity.Y >= Texture.Height
                ? Velocity.Y * deltaTime : 0;
            }
            else
            {
                Position.Y += Velocity.Y * deltaTime;
            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.Texture, this.Position, null, Color.White * this.Opacity, this.Rotation, this.Origin, this.Scale, SpriteEffects.None, 0f);

        }

    }


}