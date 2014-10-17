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
        protected int score;
        public int level;
        protected int totalRemovedRows;

        public TetrisGrid(int columns, int rows, int layer = 0, string id = ""): base(columns, rows, layer, id)
        {
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
        
        public int Score
        {
            get { return score; }
            set { score = value; }
        }
        
        public int Level
        {
            get { return level; }
        }

        //checks if there is a full row then removes them later with a timer
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
            //movegrid
            if (timer < 0)
            {
                for (int i = 0; i < multiplier; i++)
                {
                    for (int y2 = removedY; y2 >= 2; y2--)
                    {
                        for (int x2 = 2; x2 < Columns - 2; x2++)
                        {
                            colorGrid[x2, y2] = colorGrid[x2, y2 - 1];
                            boolGrid[x2, y2] = boolGrid[x2, y2 - 1];
                            //removedY++;
                        }
                    }
                }

                score += multiplier * multiplier;
                Tetris.AssetManager.PlayMusic("sprites/soundeffect", false);
                //if(IsRowEmpty(removedY))
                    timerstarted = false;
                timer = maxTimer;
                multiplier = 0;
            }
        }

        public bool IsRowEmpty(int y)
        {
            for(int x = 0; x < Columns; x++)
            {
                if(y < Rows)
                    if (boolGrid[x, y])
                        return false;
            }
            return true;
        }

        public void AddAll(GameObject obj, bool booltemp, Color color, int x, int y)
        {
            grid[x, y] = obj;
            boolGrid[x, y] = booltemp;
            colorGrid[x, y] = color;
        }

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

        public bool[,] ObjectsBool
        {
            get
            {
                return boolGrid;
            }
        }

        public void AddColor(Color color, int x, int y)
        {
            colorGrid[x, y] = color;
        }

        public Color GetColor(int x, int y)
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
        }

        public Color[,] ObjectsColor
        {
            get
            {
                return colorGrid;
            }
        }

        public int ObjCounter
        {
            get
            {
                return objCounter;
            }
        }


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int x = 0; x < Columns; x++)
            {
                for (int y = 0; y < Rows; y++) 
                {
                    if (boolGrid[x,y])
                    {
                        if (y > 1)
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
            level = (totalRemovedRows / 10) + 1;

            //Debug.Print(timer.ToString());
            if (timerstarted)
            {
                timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            CheckRemoveRow();

            objCounter = 0;
            foreach (GameObject obj in grid)
            {
                if(obj!= null)
                    objCounter++;
            }
            base.Update(gameTime);
        }
    }
}
