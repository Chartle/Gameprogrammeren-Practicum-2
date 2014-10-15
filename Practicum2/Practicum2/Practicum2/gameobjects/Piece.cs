using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System.Diagnostics;

namespace Practicum2.gameobjects
{
    class Piece : SpriteGameObject
    {
        protected PieceType pieceType;
        protected bool isNextPiece;
        protected Color color;
        protected float maxMoveTime, moveTime;
        protected bool[,] pieceGrid;
        protected TetrisGrid parentGrid;
        protected int currX = 0, currY = 0;

        public Piece(bool isNextPiece, int size, string id = "", string assetname = "sprites/block") : base(assetname, 0, id)
        {
            pieceGrid = new bool[size, size];
            this.isNextPiece = isNextPiece;
           
            if(isNextPiece)
            {
                position = new Vector2(Tetris.Screen.X- 120, 120);
            }
        }

        public bool[,] GetBlockedParts(bool[,] grid)
        {
            bool[,] blockedGrid = new bool[Columns, Rows];

            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    if (grid[x, y])
                    {
                        if (parentGrid.GetBool(currX + x, currY + y) || currX + x < 2 || currX + x > parentGrid.Columns - 3)
                        {
                            blockedGrid[x,y] = true;
                        }
                        else
                        {
                            blockedGrid[x,y] = false;
                        }
                    }
                    else
                    {
                        blockedGrid[x,y] = false;
                    }
                }
            }
            return blockedGrid;
        }
        
        public bool CanMove(int moveX, int moveY, bool relative)
        {
            for (int x = 0; x < Columns; x++)
            {
                for (int y = 0; y < Rows; y++)
                {
                    if (pieceGrid[x, y])
                    {
                        if (relative)
                        {
                            if (parentGrid.GetBool(currX + x + moveX, currY + y + moveY))
                            {
                                return false;
                            }
                            if (currX + x + moveX < 3 - LeftestBlock || currX + x + moveX > parentGrid.Columns - (4 - RightestBlock) || currY + y + moveY > parentGrid.Rows - 1)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if (parentGrid.GetBool(x + moveX, y + moveY))
                            {
                                return false;
                            }
                            if (x + moveX < 3 - LeftestBlock || x + moveX > parentGrid.Columns - (4 - RightestBlock) || y + moveY > parentGrid.Rows - 1)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        public bool CanMove(int moveX, int moveY, bool relative, bool[,] grid)
        {
            for(int x = 0; x < grid.GetLength(0); x++)
            {
                for(int y = 0; y < grid.GetLength(1); y++)
                {
                    if(grid[x,y])
                    {
                        if(relative)
                        {

                        }
                    }
                }
            }
            return true;
        }

        public void Move(int newX, int newY)
        {
            parentGrid.Remove(currX, currY);
            parentGrid.Add(this, newX, newY);
        }

        private bool[,] Rotate(string dir)
        {
            bool[,] tempArray = new bool[Columns, Rows];
            int testCounter = 0;

            if (pieceType == PieceType.Block)
            {
                return pieceGrid;
            }

            switch (dir)
            {
                case "left":
                    for (int x = 0; x < Columns; x++)
                    {
                        for (int y = 0; y < Rows; y++)
                        {
                            tempArray[y, Columns - 1 - x] = pieceGrid[x, y];
                            testCounter++;
                        }
                    }
                    break;

                case "right":
                    for (int x = 0; x < Columns; x++)
                    {
                        for (int y = 0; y < Rows; y++)
                        {
                            tempArray[Rows - 1 - y, x] = pieceGrid[x, y];
                            testCounter++;
                        }
                    }
                    break;
            }
            Debug.Print("Rotated " + dir + ", changed " + testCounter + " values");
            return tempArray;
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            GameObjectList state = Tetris.GameStateManager.GetGameState("onePlayerState") as GameObjectList;
            parentGrid = state.Find("pieceGrid") as TetrisGrid;
            bool[,] tempGrid = pieceGrid;

            currX = (int)(position.X / parentGrid.CellWidth);
            currY = (int)(position.Y / parentGrid.CellHeight);
            
            int moveX = 0, moveY = 0;
            if (!isNextPiece)
            {
                if (inputHelper.IsKeyDown(Keys.S))
                {
                    maxMoveTime = 0.05f;
                    if (moveTime > 0.05f)
                    {
                        moveTime = 0.05f;
                    }
                }
                else
                {
                    maxMoveTime = 1f;
                }

                if (inputHelper.KeyPressed(Keys.A) && !hasUpdated)
                {
                    moveX--;
                }

                if (inputHelper.KeyPressed(Keys.D) && !hasUpdated)
                {
                    moveX++;
                }

                if (moveTime < 0)
                {
                    moveTime = maxMoveTime;
                    moveY++;
                }

                if (inputHelper.KeyPressed(Keys.Q))
                {
                    tempGrid = Rotate("left");
                    moveTime = maxMoveTime;
                }

                if (inputHelper.KeyPressed(Keys.E))
                {
                    tempGrid = Rotate("right");
                    moveTime = maxMoveTime;
                }

                int newX = currX + moveX;
                int newY = currY + moveY;

                if (newX != currX || newY != currY)
                {                    
                    Move(newX, newY);    
                }

                bool canMove = CanMoveToClearPosition();

                if (canMove)
                {
                    pieceGrid = tempGrid;
                }
                else
                {
                    Move(currX, currY);
                }
                //newX = (int)MathHelper.Clamp(newX, 2 - LeftestBlock, parentGrid.Columns - 3 - RightestBlock);
                //newY = (int)MathHelper.Clamp(newY, 0, parentGrid.Rows - 1 - LowestBlock);
                //Debug.Print("new pos: " + newX + ", " + newY);
                
            }
            hasUpdated = true;
        }

        private bool CanMoveToClearPosition()
        {
            bool[,] blockedGrid = GetBlockedParts(pieceGrid);
            bool canMove = true;

            int moveX = 0, moveY = 0;

            for (int x = 0; x < Columns; x++)
            {
                for (int y = 0; y < Rows; y++)
                {
                    if (blockedGrid[x, y])
                    {
                        canMove = false;
                        if (x < Columns / 2)
                        {
                            moveX--;
                        }
                        else
                        {
                            moveX++;
                        }
                    }
                }
            }
        }

        private void Stop()
        {
            parentGrid.Remove(currX, currY);
            for (int x = 0; x < Columns; x++)
            {
                for (int y = 0; y < Rows; y++)
                {
                    if (pieceGrid[x, y])
                    {
                        parentGrid.AddBool(true, currX + x, currY + y);
                        parentGrid.AddColor(color, currX + x, currY + y);
                    }
                }
            }
        }


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for(int x = 0; x < Columns; x++)
            {
                for(int y = 0; y < Rows; y++)
                {
                    if(pieceGrid[x,y])
                    {
                        if (currY + y > 4 || isNextPiece)
                        {
                            sprite.Draw(spriteBatch, GlobalPosition + new Vector2(30 * x, 30 * y), Vector2.Zero, color);
                        }
                    }
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            moveTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;


            hasUpdated = false;

            base.Update(gameTime);
        
        }

        public PieceType RandomPiece()
        {
            int nextRandomPieceGetal = Tetris.Random.Next(7);
            switch (nextRandomPieceGetal)
            {
                case 1: this.PieceType = PieceType.Block; break;
                case 2: this.PieceType = PieceType.L; break;
                case 3: this.PieceType = PieceType.LMirror; break;
                case 4: this.PieceType = PieceType.Straight; break;
                case 5: this.PieceType = PieceType.T; break;
                case 6: this.PieceType = PieceType.Z; break;
                case 0: this.PieceType = PieceType.ZMirror; break;
            }
            Debug.Print("RANDOMNUMBER: " + nextRandomPieceGetal);
            return pieceType;
        }

        public int Rows
        {
            get { return pieceGrid.GetLength(1); }
        }

        public int Columns
        {
            get { return pieceGrid.GetLength(0); }
        }

        public PieceType PieceType
        {
            get { return pieceType; }
            set { pieceType = value; }
        }

        public float MaxMoveTime
        {
            set
            {
                maxMoveTime = value;
                moveTime = maxMoveTime;
            }
        }

        public float MoveTime
        {
            get { return moveTime; }
        }

        public bool IsNextPiece
        {
            get { return isNextPiece; }
            set { isNextPiece = value; }
        }

        private int LeftestBlock
        {
            get
            {
                for (int x = 0; x < Columns; x++)
                {
                    for (int y = 0; y < Rows; y++)
                    {
                        if (pieceGrid[x, y])
                        {
                            Debug.Print("Leftest block is " + x);
                            return x;
                        }
                    }
                }
                return -1;
            }
        }

        private int RightestBlock
        {
            get
            {
                for (int x = Columns - 1; x >= 0; x--)
                {
                    for (int y = 0; y < Rows; y++)
                    {
                        if (pieceGrid[x, y])
                        {
                            Debug.Print("Rightest block is " + x);
                            return x;
                        }
                    }
                }
                return -1;
            }
        }

        private int HighestBlock
        {
            get
            {
                for (int y = 0; y < Rows; y++)
                {
                    for (int x = 0; x < Columns; x++)
                    {
                        if (pieceGrid[x, y])
                        {
                            Debug.Print("Highest block is " + y);
                            return y;
                        }
                    }
                }
                return -1;
            }
        }

        private int LowestBlock
        {
            get
            {
                for (int y = Rows - 1; y >= 0; y--)
                {
                    for (int x = 0; x < Columns; x++)
                    {
                        if (pieceGrid[x, y])
                        {
                            Debug.Print("Lowest block is " + y);
                            return y;
                        }
                    }
                }
                return -1;
            }
        }
    }
}
