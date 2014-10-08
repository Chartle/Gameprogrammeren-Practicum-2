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
        protected Vector2 center;
        protected Color color;
        protected float maxMoveTime, moveTime;
        protected TetrisGrid parentGrid;
        protected GameObjectList state;
        protected int currX, currY;

        public Piece(int columns, int rows, bool isNextPiece, string id = "", string assetname = "sprites/block")
            : base(assetname, 0, id)
        {
            this.isNextPiece = isNextPiece;
            pieceArray = new bool[columns, rows];
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
                    center = new Vector2(1, 1);
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
                    center = new Vector2(0, 0);
                    color = Color.Pink;
                    break;

                case PieceType.L:
                    for (int y = 0; y < 3; y++)
                    {
                        pieceArray[1, y] = true;
                    }
                    center = new Vector2(1, 2);
                    pieceArray[2, 2] = true;
                    color = Color.Purple;
                    break;

                case PieceType.LMirror:
                    for (int y = 0; y < 3; y++)
                    {
                        pieceArray[1, y] = true;
                    }
                    center = new Vector2(1, 1);
                    pieceArray[0, 2] = true;
                    color = Color.Red;
                    break;

                case PieceType.T:
                    for (int x = 0; x < 3; x++)
                        pieceArray[x, 1] = true;
                    pieceArray[1, 2] = true;
                    center = new Vector2(0, 2);
                    color = Color.SaddleBrown;
                    break;

                case PieceType.Z:
                    for (int x = 0; x < 2; x++)
                    {
                        pieceArray[x, 0] = true;
                        pieceArray[x + 1, 1] = true;
                    }
                    center = new Vector2(0, 2);
                    color = Color.SeaGreen;
                    break;

                case PieceType.ZMirror:
                    for (int x = 1; x < 3; x++)
                    {
                        pieceArray[x, 0] = true;
                        pieceArray[x - 1, 1] = true;
                    }
                    center = new Vector2(1, 2);
                    color = Color.Silver;
                    center = new Vector2(0, 1);
                    break;
            }
        }

        public override void Update(GameTime gameTime)
        {
            moveTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            hasUpdated = false;
            base.Update(gameTime);
        }

        public void Turn(string dir)
        {
            //todo turn around point
            bool[,] tempArray = new bool[Rows, Columns];
            for (int i = 0; i < tempArray.GetLength(0)-1; i++)
            {
                for(int j = 0; j < tempArray.GetLength(1)-1; i++)
                {
                    tempArray[i, j] = false;
                }
            }
            switch (dir)
            {
                case "left":
                    for (int x = 0; x < Columns; x++)
                    {
                        for (int y = 0; y < Rows; y++)
                        {
                            tempArray[y, x] = pieceArray[x, y];
                        }
                    }
                        // TODO turn around center
                    break;

                        case "right":
                            //turn right
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
                                if(parentGrid.GetBool(currX+x,currY+y+1))
                                {
                                    return false;
                                }
                            }
                        }
                    }
                    return position.Y / parentGrid.CellHeight < parentGrid.Rows - Rows;

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
                            }
                        }
                    }
                    return position.X / parentGrid.CellWidth > 0;

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
                            }
                        }
                    }
                    return position.X / parentGrid.CellWidth < parentGrid.Columns - Columns;

                default:
                    return false;
            }
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            state = Tetris.GameStateManager.GetGameState("onePlayerState") as GameObjectList;
            parentGrid = state.Find("pieceGrid") as TetrisGrid;
            
            if(inputHelper.IsKeyDown(Keys.Down))
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

                if (inputHelper.KeyPressed(Keys.Up))
                {
                //    Turn("left");
                }

                int moveX = 0, moveY = 0;
                if (inputHelper.KeyPressed(Keys.Left) && !hasUpdated)
                {
                    if (CanMove("left"))
                    {
                        moveX--;
                    }
                }
                if (inputHelper.KeyPressed(Keys.Right) && !hasUpdated)
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
                    }
                }

                if (moveX != 0 || moveY != 0)
                    Move(moveX, moveY, true);
                hasUpdated = true;
            }
        }

        public void Stop()
        {
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
            parentGrid.Add(null, currX, currY);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for(int x = 0; x < pieceArray.GetLength(0); x++)
            {
                for (int y = 0; y < pieceArray.GetLength(1); y++)
                {
                    if(pieceArray[x,y])
                    {
                        sprite.Draw(spriteBatch, GlobalPosition + new Vector2(30*x, 30*y), center, color);
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
