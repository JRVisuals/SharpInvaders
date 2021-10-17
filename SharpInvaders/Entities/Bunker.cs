
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using System.Collections.Generic;
using System;

using SharpInvaders.Constants;
using SharpInvaders.Entities;

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

        public bool CheckArea(Rectangle rect)
        {

            var more = -2;
            int cs = rect.X - more;
            int ce = rect.X + rect.Width + more;
            int rs = rect.Y - more;
            int re = rect.Y + rect.Height + more;

            Color[] data = new Color[Texture.Width * Texture.Height];
            Texture.GetData(data);

            Color clear = new Color(0);

            var isSolid = false;

            for (int i = 0; i < data.Length; i++)
            {
                var row = i / Texture.Width;
                var col = i % 64;

                if ((row >= rs && row <= re) &&
                    (col >= cs && col <= ce))
                {
                    if (data[i] != clear)
                    {
                        isSolid = true;
                        data[i] = clear;
                    }
                }
            }

            Texture.SetData(data);

            return isSolid;

        }


        public new void Update(GameTime gameTime)
        {


        }


    }

}