
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
        public Animation[] CurrentAnimationSequence { get; set; }
        public bool shouldPlayOnceAndDie;
        public bool isActive;

        public TimeSpan previousFrameChangeTime = TimeSpan.Zero;
        public TimeSpan previousMovementTime = TimeSpan.Zero;
        private SpriteSheet spriteSheet;
        private SpriteBatch spriteBatch;

        private const float ClockwiseNinetyDegreeRotation = (float)(Math.PI / 2.0f);

        public SpriteFrame CurrentSprite { get; private set; }
        public SpriteEffects CurrentSpriteEffects { get; private set; }
        public int CurrentFrame { get; set; }
        public int CurrentAnimation { get; private set; }

        public string Name { get; set; }


        public AnimatedSprite(SpriteBatch spriteBatch, SpriteSheet spriteSheet, Dictionary<AnimKeys, Animation[]> animationDictionary, Animation[] defaultAnimationSequence, bool shouldStartOnRandomFrame = false, bool shouldPlayOnceAndDie = false, string name = "generic")
        {
            this.Name = name;
            this.Animations = animationDictionary;
            this.spriteSheet = spriteSheet;
            this.CurrentAnimationSequence = defaultAnimationSequence;
            this.spriteBatch = spriteBatch;
            this.shouldPlayOnceAndDie = shouldPlayOnceAndDie;
            this.isActive = true;

            if (shouldStartOnRandomFrame)
            {
                var rand = new Random();
                CurrentFrame = rand.Next(1, this.CurrentAnimationSequence[this.CurrentAnimation].Sprites.Length);
            }

            // Force animation update - otherwise we have an issue with sprites that are instantiated but not updated/drawn until later 
            var animation = this.CurrentAnimationSequence[this.CurrentAnimation];
            this.CurrentSprite = this.spriteSheet.Sprite(animation.Sprites[this.CurrentFrame]);

        }

        public override void Update(GameTime gameTime)
        {
            if (!this.isActive) return;

            var nowTime = gameTime.TotalGameTime;
            var dtFrame = nowTime - this.previousFrameChangeTime;
            var dtPosition = nowTime - this.previousMovementTime;

            // Allows for sequencing of multiple animations
            var animation = this.CurrentAnimationSequence[this.CurrentAnimation];

            if (dtFrame >= animation.TimePerFrame)
            {
                this.previousFrameChangeTime = nowTime;
                this.CurrentFrame++;

                if (this.CurrentFrame >= animation.Sprites.Length)
                {
                    if (!this.shouldPlayOnceAndDie)
                    {
                        this.CurrentFrame = 0;
                        if (++this.CurrentAnimation >= this.CurrentAnimationSequence.Length)
                        {
                            this.CurrentAnimation = 0;
                        }

                        animation = this.CurrentAnimationSequence[this.CurrentAnimation];
                    }
                    else
                    {
                        this.isActive = false;
                        return;
                    }
                }

                this.CurrentSpriteEffects = animation.Effect;
            }

            this.CurrentSprite = this.spriteSheet.Sprite(animation.Sprites[this.CurrentFrame]);
            this.previousMovementTime = nowTime;

            base.Update(gameTime);
        }

        public void Draw()
        {

            if (!this.isActive) return;

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

        public void DrawMenu()
        {

            if (!this.isActive) return;

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

            spriteBatch.Draw(
                texture: this.CurrentSprite.Texture,
                position: this.Position,
                sourceRectangle: this.CurrentSprite.SourceRectangle,
                color: new Color(0, 0, 0, 100),
                rotation: this.Rotation,
                origin: this.Origin,
                scale: this.Scale,
                effects: SpriteEffects.None,
                layerDepth: 0.0f
            );

        }



    }


}
