
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using System.Collections.Generic;

namespace SharpInvaders
{
    class BunkerGroup
    {


        private List<Bunker> Bunkers;


        public BunkerGroup(ContentManager contentManager)
        {

            var positionX = 185;
            Bunkers = new List<Bunker>(4);
            for (int i = 0; i < Bunkers.Capacity; i++)
            {
                Bunkers.Add(new Bunker(contentManager, positionX * (i) + positionX / 2));
            }


        }

        public void Update(GameTime gameTime)
        {


        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            for (int i = 0; i < Bunkers.Count; i++)
            {
                Bunkers[i].Draw(gameTime, spriteBatch);
            }
        }




    }

}