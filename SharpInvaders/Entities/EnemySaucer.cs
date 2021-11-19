
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using System.Threading.Tasks;

using TexturePackerLoader;
using SharpInvaders.Constants;

namespace SharpInvaders.Entities
{

    class EnemySaucer
    {

        private ContentManager content;

        public SpriteBatch spriteBatch;
        public SpriteSheet spriteSheet;

        public Vector2 initialPosition;
        public Vector2 position;


        public float spriteHeight;
        public float spriteWidth;

        public bool isHittable;


        private SoundEffect sfxSaucerDie;
        private SoundEffect sfxSaucer;
        public SoundEffectInstance sfxLoop;
        public int moveSpeed;

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

        public EnemySaucer(ContentManager content, SpriteBatch spriteBatch, SpriteSheet spriteSheet, Vector2 initialPosition, Player player)
        {

            this.content = content;
            this.spriteBatch = spriteBatch;
            this.spriteSheet = spriteSheet;

            this.playerRef = player;

            this.random = new Random();

            this.initialPosition = this.position = initialPosition;

            this.moveSpeed = 1;

            this.Animations = this.AnimationDictionary();

            this.isActive = false;
            this.isHittable = false;

            this.AnimatedSprite = new AnimatedSprite<EnemyAnim>(
                spriteBatch, spriteSheet, this.Animations, this.Animations[EnemyAnim.Idle], false, false, "saucer");

            this.AnimatedSprite.Position = this.position;

            this.AnimatedSprite.CurrentAnimationSequence = this.Animations[EnemyAnim.Idle];

            // Used for collision detection
            this.spriteHeight = 32;
            this.spriteWidth = 64;

            this.sfxSaucerDie = this.content.Load<SoundEffect>("saucerDie");
            this.sfxSaucer = this.content.Load<SoundEffect>("saucerloop");
            this.sfxLoop = sfxSaucer.CreateInstance();
            this.sfxLoop.IsLooped = true;


        }


        public void Die(GameTime gameTime)
        {
            this.sfxLoop.Stop();
            this.sfxSaucerDie.Play();
            var Anim = this.AnimatedSprite;
            Anim.CurrentFrame = 0;
            Anim.CurrentAnimationSequence = this.Animations[EnemySaucer.EnemyAnim.Pop];
            Anim.shouldPlayOnceAndDie = true;
            Anim.previousFrameChangeTime = gameTime.TotalGameTime;
            this.isHittable = false;
            this.isActive = true; // false works but no death anim
            Task.Delay(250).ContinueWith((task) => this.isActive = false);
        }

        public void Respawn(GameTime gameTime)
        {
            this.sfxLoop.Play();
            this.isHittable = true;
            this.isActive = true;

            this.position = initialPosition;

            var Anim = this.AnimatedSprite;
            Anim.CurrentFrame = 0;
            Anim.CurrentAnimationSequence = this.Animations[EnemySaucer.EnemyAnim.Idle];
            Anim.shouldPlayOnceAndDie = false;
            Anim.Position = this.position;
            Anim.previousFrameChangeTime = gameTime.TotalGameTime;

        }

        public void Update(GameTime gameTime)
        {
            if (!this.isActive) return;
            this.AnimatedSprite.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!this.isActive) return;

            var Anim = this.AnimatedSprite;

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
