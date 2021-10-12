
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using System.Collections.Generic;

namespace SharpInvaders
{
    class BunkerGroup
    {


        public List<Bunker> Bunkers;

        public BunkerGroup(ContentManager contentManager)
        {

            var positionX = Constants.GAME_WIDTH / Constants.BUNKERS_TOTAL;
            Bunkers = new List<Bunker>(Constants.BUNKERS_TOTAL);
            for (int i = 0; i < Bunkers.Capacity; i++)
            {
                Texture2D tex = contentManager.Load<Texture2D>($"bunker{i + 1}");
                Bunkers.Add(new Bunker(contentManager, tex, positionX * (i) + positionX / 2));
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