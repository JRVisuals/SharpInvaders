
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

using TexturePackerLoader;
using SharpInvaders.Constants;

namespace SharpInvaders.Entities
{

    class EnemySaucer
    {

        private ContentManager content;

        public SpriteBatch SpriteBatch;
        public SpriteSheet SpriteSheet;

        public Vector2 InitialPosition;
        public Vector2 Position;

        public float SpriteHeight;
        public float SpriteWidth;

        public bool isHittable;


        public SoundEffect sfxSaucer;
        private Random random;

        public Boolean isActive;

        public Player playerRef;

        public enum EnemyAnim
        {
            Idle,
            Pop,
        }
        public Dictionary<EnemyAnim, Animation[]> Animations { get; set; }
        public AnimatedSprite<EnemyAnim> AnimatedSprite;

        public EnemySaucer(ContentManager content, SpriteBatch spriteBatch, SpriteSheet spriteSheet, Vector2 initialPosition, Vector2 rowColPosition, Player player,)
        {

            this.content = content;
            this.SpriteBatch = spriteBatch;
            this.SpriteSheet = spriteSheet;

            this.playerRef = player;

            this.random = new Random();

            this.InitialPosition = this.Position = initialPosition;


            this.Animations = AnimationDictionary();

            this.isHittable = true;

            this.AnimatedSprite = new AnimatedSprite<EnemyAnim>(
                spriteBatch, spriteSheet, this.Animations,
                this.Animations[EnemyAnim.Idle],
                shouldStartOnRandomFrame: true
            );

            this.AnimatedSprite.Position = this.Position;

            this.AnimatedSprite.CurrentAnimationSequence = this.Animations[EnemyAnim.Idle];

            // Used for collision detection
            this.SpriteHeight = 32;
            this.SpriteWidth = 64;

            //sfxFire = this.content.Load<SoundEffect>("laserEnemy");

        }




        public void Die(GameTime gameTime)
        {
            this.AnimatedSprite.CurrentFrame = 0;
            this.AnimatedSprite.CurrentAnimationSequence = this.Animations[EnemySaucer.EnemyAnim.Pop];
            this.AnimatedSprite.shouldPlayOnceAndDie = true;
            this.AnimatedSprite.previousFrameChangeTime = gameTime.TotalGameTime;
            this.isHittable = false;
        }

        public void Respawn(GameTime gameTime)
        {


            this.Position = InitialPosition;

            var Anim = this.AnimatedSprite;
            Anim.Position = this.Position;
            Anim.CurrentAnimationSequence = this.Animations[EnemySaucer.EnemyAnim.Idle];
            Anim.shouldPlayOnceAndDie = false;
            Anim.previousFrameChangeTime = gameTime.TotalGameTime;
            this.isHittable = true;

            var rand = new Random();
            Anim.CurrentFrame = rand.Next(1, Anim.CurrentAnimationSequence[Anim.CurrentAnimation].Sprites.Length);
            Anim.isActive = true;
        }


        public void Update(GameTime gameTime, Vector2 virtualPosition)
        {


            if (this.isHittable)
            {
                this.AnimatedSprite.Position.X = this.InitialPosition.X + virtualPosition.X;


            }
            else
            {
                // Falling from the sky
                this.AnimatedSprite.Position.Y += 50 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }


            this.AnimatedSprite.Update(gameTime);

        }


        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            AnimatedSprite<EnemyAnim> Anim = this.AnimatedSprite;
            if (Anim.CurrentAnimationSequence == Anim.Animations[EnemyAnim.Pop] && Anim.CurrentFrame > 4) return;
            Anim.Opacity = 1.0f;
            Anim.Draw();
        }


        private Dictionary<EnemyAnim, Animation[]> AnimationDictionary()
        {

            Animation idle = null;
            Animation pop = null;

            string[] idleFrames = null;
            string[] popFrames = null;

            idleFrames = new[] {
                        TexturePackerMonoGameDefinitions.tpSprites.EnemySaucer_idle_0,
                        TexturePackerMonoGameDefinitions.tpSprites.EnemySaucer_idle_1,
                        TexturePackerMonoGameDefinitions.tpSprites.EnemySaucer_idle_2,
                        TexturePackerMonoGameDefinitions.tpSprites.EnemySaucer_idle_3,
                    };

            popFrames = new[] {
                        TexturePackerMonoGameDefinitions.tpSprites.EnemySaucer_pop_0,
                        TexturePackerMonoGameDefinitions.tpSprites.EnemySaucer_pop_1,
                        TexturePackerMonoGameDefinitions.tpSprites.EnemySaucer_pop_2,
                        TexturePackerMonoGameDefinitions.tpSprites.EnemySaucer_pop_3,
                    };

            idle = new Animation(timePerFrame: TimeSpan.FromSeconds(1f / 10f), SpriteEffects.None, idleFrames);
            pop = new Animation(timePerFrame: TimeSpan.FromSeconds(1f / 15f), SpriteEffects.None, popFrames);

            Dictionary<EnemyAnim, Animation[]> AnimationDictionary =
                new Dictionary<EnemyAnim, Animation[]>();

            if (idle != null) AnimationDictionary.Add(EnemyAnim.Idle, new[] { idle });
            if (pop != null) AnimationDictionary.Add(EnemyAnim.Pop, new[] { pop });

            return AnimationDictionary;
        }


    }

}
