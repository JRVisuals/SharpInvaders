
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

        public enum EnemyAnims
        {
            Idle,
            IdleFast,
            Pop,
            Test
        }
        private Dictionary<EnemyAnims, Animation[]> Animations { get; set; }
        public AnimatedSprite<EnemyAnims> AnimatedSprite;

        public Enemy(SpriteBatch spriteBatch, SpriteSheet spriteSheet)
        {

            this.SpriteBatch = spriteBatch;
            this.SpriteSheet = spriteSheet;

            this.Animations = AnimationDictionary();

            this.AnimatedSprite = new AnimatedSprite<EnemyAnims>(spriteBatch, spriteSheet, this.Animations, this.Animations[EnemyAnims.Idle]);

            this.AnimatedSprite.Position = new Vector2(100f, 300f);

            // TODO: Sort out a way to pull these as enumarations rather than string
            this.AnimatedSprite.animationSequence = this.Animations[EnemyAnims.Test];

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

            var idle = new Animation(timePerFrame: TimeSpan.FromSeconds(1f / 12f), SpriteEffects.None, EyesIdleFrames);
            var idleFast = new Animation(timePerFrame: TimeSpan.FromSeconds(1f / 18f), SpriteEffects.None, EyesIdleFrames);
            var pop = new Animation(timePerFrame: TimeSpan.FromSeconds(1f / 18f), SpriteEffects.None, EyesPopFrames);

            Dictionary<EnemyAnims, Animation[]> AnimationDictionary =
                new Dictionary<EnemyAnims, Animation[]>();

            AnimationDictionary.Add(EnemyAnims.Idle, new[] { idle });
            AnimationDictionary.Add(EnemyAnims.IdleFast, new[] { idleFast });
            AnimationDictionary.Add(EnemyAnims.Pop, new[] { pop });
            AnimationDictionary.Add(EnemyAnims.Test, new[] { idle, idle, idle, idle, pop });

            return AnimationDictionary;
        }

        public void Update(GameTime gameTime)
        {
            this.AnimatedSprite.Update(gameTime);
        }

        public void Draw()
        {
            this.AnimatedSprite.Draw();
        }


    }

}
