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
    class LMirrorPiece : Piece
    {
        public LMirrorPiece(bool isNextPiece, string id = "", int size = 3, string assetname = "sprites/block") : base(isNextPiece, size, id)
        {
            for (int y = 0; y < 3; y++)
            {
                pieceGrid[1, y] = true;
            }
            pieceGrid[0, 2] = true;

            color = Color.Blue;
            pieceType = PieceType.LMirror;
        }
    }
}
