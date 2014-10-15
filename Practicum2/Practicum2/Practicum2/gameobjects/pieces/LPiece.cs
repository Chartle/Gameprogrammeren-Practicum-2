using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Diagnostics;
using Microsoft.Xna.Framework.Input;

namespace Practicum2.gameobjects.pieces
{
    class LPiece : Piece
    {
        public LPiece(bool isNextPiece, string id = "", int size = 3, string assetname = "sprites/block") : base(isNextPiece, size, id)
        {
            for (int y = 0; y < 3; y++)
            {
                pieceGrid[1, y] = true;
            }
            pieceGrid[2, 2] = true;

            color = Color.Orange;
            pieceType = PieceType.L;
        }
    }
}
