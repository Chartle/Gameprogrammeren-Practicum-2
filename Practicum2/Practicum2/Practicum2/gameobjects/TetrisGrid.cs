using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Practicum2.gameobjects
{
    class TetrisGrid : GameObjectGrid
    {
        protected bool[,] boolGrid;
        protected Color[,] colorGrid;
        SpriteGameObject block;
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

        public Color[,] ObjectsColor
        {
            get
            {
                return colorGrid;
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
                        block = new SpriteGameObject("sprites/block", 0);
                        block.Sprite.Draw(spriteBatch, position + new Vector2(x * 30, y * 30), Vector2.Zero, colorGrid[x, y]);
                    }
                }
            }

                /*foreach (GameObject obj in grid)
            {
                if(obj != null)
                    obj.Draw(gameTime, spriteBatch);
            } TODO */
                base.Draw(gameTime, spriteBatch);
        }
    }
}
