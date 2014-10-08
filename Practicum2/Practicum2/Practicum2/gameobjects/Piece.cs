using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Diagnostics;
using Microsoft.Xna.Framework.Input;

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
            if(isNextPiece)
            {
                position = new Vector2(Tetris.Screen.X - 120, 120);
            }
        }

        protected void setType(PieceType pieceType)
        {
            switch (pieceType)
            {
                case PieceType.Straight:
                    for (int y = 0; y < 4; y++)
                            pieceArray[1, y] = true;
                    color = Color.Blue;
                    break;

                case PieceType.Block:
                    for (int x = 0; x < 2; x++)
                    {
                        for (int y = 0; y < 2; y++)
                        {
                            pieceArray[x, y] = true;
                        }
                    }
                    color = Color.Pink;
                    break;

                case PieceType.L:
                    for (int y = 0; y < 3; y++)
                    {
                        pieceArray[1, y] = true;
                    }
                    pieceArray[2, 2] = true;
                    color = Color.Purple;
                    break;

                case PieceType.LMirror:
                    for (int y = 0; y < 3; y++)
                    {
                        pieceArray[1, y] = true;
                    }
                    pieceArray[0, 2] = true;
                    color = Color.Red;
                    break;

                case PieceType.T:
                    for (int x = 0; x < 3; x++)
                        pieceArray[x, 1] = true;
                    pieceArray[1, 2] = true;
                    color = Color.SaddleBrown;
                    break;

                case PieceType.Z:
                    for (int x = 0; x < 2; x++)
                    {
                        pieceArray[x, 0] = true;
                        pieceArray[x + 1, 1] = true;
                    }
                    color = Color.SeaGreen;
                    break;

                case PieceType.ZMirror:
                    for (int x = 1; x < 3; x++)
                    {
                        pieceArray[x, 0] = true;
                        pieceArray[x - 1, 1] = true;
                    }
                    color = Color.Silver;
                    break;
            }
        }

        public override void Update(GameTime gameTime)
        {
            moveTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            hasUpdated = false;
            base.Update(gameTime);
        }

        public void Rotate(string dir)
        {
            //todo turn around point
            bool[,] tempArray = new bool[Columns, Rows];
            if(pieceType == PieceType.Block)
            {
                return;
            }
            switch (dir)
            {
                case "left":
                    for (int x = 0; x < Columns; x++)
                    {
                        for (int y = 0; y < Rows; y++)
                        {
                            tempArray[y, Columns - 1 - x] = pieceArray[x, y];
                        }
                    }
                    break;

                case "right":
                    for (int x = 0; x < Columns; x++)
                    {
                        for (int y = 0; y < Rows; y++)
                        {
                            tempArray[Rows - 1 - y, x] = pieceArray[x, y];
                        }
                    }   
                    break;
            }
            pieceArray = tempArray;
        }

        public void Move(int newX, int newY, bool relative)
        {
            int x = (int)(position.X / parentGrid.CellWidth);
            int y = (int)(position.Y / parentGrid.CellHeight);

            if (relative)
            {
                parentGrid.Add(this, x + newX, y + newY);
                Debug.Print("Moving relatively to: " + (x + newX) + ", " + (y + newY));
            }
            else
            {
                parentGrid.Add(this, newX, newY);
                Debug.Print("Moving to: " + newX + ", " + y);
            }
            parentGrid.Add(null, x, y);
        }
        
        public bool CanMove(string direction)
        {
            switch (direction)
            {
                case "down":
                    for (int x = 0; x < Columns; x++ )
                    {
                        for(int y = Rows-1; y >= 0; y--)
                        {
                            if(pieceArray[x,y])
                            {
                                if (parentGrid.GetBool(currX + x, currY + y + 1)) 
                                {
                                    return false;
                                }
                                if (currY + y + 1 >= parentGrid.Rows) 
                                {
                                    return false;
                                }
                            }
                        }
                    }
                    return true;

                case "left":
                    for (int y = 0; y < Rows; y++)
                    {
                        for (int x = 0; x < Columns; x++)
                        {
                            if (pieceArray[x, y])
                            {
                                if (parentGrid.GetBool(currX + x -1, currY + y))
                                {
                                    return false;
                                }
                                if(currX + x - 1 < 2)
                                {
                                    return false;
                                }
                            }
                        }
                    }
                    return true;

                case "right":
                    for (int y = 0; y < Rows; y++)
                    {
                        for (int x = Columns - 1; x >= 0; x--)
                        {
                            if (pieceArray[x, y])
                            {
                                if (parentGrid.GetBool(currX + x + 1, currY + y))
                                {
                                    return false;
                                }
                                if (currX + x + 1 >= parentGrid.Columns - 2) 
                                {
                                    return false;
                                }
                            }
                        }
                    }
                    return true;
                default:
                    return false;
            }
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            state = Tetris.GameStateManager.GetGameState("onePlayerState") as GameObjectList;
            parentGrid = state.Find("pieceGrid") as TetrisGrid;
            
            if(inputHelper.IsKeyDown(Keys.S))
            {
                maxMoveTime = 0.05f;
                if(moveTime > 0.05f)
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

                if (inputHelper.KeyPressed(Keys.Q))
                {
                    Rotate("left");
                }

                if(inputHelper.KeyPressed(Keys.E))
                {
                    Rotate("right");
                }

                int moveX = 0, moveY = 0;
                if (inputHelper.KeyPressed(Keys.A) && !hasUpdated)
                {
                    if (CanMove("left"))
                    {
                        moveX--;
                    }
                }
                if (inputHelper.KeyPressed(Keys.D) && !hasUpdated)
                {
                    if (CanMove("right"))
                    {
                        moveX++;
                    }
                }

                if (moveTime < 0)
                {
                    moveTime = maxMoveTime;
                    if (CanMove("down"))
                    {
                        moveY++;
                    }
                    else
                    {
                        Stop();
                        return;
                    }
                }

                if (moveX != 0 || moveY != 0)
                    Move(moveX, moveY, true);
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
            base.Reset();
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
    }
}
