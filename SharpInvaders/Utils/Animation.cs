namespace SharpInvaders
{
    using System;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class Animation
    {
        public Animation(TimeSpan timePerFrame, SpriteEffects effect, string[] sprites)
        {
            this.Sprites = sprites;
            this.TimePerFrame = timePerFrame;
            this.Effect = effect;
        }

        public SpriteEffects Effect { get; private set; }

        public TimeSpan TimePerFrame { get; private set; }

        public string[] Sprites { get; private set; }
    }
}