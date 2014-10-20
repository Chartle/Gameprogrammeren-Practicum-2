using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.Diagnostics;

namespace Practicum2.gameobjects
{
    class TetrisGrid : GameObjectGrid
    {
        protected bool[,] boolGrid;
        protected Color[,] colorGrid;
        protected int objCounter, multiplier, removedY;
        SpriteGameObject block;
        float timer, maxTimer;
        bool timerstarted;
        protected int score, level, totalRemovedRows;

        public TetrisGrid(int columns, int rows, int layer = 0, string id = ""): base(columns, rows, layer, id)
        {
            // Initializes the boolean grid, the color grid and all the needed variables
            boolGrid = new bool[columns, rows];
            colorGrid = new Color[columns, rows];
            for (int x = 0; x < columns; x++)
                for (int y = 0; y < rows; y++)
                {
                    boolGrid[x, y] = false;
                    colorGrid[x, y] = Color.White;
                }

            score = 0;
            objCounter = 0;
            removedY = 0;
            maxTimer = 0.05f;
            timer = maxTimer;
            timerstarted = false;
            multiplier = 0;
            level = 1;
            totalRemovedRows = 0;
        }

        public void CheckRemoveRow()
        {
            int counter;
            for (int y = 2; y < Rows; y++)
            {
                counter = 0;
                //grid is 2 bigger at each ends
                for (int x = 2; x < Columns - 2; x++)
                {
                    if (boolGrid[x, y] == false)
                    {
                        counter++;
                    }
                }
                if (counter == 0)
                {
                    multiplier++;
                    for (int x = 2; x < Columns - 2; x++)
                    {
                        //ADD SCORE
                        colorGrid[x, y] = Color.White;
                        boolGrid[x, y] = false;
                        removedY = y;
                    }
                    //START TIMER
                    timer = maxTimer;
                    timerstarted = true;
                    totalRemovedRows++;
                }
            }
            //If the grid has to be moved, move it the amount of rows deleted
            if (timer < 0)
            {
                for (int i = 0; i < multiplier; i++)
                {
                    for (int y = removedY; y >= 2; y--)
                    {
                        for (int x = 2; x < Columns - 2; x++)
                        {
                            colorGrid[x, y] = colorGrid[x, y - 1];
                            boolGrid[x, y] = boolGrid[x, y - 1];
                        }
                    }
                }

                score += multiplier * multiplier;

                Tetris.AssetManager.PlaySound("audio/clearLine");
                
                timerstarted = false;
                timer = maxTimer;
                multiplier = 0;
            }
        }

        /*public bool IsRowEmpty(int y)
        {
            for(int x = 0; x < Columns; x++)
            {
                if(y < Rows)
                    if (boolGrid[x, y])
                        return false;
            }
            return true;
        }*/

        /*public void AddAll(GameObject obj, bool booltemp, Color color, int x, int y)
        {
            grid[x, y] = obj;
            boolGrid[x, y] = booltemp;
            colorGrid[x, y] = color;
        }*/

        public void AddBool(bool obj, int x, int y)
        {   
                boolGrid[x, y] = obj;
        }

        public bool GetBool(int x, int y)
        {
            if (x >= 0 && x < grid.GetLength(0) && y >= 0 && y < grid.GetLength(1))
                return boolGrid[x, y];
            else
                return false;
        }


        public void AddColor(Color color, int x, int y)
        {
            colorGrid[x, y] = color;
        }

        /*public Color GetColor(int x, int y)
        {
            if (x >= 0 && x < grid.GetLength(0) && y >= 0 && y < grid.GetLength(1))
                return colorGrid[x, y];
            else
                return Color.White;
        }

        protected void RemoveRow(int y)
        {
            // check and/or remove row
        }

        public void Move(int x, int y, int newX, int newY)
        {
            grid[newX, newY] = grid[x, y];
            grid[x, y] = null;
        }*/

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Draws all the landed blocks using the boolean grid and the color grid
            for (int x = 0; x < Columns; x++)
            {
                for (int y = 0; y < Rows; y++) 
                {
                    if (boolGrid[x,y])
                    {
                        if (y > 4)
                        {
                            block = new SpriteGameObject("sprites/block", 0);
                            block.Sprite.Draw(spriteBatch, position + new Vector2(x * 30, y * 30), Vector2.Zero, colorGrid[x, y]);
                        }
                    }
                }
            }
            base.Draw(gameTime, spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            int previousLevel = level;
            level = (totalRemovedRows / 10) + 1;

            // If the level has changed, play a sound effect
            if(previousLevel != level)
                Tetris.AssetManager.PlaySound("audio/levelUp");
            
            // Update the timer and check if rows have to be removed
            if (timerstarted)
            {
                timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            CheckRemoveRow();

            // Check how many objects are in the grid
            objCounter = 0;
            foreach (GameObject obj in grid)
            {
                if(obj!= null)
                    objCounter++;
            }
            
            base.Update(gameTime);
        }

        public override void Reset()
        {
            // Clear the grids

            for (int x = 0; x < Columns; x++)
            {
                for (int y = 0; y < Rows; y++)
                {
                    boolGrid[x, y] = false;
                    colorGrid[x, y] = Color.White;
                    grid[x, y] = null;
                }
            }

            base.Reset();
        }
        
        /*public bool[,] ObjectsBool
        {
            get
            {
                return boolGrid;
            }
        }

        public Color[,] ObjectsColor
        {
            get
            {
                return colorGrid;
            }
        }*/

        public int ObjCounter
        {
            get
            {
                return objCounter;
            }
        }

        public int Score
        {
            get { return score; }
            set { score = value; }
        }

        public int Level
        {
            get { return level; }
        }
    }
}
