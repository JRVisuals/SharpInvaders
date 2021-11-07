
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
        public EnemyGroup EnemyGroup;
        public int EnemyIndex;

        public Vector2 InitialPosition;
        public Vector2 Position;
        public Vector2 RowColPosition;
        public float SpriteHeight;
        public float SpriteWidth;

        public bool isHittable;


        public enum EnemyType
        {
            Blue,
            Pink,
        }

        public enum EnemyAnim
        {
            Idle,
            IdleFast,
            Pop,
        }
        public Dictionary<EnemyAnim, Animation[]> Animations { get; set; }
        public AnimatedSprite<EnemyAnim> AnimatedSprite;

        public Enemy(SpriteBatch spriteBatch, SpriteSheet spriteSheet, EnemyGroup enemyGroup, int enemyIndex, Vector2 initialPosition, Vector2 rowColPosition, EnemyType enemyType)
        {

            this.SpriteBatch = spriteBatch;
            this.SpriteSheet = spriteSheet;
            this.EnemyGroup = enemyGroup;
            this.EnemyIndex = enemyIndex;

            this.InitialPosition = this.Position = initialPosition;
            this.RowColPosition = rowColPosition;

            this.Animations = AnimationDictionary(enemyType);

            this.isHittable = true;

            this.AnimatedSprite = new AnimatedSprite<EnemyAnim>(
                spriteBatch, spriteSheet, this.Animations,
                this.Animations[EnemyAnim.Idle],
                shouldStartOnRandomFrame: true
            );

            this.AnimatedSprite.Position = this.Position;

            this.AnimatedSprite.CurrentAnimationSequence = this.Animations[EnemyAnim.Idle];

            // Used for collision detection
            this.SpriteHeight = this.SpriteWidth = 32;

        }

        public void Die(GameTime gameTime)
        {
            this.AnimatedSprite.CurrentFrame = 0;
            this.AnimatedSprite.CurrentAnimationSequence = this.Animations[Enemy.EnemyAnim.Pop];
            this.AnimatedSprite.shouldPlayOnceAndDie = true;
            this.AnimatedSprite.previousFrameChangeTime = gameTime.TotalGameTime;
            this.isHittable = false;
        }

        public void Respawn(GameTime gameTime)
        {
            this.Position = InitialPosition;

            var Anim = this.AnimatedSprite;
            Anim.Position = this.Position;
            Anim.CurrentAnimationSequence = this.Animations[Enemy.EnemyAnim.Idle];
            Anim.shouldPlayOnceAndDie = false;
            Anim.previousFrameChangeTime = gameTime.TotalGameTime;
            this.isHittable = true;

            var rand = new Random();
            Anim.CurrentFrame = rand.Next(1, Anim.CurrentAnimationSequence[Anim.CurrentAnimation].Sprites.Length);
            Anim.isActive = true;
        }

        public void Update(GameTime gameTime, Vector2 virtualPosition)
        {

            this.AnimatedSprite.Update(gameTime);


            //new Vector2(15 + (positionX * (col) + positionX / 2), startY + (row * rowGap));

            this.AnimatedSprite.Position.X = this.InitialPosition.X + virtualPosition.X;
            this.AnimatedSprite.Position.Y = this.InitialPosition.Y + virtualPosition.Y;

            // Falling from the sky
            if (!this.isHittable) this.AnimatedSprite.Position.Y += 50 * (float)gameTime.ElapsedGameTime.TotalSeconds;

        }

        public void Draw()
        {
            AnimatedSprite<EnemyAnim> Anim = this.AnimatedSprite;
            if (Anim.CurrentAnimationSequence == Anim.Animations[EnemyAnim.Pop] && Anim.CurrentFrame > 4) return;
            this.AnimatedSprite.Draw();
        }




        // TODO: Is it crazy innefficient to generate these for every instance??
        private Dictionary<EnemyAnim, Animation[]> AnimationDictionary(EnemyType enemyType)
        {

            Animation idle = null;
            Animation idleFast = null;
            Animation pop = null;

            string[] idleFrames = null;
            string[] popFrames = null;

            switch (enemyType)
            {
                case EnemyType.Blue:

                    idleFrames = new[] {
                        TexturePackerMonoGameDefinitions.tpSprites.EnemyEyes_idle_0,
                        TexturePackerMonoGameDefinitions.tpSprites.EnemyEyes_idle_1,
                        TexturePackerMonoGameDefinitions.tpSprites.EnemyEyes_idle_2,
                        TexturePackerMonoGameDefinitions.tpSprites.EnemyEyes_idle_3,
                        TexturePackerMonoGameDefinitions.tpSprites.EnemyEyes_idle_4,
                    };

                    popFrames = new[] {
                        TexturePackerMonoGameDefinitions.tpSprites.EnemyEyes_pop_0,
                        TexturePackerMonoGameDefinitions.tpSprites.EnemyEyes_pop_1,
                        TexturePackerMonoGameDefinitions.tpSprites.EnemyEyes_pop_2,
                        TexturePackerMonoGameDefinitions.tpSprites.EnemyEyes_pop_3,
                        TexturePackerMonoGameDefinitions.tpSprites.EnemyEyes_pop_4,
                        TexturePackerMonoGameDefinitions.tpSprites.EnemyEyes_pop_5,
                    };

                    idle = new Animation(timePerFrame: TimeSpan.FromSeconds(1f / 10f), SpriteEffects.None, idleFrames);
                    idleFast = new Animation(timePerFrame: TimeSpan.FromSeconds(1f / 15f), SpriteEffects.None, idleFrames);
                    pop = new Animation(timePerFrame: TimeSpan.FromSeconds(1f / 15f), SpriteEffects.None, popFrames);

                    break;

                case EnemyType.Pink:

                    idleFrames = new[] {
                        TexturePackerMonoGameDefinitions.tpSprites.EnemyPinks_idle_0,
                        TexturePackerMonoGameDefinitions.tpSprites.EnemyPinks_idle_1,
                        TexturePackerMonoGameDefinitions.tpSprites.EnemyPinks_idle_2,
                        TexturePackerMonoGameDefinitions.tpSprites.EnemyPinks_idle_3,
                        TexturePackerMonoGameDefinitions.tpSprites.EnemyPinks_idle_4,
                    };

                    popFrames = new[] {
                        TexturePackerMonoGameDefinitions.tpSprites.EnemyPinks_pop_0,
                        TexturePackerMonoGameDefinitions.tpSprites.EnemyPinks_pop_1,
                        TexturePackerMonoGameDefinitions.tpSprites.EnemyPinks_pop_2,
                        TexturePackerMonoGameDefinitions.tpSprites.EnemyPinks_pop_3,
                        TexturePackerMonoGameDefinitions.tpSprites.EnemyPinks_pop_4,
                        TexturePackerMonoGameDefinitions.tpSprites.EnemyPinks_pop_5,
                    };
                    idle = new Animation(timePerFrame: TimeSpan.FromSeconds(1f / 10f), SpriteEffects.None, idleFrames);
                    idleFast = new Animation(timePerFrame: TimeSpan.FromSeconds(1f / 15f), SpriteEffects.None, idleFrames);
                    pop = new Animation(timePerFrame: TimeSpan.FromSeconds(1f / 15f), SpriteEffects.None, popFrames);

                    break;


                default:
                    break;
            }




            Dictionary<EnemyAnim, Animation[]> AnimationDictionary =
                new Dictionary<EnemyAnim, Animation[]>();

            if (idle != null) AnimationDictionary.Add(EnemyAnim.Idle, new[] { idle });
            if (idleFast != null) AnimationDictionary.Add(EnemyAnim.IdleFast, new[] { idleFast });
            if (pop != null) AnimationDictionary.Add(EnemyAnim.Pop, new[] { pop });

            return AnimationDictionary;
        }


    }

}
