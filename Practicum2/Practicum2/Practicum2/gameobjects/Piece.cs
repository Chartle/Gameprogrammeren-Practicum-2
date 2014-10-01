using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Diagnostics;

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
                    for (int i = 0; i < 4; i++) 
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
            //moveTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (moveTime < 0)
            {
                moveTime = maxMoveTime;
                if(canMoveDown())
                {
                    moveDown();
                }
            }
        }

        protected void moveDown()
        {
            int x = (int)(position.X / parentGrid.CellWidth);
            int y = (int)(position.Y / parentGrid.CellHeight);
            GameObject obj = this as GameObject;
            parentGrid.Add(null, x, y);
            parentGrid.Add(obj, x, y + 1);
            hasUpdated = true;
        }

        private bool canMoveDown()
        {
            GameObjectList state = Tetris.GameStateManager.GetGameState("onePlayerState") as GameObjectList;
            parentGrid = state.Find("pieceGrid") as GameObjectGrid;
            return position.Y / parentGrid.CellHeight < parentGrid.Rows - 1;
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
            set { maxMoveTime = value; }
        }
    }
}
