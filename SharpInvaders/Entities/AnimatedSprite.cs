
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SharpInvaders.Constants;
using TexturePackerLoader;

namespace SharpInvaders.Entities
{

    class AnimatedSprite<AnimKeys> : Entity
    {

        public Dictionary<AnimKeys, Animation[]> Animations { get; set; }
        public Animation[] animationSequence { get; set; }

        private TimeSpan previousFrameChangeTime = TimeSpan.Zero;
        private TimeSpan previousMovementTime = TimeSpan.Zero;
        private SpriteSheet spriteSheet;
        private SpriteBatch spriteBatch;

        private const float ClockwiseNinetyDegreeRotation = (float)(Math.PI / 2.0f);


        public AnimatedSprite(SpriteBatch spriteBatch, SpriteSheet spriteSheet, Dictionary<AnimKeys, Animation[]> animationDictionary, Animation[] defaultAnimationSequence)
        {
            this.Animations = animationDictionary;
            this.spriteSheet = spriteSheet;
            this.animationSequence = defaultAnimationSequence;
            this.spriteBatch = spriteBatch;

        }



        public SpriteFrame CurrentSprite { get; private set; }
        public SpriteEffects CurrentSpriteEffects { get; private set; }
        public int CurrentFrame { get; private set; }
        public int CurrentAnimation { get; private set; }

        public override void Update(GameTime gameTime)
        {
            var nowTime = gameTime.TotalGameTime;
            var dtFrame = nowTime - this.previousFrameChangeTime;
            var dtPosition = nowTime - this.previousMovementTime;

            // Allows for sequencing of multiple animations
            var animation = this.animationSequence[this.CurrentAnimation];

            if (dtFrame >= animation.TimePerFrame)
            {
                this.previousFrameChangeTime = nowTime;
                this.CurrentFrame++;

                if (this.CurrentFrame >= animation.Sprites.Length)
                {
                    this.CurrentFrame = 0;
                    if (++this.CurrentAnimation >= this.animationSequence.Length)
                    {
                        this.CurrentAnimation = 0;
                    }

                    animation = this.animationSequence[this.CurrentAnimation];
                }

                this.CurrentSpriteEffects = animation.Effect;
            }

            this.CurrentSprite = this.spriteSheet.Sprite(animation.Sprites[this.CurrentFrame]);
            this.previousMovementTime = nowTime;
        }

        public void Draw()
        {

            // Supports rotated sprites in spritesheets
            if (this.CurrentSprite.IsRotated)
            {
                this.Rotation -= ClockwiseNinetyDegreeRotation;
                switch (this.CurrentSpriteEffects)
                {
                    case SpriteEffects.FlipHorizontally: this.CurrentSpriteEffects = SpriteEffects.FlipVertically; break;
                    case SpriteEffects.FlipVertically: this.CurrentSpriteEffects = SpriteEffects.FlipHorizontally; break;
                }
            }

            // Supports horizontal and vertical sprite flipping (needs work)
            // switch (this.CurrentSpriteEffects)
            // {
            //     case SpriteEffects.FlipHorizontally: this.Origin.X = this.CurrentSprite.SourceRectangle.Width - this.Origin.X; break;
            //     case SpriteEffects.FlipVertically: this.Origin.Y = this.CurrentSprite.SourceRectangle.Height - this.Origin.Y; break;
            // }

            spriteBatch.Draw(
                texture: this.CurrentSprite.Texture,
                position: this.Position,
                sourceRectangle: this.CurrentSprite.SourceRectangle,
                color: Color.White,
                rotation: this.Rotation,
                origin: this.Origin,
                scale: this.Scale,
                effects: SpriteEffects.None,
                layerDepth: 0.0f
            );
        }



    }


}
