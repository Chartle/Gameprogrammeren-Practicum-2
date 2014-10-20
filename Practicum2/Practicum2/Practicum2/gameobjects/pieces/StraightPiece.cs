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
    class StraightPiece : Piece
    {  
        public StraightPiece(bool isNextPiece, string id = "", int size = 4, string assetname = "sprites/block") : base(isNextPiece, id, size)
        {
            for (int y = 0; y < 4; y++)
                pieceArray[1, y] = true;

            color = Color.Cyan;
            pieceType = PieceType.Straight;
        }
    }
}
