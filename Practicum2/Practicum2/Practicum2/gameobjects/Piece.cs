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

        public Piece(PieceType pieceType, Color color, string assetname = "sprites/block")
            : base(assetname)
        {
            this.pieceType = pieceType;
            this.color = color;

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
                    break;
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
                        sprite.Draw(spriteBatch, position + new Vector2(30*x, 30*y), origin, color);
                        Debug.Print("drawing sprite on " + x + ", " + y);
                    }
                }
            }
            base.Draw(gameTime, spriteBatch);
        }
    }
}
