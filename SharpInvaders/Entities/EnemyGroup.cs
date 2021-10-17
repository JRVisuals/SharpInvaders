using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using System.Collections.Generic;
using SharpInvaders.Constants;
using SharpInvaders.Entities;
using TexturePackerLoader;

namespace SharpInvaders
{
    class EnemyGroup
    {

        public List<Enemy> Enemies;

        public EnemyGroup(SpriteBatch spriteBatch, SpriteSheet spriteSheet)
        {

            var startY = 180;
            var perRow = 12;
            var totalRows = 7;
            var rowGap = 40;

            var positionX = (Global.GAME_WIDTH - 50) / perRow;
            Enemies = new List<Enemy>(totalRows * perRow);


            var enemyIndex = 0;
            for (int row = 0; row < totalRows; row++)
            {

                for (int i = 0; i < perRow; i++)
                {
                    var initialPosition = new Vector2(15 + (positionX * (i) + positionX / 2), startY + (row * rowGap));
                    var e = new Enemy(spriteBatch, spriteSheet, this, enemyIndex, initialPosition);

                    Enemies.Add(e);
                    enemyIndex++;
                }
            }
        }

        public void KillEnemy(int index, GameTime gameTime)
        {
            foreach (var e in Enemies)
            {
                if (e.EnemyIndex == index) e.Die(gameTime);
            }

        }

        public void Update(GameTime gameTime)
        {

            for (int i = 0; i < Enemies.Count; i++)
            {
                Enemies[i].Update(gameTime);
            }


        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            for (int i = 0; i < Enemies.Count; i++)
            {
                Enemies[i].Draw();
            }
        }




    }

}