
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using System.Collections.Generic;
using System;

using SharpInvaders.Constants;

namespace SharpInvaders
{
    class Bunker : Entity
    {


        public Bunker(ContentManager Content, Texture2D tex, float X)
        {

            Texture = tex;
            Position = new Vector2(X, Global.GAME_HEIGHT - 115);
            Origin = new Vector2(32, 64);
            Velocity = new Vector2(0, 0);

        }

        /* Clears the texture to transparent in the area defined by a rectangle.
           Currently there is no bounds checking. Do the right thing.
           */
        public void DestroyArea(Rectangle rect)
        {

            int cs = rect.X;
            int ce = rect.X + rect.Width;
            int rs = rect.Y;
            int re = rect.Y + rect.Height;

            Color[] data = new Color[Texture.Width * Texture.Height];
            Texture.GetData(data);

            Color clear = new Color(0);
            for (int i = 0; i < data.Length; i++)
            {
                var row = i / Texture.Width;
                var col = i % 64;
                if ((row >= rs && row <= re) &&
                    (col >= cs && col <= ce))
                    data[i] = clear;
            }

            Texture.SetData(data);

        }

        public bool CheckArea(Rectangle rect)
        {

            int cs = rect.X;
            int ce = rect.X + rect.Width;
            int rs = rect.Y;
            int re = rect.Y + rect.Height;

            Color[] data = new Color[Texture.Width * Texture.Height];
            Texture.GetData(data);

            var isSolid = false;
            for (int i = 0; i < data.Length; i++)
            {
                var row = i / Texture.Width;
                var col = i % 64;
                if ((row >= rs && row <= re) &&
                    (col >= cs && col <= ce) &&
                    data[i].A != 0
                    ) isSolid = true;

            }

            return isSolid;

        }


        public new void Update(GameTime gameTime)
        {


        }


    }

}