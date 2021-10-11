
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using System.Collections.Generic;

namespace SharpInvaders
{
    class Bunker : Entity
    {

        public int BunkerIndex;

        public Bunker(ContentManager Content, float X)
        {

            Texture = Content.Load<Texture2D>("bunker1");
            Position = new Vector2(X, Constants.GAME_HEIGHT - 100);
            Origin = new Vector2(32, 64);
            Velocity = new Vector2(0, 0);

        }

        public new void Update(GameTime gameTime)
        {


        }


    }

}