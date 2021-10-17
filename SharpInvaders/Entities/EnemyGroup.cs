using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using System.Collections.Generic;
using SharpInvaders.Constants;
using SharpInvaders.Entities;
using TexturePackerLoader;
using System;

namespace SharpInvaders
{
    class EnemyGroup
    {

        public List<Enemy> Enemies;

        private Vector2 Position;
        private float xSpeed;

        private int startY;
        private int totalColumns;
        private int totalRows;
        private int rowGap;

        private int xDir;
        private int xVirtualBoundMax;
        private int xVirtualBoundMin;
        private int yVirtualBound;
        private int yDropStep;

        public EnemyGroup(SpriteBatch spriteBatch, SpriteSheet spriteSheet)
        {

            this.startY = 180;
            this.totalColumns = 12;
            this.totalRows = 4;
            this.rowGap = 40;

            this.xSpeed = 0.5f;
            this.xDir = 1;
            this.xVirtualBoundMax = 32;
            this.xVirtualBoundMin = -32;
            this.yDropStep = 10;
            this.yVirtualBound = this.yDropStep * 20;

            var positionX = (Global.GAME_WIDTH - 50) / totalColumns;
            Enemies = new List<Enemy>(totalRows * totalColumns);

            // This is a virtual position for the group itself
            Position = new Vector2(0, 0);

            var enemyIndex = 0;
            for (int row = 0; row < totalRows; row++)
            {

                for (int col = 0; col < totalColumns; col++)
                {
                    var initialPosition = new Vector2(15 + (positionX * (col) + positionX / 2), startY + (row * rowGap));
                    var rowColPosition = new Vector2(row, col);
                    var e = new Enemy(spriteBatch, spriteSheet, this, enemyIndex, initialPosition, rowColPosition);

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


            // Check edges to adjust 
            float xMin = Global.GAME_WIDTH;
            float xMax = 0;

            var countAlive = 0;
            foreach (var e in Enemies)
            {
                if (e.isHittable)
                {
                    countAlive++;
                    var eaX = e.AnimatedSprite.Position.X;
                    if (eaX > xMax) xMax = eaX;
                    if (eaX < xMin) xMin = eaX;
                }
            }

            if (countAlive < (this.totalColumns * this.totalRows) / 2) this.xSpeed = 1.5f;



            // Walk them down using a virtual position
            Position.X += (xSpeed * xDir);

            if (Position.X >= xVirtualBoundMax)
            {
                xDir = -1;
                Position.X += (xSpeed * xDir);

                Position.Y += yDropStep;
                if (Position.Y > yVirtualBound) Position.Y = yVirtualBound;
            }

            if (Position.X <= xMin * -1000)
            {

                Console.WriteLine($"x:{Position.X} bound:{this.xVirtualBoundMin - (xMin * this.totalColumns * 1.5)}");

                xDir = 1;
                Position.X += (xSpeed * xDir);

                Position.Y += yDropStep;
                if (Position.Y > yVirtualBound) Position.Y = yVirtualBound;
            }

            foreach (var e in Enemies) { e.Update(gameTime, Position); }

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var e in Enemies) { e.Draw(); }
        }




    }

}