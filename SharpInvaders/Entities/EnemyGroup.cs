using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using System.Collections.Generic;
using SharpInvaders.Entities;
using TexturePackerLoader;
using System;

namespace SharpInvaders
{
    using SharpInvaders.Constants;
    using SharpInvaders.Entities;
    class EnemyGroup
    {

        private ContentManager content;
        public List<Enemy> Enemies;

        public Vector2 InitialPosition;
        public Vector2 Position;
        private float xSpeed;
        private float xSpeedMax;

        private int startY;
        private int totalColumns;
        private int totalRows;
        private int rowGap;

        private int xDir;

        private int yVirtualBound;
        private int yDropStep;

        public int countAlive;

        public Player playerRef;

        public EnemyGroup(ContentManager content, SpriteBatch spriteBatch, SpriteSheet spriteSheet, Player player, BunkerGroup bunkerGroup)
        {

            this.content = content;
            this.startY = Global.ENEMY_STARTY;
            this.totalColumns = Global.ENEMY_COLS;
            this.totalRows = Global.ENEMY_ROWS;
            this.rowGap = Global.ENEMY_ROWGAP;
            this.xSpeed = Global.ENEMY_SPEEDX;
            this.xSpeedMax = Global.ENEMY_SPEEDX_MAX;
            this.yDropStep = Global.ENEMY_DROPY;
            this.yVirtualBound = Global.ENEMY_MAXY;
            this.xDir = 1;
            this.countAlive = 0;

            this.playerRef = player;

            var positionX = (Global.GAME_WIDTH - 50) / totalColumns;
            Enemies = new List<Enemy>(totalRows * totalColumns);

            // This is a virtual position for the group itself
            InitialPosition = Position = new Vector2(0, 0);

            var enemyIndex = 0;
            for (int row = 0; row < totalRows; row++)
            {

                for (int col = 0; col < totalColumns; col++)
                {
                    var initialPosition = new Vector2(15 + (positionX * (col) + positionX / 2), startY + (row * rowGap));
                    var rowColPosition = new Vector2(row, col);
                    var enemyType = row < 3 ? row == 0 ? Enemy.EnemyType.Green : Enemy.EnemyType.Pink : Enemy.EnemyType.Blue;
                    var e = new Enemy(
                        this.content,
                        spriteBatch,
                        spriteSheet,
                        this,
                        enemyIndex,
                        initialPosition,
                        rowColPosition,
                        enemyType,
                        this.playerRef,
                        bunkerGroup);

                    Enemies.Add(e);
                    enemyIndex++;
                }
            }
        }

        public void ReSpawn(GameTime gameTime)
        {
            this.xSpeed = 0.5f;
            this.xDir = 1;
            Position = new Vector2(0, 0);
            foreach (var e in Enemies) e.Respawn(gameTime);

        }

        public void KillEnemy(int index, GameTime gameTime)
        {
            foreach (var e in Enemies)
            {
                if (e.EnemyIndex == index) e.Die(gameTime);
            }

        }

        private void GroupHitEdge(int newDir)
        {
            this.xDir = newDir;
            Position.Y += yDropStep;
            if (Position.Y > yVirtualBound) Position.Y = yVirtualBound;
        }

        public void Update(GameTime gameTime)
        {


            // Check edges to adjust 
            float xMin = 0;
            float xMax = Global.GAME_WIDTH - 32;


            var tempCount = 0;
            var edgeChecked = false;
            foreach (var e in Enemies)
            {
                if (e.isHittable)
                {
                    tempCount++;
                    // Check edges
                    var eaX = e.AnimatedSprite.Position.X;
                    if (eaX > xMax && !edgeChecked) { edgeChecked = true; GroupHitEdge(-1); }
                    if (eaX < xMin && !edgeChecked) { edgeChecked = true; GroupHitEdge(1); }
                }

            }
            this.countAlive = tempCount;

            if (this.countAlive == 0)
            {
                ReSpawn(gameTime);
                return;
            }

            if (this.countAlive < (this.totalColumns * this.totalRows) / 2) this.xSpeed = 1.0f;
            if (this.countAlive < (this.totalColumns * this.totalRows) / 4) this.xSpeed = 1.5f;
            if (this.countAlive == 1) this.xSpeed = 3.0f;

            // Walk them down using a virtual position
            Position.X += (xSpeed * xDir);

            foreach (var e in Enemies) { e.Update(gameTime, Position); }

        }

        public void UpdateMenu(GameTime gameTime)
        {


            // Check edges to adjust 
            float xMin = 0;
            float xMax = Global.GAME_WIDTH - 32;


            var tempCount = 0;
            var edgeChecked = false;
            foreach (var e in Enemies)
            {
                if (e.isHittable)
                {
                    tempCount++;
                    // Check edges
                    var eaX = e.AnimatedSprite.Position.X;
                    if (eaX > xMax && !edgeChecked) { edgeChecked = true; GroupHitEdge(-1); }
                    if (eaX < xMin && !edgeChecked) { edgeChecked = true; GroupHitEdge(1); }
                }

            }
            this.countAlive = tempCount;

            if (this.countAlive == 0)
            {
                ReSpawn(gameTime);
                return;
            }

            if (this.countAlive < (this.totalColumns * this.totalRows) / 2) this.xSpeed = 1.0f;
            if (this.countAlive < (this.totalColumns * this.totalRows) / 4) this.xSpeed = 1.5f;
            if (this.countAlive == 1) this.xSpeed = 3.0f;



            foreach (var e in Enemies) { e.UpdateMenu(gameTime, Position); }

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var e in Enemies) { e.Draw(gameTime, spriteBatch); }
        }

        public void DrawMenu(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var e in Enemies) { e.DrawMenu(gameTime, spriteBatch); }
        }




    }

}