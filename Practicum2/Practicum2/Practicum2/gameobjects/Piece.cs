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
        protected bool[,] pieceArray;
        protected Color color;
        protected float maxMoveTime, moveTime;
        protected TetrisGrid parentGrid;
        protected GameObjectList state;
        protected int currX, currY;

        public Piece(bool isNextPiece, string id = "", int size = 0, string assetname = "sprites/block")
            : base(assetname, 0, id)
        {
            this.isNextPiece = isNextPiece;
            pieceArray = new bool[size, size];
            if (isNextPiece)
            {
                position = new Vector2(Tetris.Screen.X - 120, 120);
            }
        }

        public override void Update(GameTime gameTime)
        {
            moveTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            hasUpdated = false;
            base.Update(gameTime);
        }

        public bool[,] Rotate(string dir)
        {
            bool[,] tempArray = new bool[Columns, Rows];
           

            if(pieceType == PieceType.Block)
            {
                return pieceArray;
            }

            switch (dir)
            {
                case "left":
                    for (int x = 0; x < Columns; x++)
                    {
                        for (int y = 0; y < Rows; y++)
                        {
                            Debug.Print("adding bool to new array");
                            tempArray[y, Columns - 1 - x] = pieceArray[x, y];
                        }
                    }
                    Debug.Print("Rotated left");
                    break;

                case "right":
                    for (int x = 0; x < Columns; x++)
                    {
                        for (int y = 0; y < Rows; y++)
                        {
                            Debug.Print("adding bool to new array: " + tempArray[Rows - 1 - y, x] + " becomes " + pieceArray[x, y]);
                            tempArray[Rows - 1 - y, x] = pieceArray[x, y];
                        }
                    }
                    Debug.Print("Rotated right");
                    break;
            }
            Debug.Print("" + pieceArray);
            for (int x = 0; x < Columns; x++)
            {
                for (int y = 0; y < Rows; y++)
                {
                    Debug.Print("" + tempArray[x,y]);
                   
                }
            }
            Debug.Print("" + tempArray);
            return tempArray;

            //Vector2 moveVector = OutsideGrid();
        }

        protected Vector2 OutsideGrid()
        {
            Vector2 returnVector = Vector2.Zero;
            for (int x = 0; x < Columns; x++)
            {
                for(int y = 0; y < Rows; y++)
                {
                    if(pieceArray[x,y])
                    {
                        if(currX + x > parentGrid.Columns - 2)
                        {
                            returnVector.X = -1;
                        }
                        
                        else if(currX + x < 2)
                        {
                            returnVector.X = 1;
                        }

                        if(currY + y > parentGrid.Rows - 2)
                        {
                            returnVector.Y = -1;
                        }
                    }
                }
            }
            return returnVector;
        }

        public void Move(int newX, int newY, bool relative)
        {
            if (relative)
            {
                parentGrid.Add(this, currX + newX, currY + newY);
                Debug.Print("Moving relatively to: " + (currX + newX) + ", " + (currY + newY));
            }
            else
            {
                parentGrid.Add(this, newX, newY);
                Debug.Print("Moving to: " + newX + ", " + newY);
            }
            parentGrid.Add(null, currX, currY);
        }
        
        public bool CanMove(int moveX, int moveY, bool relative)
        {
            if(relative)
                for (int x = 0; x < Columns; x++ )
                {
                    for(int y = 0; y < Rows; y++)
                    {
                        if(pieceArray[x,y])
                        {
                            if (parentGrid.GetBool(currX + x + moveX, currY + y + moveY)) 
                            {
                                return false;
                            }
                            if (currX + x + moveX < 2 || currX + x + moveX > parentGrid.Columns - 3 || currY + y + moveY > parentGrid.Rows - 1) 
                            {
                                return false;
                            }
                        }
                    }
                }
            else
            {
                for (int x = 0; x < Columns; x++)
                {
                    for (int y = 0; y < Rows; y++)
                    {
                        if (pieceArray[x, y])
                        {
                            if (parentGrid.GetBool(x + moveX, y + moveY))
                            {
                                return false;
                            }
                            if (x + moveX < 2 || x + moveX > parentGrid.Columns - 3 || y + moveY > parentGrid.Rows - 1)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            state = Tetris.GameStateManager.GetGameState("onePlayerState") as GameObjectList;
            parentGrid = state.Find("pieceGrid") as TetrisGrid;
            bool[,] tempArray = new bool[Columns, Rows];
           
            Array.Copy(pieceArray, 0, tempArray, 0, pieceArray.Length);
            
            if (inputHelper.IsKeyDown(Keys.S))
            {
                maxMoveTime = 0.05f;
                if (moveTime > 0.05f)
                    moveTime = 0.05f;
            }
            else
            {
                maxMoveTime = 1f;
            }
            currX = (int)(position.X / parentGrid.CellWidth);
            currY = (int)(position.Y / parentGrid.CellHeight);

            if (!isNextPiece)
            {
                int moveX = 0, moveY = 0;
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
                    tempArray = Rotate("left");
                    //Debug.Print("Rotated left, leftest block is " + LeftestBlock + ", rightest block is " + RightestBlock);
                }

                if (inputHelper.KeyPressed(Keys.E))
                {
                    tempArray = Rotate("right");
                }

                if (moveX != 0 || moveY != 0)
                {
                    Array.Copy(tempArray, 0, pieceArray, 0, pieceArray.Length);

                    int newX = currX + moveX;
                    newX = (int)MathHelper.Clamp(newX, 2 - LeftestBlock, parentGrid.Columns - (3 + RightestBlock));
                    int newY = currY + moveY;
                    newY = (int)MathHelper.Clamp(newY, 0, parentGrid.Rows - 1);
                    Debug.Print("newx: " + newX + ", newY: " + newY);

                    if (CanMove(newX, newY, false))
                    {
                        Debug.Print("moving to " + newX + ", " + newY);
                        Move(newX, newY, false); 
                    }
                    else
                    {
                        Debug.Print("stopping block, because newx is " + newX + " and newy is " + newY);
                        Stop();
                    }
                }
                hasUpdated = true;
            }
        }

        
        public void Stop()
        {
            parentGrid.Add(null, currX, currY);
            for (int x = 0; x < Columns; x++)
            {
                for (int y = 0; y < Rows; y++)
                {
                    if (pieceArray[x, y])
                    {
                        parentGrid.AddBool(true, currX + x, currY + y);
                        parentGrid.AddColor(color, currX + x, currY + y);
                    }
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for(int x = 0; x < pieceArray.GetLength(0); x++)
            {
                for (int y = 0; y < pieceArray.GetLength(1); y++)
                {
                    if(pieceArray[x,y])
                    {
                        if (currY + y > 1)
                        {
                            sprite.Draw(spriteBatch, GlobalPosition + new Vector2(30 * x, 30 * y), Vector2.Zero, color);
                        }
                    }
                }
            }   
        }

        public override void Reset()
        {
            //Array tempArray = Enum.GetValues(typeof(PieceType));
            //pieceType = (PieceType)tempArray.GetValue(Tetris.Random.Next(tempArray.Length));
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

        private int Rows
        {
            get { return pieceArray.GetLength(1); }
        }

        private int Columns
        {
            get { return pieceArray.GetLength(0); }
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
                        if (pieceArray[x, y])
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
                        if (pieceArray[x, y])
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
                        if (pieceArray[x, y])
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
                        if (pieceArray[x, y])
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
