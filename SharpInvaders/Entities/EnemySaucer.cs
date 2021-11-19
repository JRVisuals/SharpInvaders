
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

        public SpriteBatch SpriteBatch;
        public SpriteSheet SpriteSheet;

        public Vector2 InitialPosition;
        public Vector2 Position;


        public float SpriteHeight;
        public float SpriteWidth;

        public bool isHittable;


        public SoundEffect sfxSaucer;
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
            this.SpriteBatch = spriteBatch;
            this.SpriteSheet = spriteSheet;

            this.playerRef = player;

            this.random = new Random();

            this.InitialPosition = this.Position = initialPosition;

            this.moveSpeed = 1;

            this.Animations = this.AnimationDictionary();

            this.isActive = false;
            this.isHittable = false;

            this.AnimatedSprite = new AnimatedSprite<EnemyAnim>(
                spriteBatch, spriteSheet, this.Animations, this.Animations[EnemyAnim.Idle], false, false, "saucer");

            this.AnimatedSprite.Position = this.Position;

            this.AnimatedSprite.CurrentAnimationSequence = this.Animations[EnemyAnim.Idle];

            // Used for collision detection
            this.SpriteHeight = 32;
            this.SpriteWidth = 64;

            //sfxFire = this.content.Load<SoundEffect>("laserEnemy");

        }


        public void Die(GameTime gameTime)
        {
            var Anim = this.AnimatedSprite;
            Anim.CurrentFrame = 0;
            Anim.CurrentAnimationSequence = this.Animations[EnemySaucer.EnemyAnim.Pop];
            Anim.shouldPlayOnceAndDie = true;
            Anim.previousFrameChangeTime = gameTime.TotalGameTime;
            this.isHittable = false;
            this.isActive = true; // false works but no death anim
            Task.Delay(1000).ContinueWith((task) => this.isActive = false);
        }

        public void Respawn(GameTime gameTime)
        {
            Console.WriteLine("RESPAWN SAUCER");

            this.isHittable = true;
            this.isActive = true;

            this.Position = InitialPosition;

            var Anim = this.AnimatedSprite;
            Anim.CurrentFrame = 0;
            Anim.CurrentAnimationSequence = this.Animations[EnemySaucer.EnemyAnim.Idle];
            Anim.shouldPlayOnceAndDie = false;
            Anim.Position = this.Position;
            Anim.previousFrameChangeTime = gameTime.TotalGameTime;



            Console.WriteLine(this.Position);

        }

        public void Update(GameTime gameTime)
        {

            if (!this.isActive) return;

            var Anim = this.AnimatedSprite;
            Anim.Update(gameTime);

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
