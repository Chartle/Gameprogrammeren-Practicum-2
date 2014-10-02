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
        PieceType pieceType;
        bool[,] pieceArray = new bool[4,4];
        Vector2 center;
        Color color;
        float maxMoveTime, moveTime;
        GameObjectGrid parentGrid;

        public Piece(PieceType pieceType, string assetname = "sprites/block")
            : base(assetname)
        {
            this.pieceType = pieceType;

            for (int i = 0; i < 4; i++ )
            {
                for(int j = 0; j < 4; j++)
                {
                    pieceArray[i, j] = false;
                }
            }
            setType(this.pieceType);
        }

        protected void setType(PieceType pieceType)
        {
            switch(pieceType)
            {
                case PieceType.Straight:
                    for (int i = 0; i < 3; i++) 
                    {
                        pieceArray[0, i] = true;
                    }
                    color = Color.Blue;
                    center = new Vector2(0, 1);
                    break;
            }

        }

        public override void Update(GameTime gameTime)
        {
            GameObjectList state = Tetris.GameStateManager.GetGameState("onePlayerState") as GameObjectList;
            parentGrid = state.Find("pieceGrid") as GameObjectGrid;
            moveTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            hasUpdated = false;
            base.Update(gameTime);
        }

        protected void Move(int newX, int newY, bool relative)
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

        private int ActualWidth
        {
            get
            {
                for (int i = Columns - 1; i >= 0; i--) 
                {
                    for(int j = 0 ; j < Rows ; j++)
                    {
                        if(pieceArray[i,j])
                        {
                            return i-1;
                        }
                    }
                }
                return -1;
            }
        }

        private int ActualHeight
        {
            get
            {
                for (int i = Rows - 1; i >= 0; i--)
                {
                    for (int j = 0; j < Columns; j++)
                    {
                        if (pieceArray[j, i])
                        {
                            return i+1;
                        }
                    }
                }
                return -1;
            }
        }

        private bool CanMove(string direction)
        {
            switch (direction)
            {
                case "down":
                    return position.Y / parentGrid.CellHeight < parentGrid.Rows - ActualHeight;

                case "left":
                    return position.X / parentGrid.CellWidth > 0;

                case "right":
                    return position.X / parentGrid.CellWidth < parentGrid.Columns + ActualWidth;

                default:
                    return false;
            }
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            //if (!hasUpdated)
            {
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

                    }
                }

                if (moveX != 0 || moveY != 0)
                    Move(moveX, moveY, true);

            }
            hasUpdated = true;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for(int x = 0; x < pieceArray.GetLength(0); x++)
            {
                for (int y = 0; y < pieceArray.GetLength(1); y++)
                {
                    if(pieceArray[x,y])
                    {
                        sprite.Draw(spriteBatch, GlobalPosition + new Vector2(30*x, 30*y), origin, color);
                    }
                }
            }
            
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
    }
}
