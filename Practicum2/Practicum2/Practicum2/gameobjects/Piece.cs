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
        protected int currX = 0, currY = 0, nextRandomPieceGetal;
        protected GameObjectList state;
        public float drawY;


        public Piece(bool isNextPiece, int size, string id = "", string assetname = "sprites/block") : base(assetname, 0, id)
        {
            // initializing grid for the piece, and placing the non-moving piece
            pieceGrid = new bool[size, size];
            this.isNextPiece = isNextPiece;
           
            if(isNextPiece)
            {
                position = new Vector2(Tetris.Screen.X- 120, 120);
            }
        }


        public PieceType RandomPiece()
        {
            // Returns a random piece

            nextRandomPieceGetal = Tetris.Random.Next(7);
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

        private bool[,] Rotate(int dir)
        {
            // Returns an array with the rotated values in either direction of the original piece

            bool[,] tempArray = new bool[Columns, Rows];
            int testCounter = 0;

            if (pieceType == PieceType.Block)
            {
                return pieceGrid;
            }

            switch (dir)
            {
                case -1:
                    for (int x = 0; x < Columns; x++)
                    {
                        for (int y = 0; y < Rows; y++)
                        {
                            tempArray[y, Columns - 1 - x] = pieceGrid[x, y];
                            testCounter++;
                        }
                    }
                    break;

                case 1:
                    for (int x = 0; x < Columns; x++)
                    {
                        for (int y = 0; y < Rows; y++)
                        {
                            tempArray[Rows - 1 - y, x] = pieceGrid[x, y];
                            testCounter++;
                        }
                    }
                    break;

                default:
                    break;
            }
            Debug.Print("Rotated " + dir + ", changed " + testCounter + " values");
            
            return tempArray;
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            state = Tetris.GameStateManager.GetGameState("onePlayerState") as GameObjectList;
            parentGrid = state.Find("pieceGrid") as TetrisGrid;
            
            if (!isNextPiece)
            {
                //initializing move handling

                bool[,] tempGrid = new bool[Columns, Rows];

                tempGrid = pieceGrid;
                int rotateDir = 0, moveX = 0, moveY = 0;

                currX = (int)(position.X / parentGrid.CellWidth);
                currY = (int)(position.Y / parentGrid.CellHeight);

                bool autoMoveDown = false;

                // If the player is pressing S, make the piece fall quicker
                if (inputHelper.IsKeyDown(Keys.S))
                {
                    maxMoveTime = 0.05f;
                    if (moveTime > 0.05f)
                    {
                        moveTime = 0.05f;
                    }
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
                    autoMoveDown = true;
                }

                if (inputHelper.KeyPressed(Keys.Q))
                {
                    rotateDir = -1;
                }

                if (inputHelper.KeyPressed(Keys.E))
                {
                    rotateDir = 1;
                }

                if (rotateDir != 0)
                    tempGrid = Rotate(rotateDir);

                int newX = currX + moveX;
                int newY = currY + moveY;

                bool canMoveX = true;
                bool canMoveY = true;
                
                // For each true value in the piece grid, check if it can move in the desired direction

                for (int x = 0; x < Columns; x++)
                {
                    for (int y = 0; y < Rows; y++)
                    {
                        if(tempGrid[x,y])
                        {
                            if (newY + y > parentGrid.Rows - 1 || parentGrid.GetBool(currX + x, newY + y)) 
                            {
                                canMoveY = false;
                            }
                            else if (newX + x < 2 || newX + x > parentGrid.Columns - 3)
                            {
                                canMoveX = false;
                            }
                            else if (parentGrid.GetBool(newX + x, newY + y))
                            {
                                canMoveX = false;
                                canMoveY = false;
                            }


                        }
                    }
                }

                // If the move timer has run out, and the piece cannot move down then place it on the 'landed' grid and remove the piece from the field
                if (autoMoveDown)
                {
                    if (canMoveY)
                    {
                        newX = currX;
                        parentGrid.Remove(currX, currY);
                        parentGrid.Add(this, newX, newY);
                        drawY = -30f;
                    }
                    else
                    {
                        Stop();
                    }
                }
                // If the move timer has not run out, and the piece can be placed in the new position, do so. canMoveX and canMoveY are checked with the temporary grid after the rotation,
                // so if the piece cannot rotate (because it would fall outside the field, or in a landed piece) either of the variables will be false.
                else if (canMoveX && canMoveY)
                {
                    if (newX != currX || newY != currY)
                    {
                        parentGrid.Remove(currX, currY);
                        parentGrid.Add(this, newX, newY);
                    }
                    pieceGrid = tempGrid;
                }

                // If the piece cannot be placed, keep it on the old position and play a "fail" sound.
                else
                {
                    parentGrid.Remove(newX, newY);
                    parentGrid.Add(this, currX, currY);
                    Tetris.AssetManager.PlaySound("audio/fail");
                }
            }
            // hasUpdated is used so that, when it is moving in the grid, the Update method (and consequently the HandleInput method) are not called multiple times in the same cycle
            hasUpdated = true;
        }

        private void Stop()
        {
            // Places the piece on the grid of booleans and the grid of colors.
            Tetris.AssetManager.PlaySound("audio/piecePlaced");

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
            //If the piece is either in the field, or the 'next piece', draw each block on the appropriate position
            //drawY is used for the falling animation

            for(int x = 0; x < Columns; x++)
            {
                for(int y = 0; y < Rows; y++)
                {
                    if(pieceGrid[x,y])
                    {
                        if (currY + y > 4 || isNextPiece)
                        {
                            sprite.Draw(spriteBatch, GlobalPosition + new Vector2(30 * x, 30 * y + drawY), Vector2.Zero, color);
                        }
                    }
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            //Updates the movetime and drawY
            moveTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (!isNextPiece)
            {
                drawY += 30 / maxMoveTime * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            hasUpdated = false;

            base.Update(gameTime);
        
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
            get { return maxMoveTime; }
            set { maxMoveTime = value; }
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

        public int LowestBlock
        {
            // Used to place the piece at the correct position above the grid
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
