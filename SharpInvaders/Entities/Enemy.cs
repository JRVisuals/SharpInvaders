
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using TexturePackerLoader;

namespace SharpInvaders.Entities
{

    // TODO: How would I pass in data using this interface?
    // public interface ISpriteData
    // {

    //     SpriteBatch spriteBatch { get; set; }
    //     SpriteSheet spriteSheet { get; set; }

    // }

    class Enemy
    {

        public SpriteBatch SpriteBatch;
        public SpriteSheet SpriteSheet;

        public Vector2 Position { get; set; }
        public float SpriteHeight;
        public float SpriteWidth;

        public bool isHittable;

        public enum EnemyAnims
        {
            Idle,
            IdleFast,
            Pop,
            Test
        }
        public Dictionary<EnemyAnims, Animation[]> Animations { get; set; }
        public AnimatedSprite<EnemyAnims> AnimatedSprite;

        public Enemy(SpriteBatch spriteBatch, SpriteSheet spriteSheet, Vector2 initialPosition)
        {

            this.SpriteBatch = spriteBatch;
            this.SpriteSheet = spriteSheet;

            this.Position = initialPosition;

            this.Animations = AnimationDictionary();

            this.isHittable = true;

            this.AnimatedSprite = new AnimatedSprite<EnemyAnims>(
                spriteBatch, spriteSheet, this.Animations,
                this.Animations[EnemyAnims.Idle],
                shouldStartOnRandomFrame: true
            );

            this.AnimatedSprite.Position = this.Position;

            this.AnimatedSprite.CurrentAnimationSequence = this.Animations[EnemyAnims.Idle];

            // Used for collision detection
            this.SpriteHeight = this.SpriteWidth = 32;

        }


        public void Update(GameTime gameTime)
        {
            this.AnimatedSprite.Update(gameTime);
            if (!this.isHittable) this.AnimatedSprite.Position.Y += 50 * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Draw()
        {
            AnimatedSprite<EnemyAnims> Anim = this.AnimatedSprite;
            if (Anim.CurrentAnimationSequence == Anim.Animations[EnemyAnims.Pop] && Anim.CurrentFrame > 4) return;
            this.AnimatedSprite.Draw();
        }




        // TODO: Is it crazy innefficient to generate these for every instance??
        private Dictionary<EnemyAnims, Animation[]> AnimationDictionary()
        {


            var EyesIdleFrames = new[] {
                TexturePackerMonoGameDefinitions.tpSprites.EnemyEyes_idle_0,
                TexturePackerMonoGameDefinitions.tpSprites.EnemyEyes_idle_1,
                TexturePackerMonoGameDefinitions.tpSprites.EnemyEyes_idle_2,
                TexturePackerMonoGameDefinitions.tpSprites.EnemyEyes_idle_3,
                TexturePackerMonoGameDefinitions.tpSprites.EnemyEyes_idle_4,
            };

            var EyesPopFrames = new[] {
                TexturePackerMonoGameDefinitions.tpSprites.EnemyEyes_pop_0,
                TexturePackerMonoGameDefinitions.tpSprites.EnemyEyes_pop_1,
                TexturePackerMonoGameDefinitions.tpSprites.EnemyEyes_pop_2,
                TexturePackerMonoGameDefinitions.tpSprites.EnemyEyes_pop_3,
                TexturePackerMonoGameDefinitions.tpSprites.EnemyEyes_pop_4,
                TexturePackerMonoGameDefinitions.tpSprites.EnemyEyes_pop_5,
            };

            var idle = new Animation(timePerFrame: TimeSpan.FromSeconds(1f / 10f), SpriteEffects.None, EyesIdleFrames);
            var idleFast = new Animation(timePerFrame: TimeSpan.FromSeconds(1f / 15f), SpriteEffects.None, EyesIdleFrames);
            var pop = new Animation(timePerFrame: TimeSpan.FromSeconds(1f / 15f), SpriteEffects.None, EyesPopFrames);

            Dictionary<EnemyAnims, Animation[]> AnimationDictionary =
                new Dictionary<EnemyAnims, Animation[]>();

            AnimationDictionary.Add(EnemyAnims.Idle, new[] { idle });
            AnimationDictionary.Add(EnemyAnims.IdleFast, new[] { idleFast });
            AnimationDictionary.Add(EnemyAnims.Pop, new[] { pop });
            AnimationDictionary.Add(EnemyAnims.Test, new[] { idle, idle, idle, idle, pop });

            return AnimationDictionary;
        }


    }

}
