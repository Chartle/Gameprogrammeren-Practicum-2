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
    class ZMirrorPiece : Piece
    {
        public ZMirrorPiece(bool isNextPiece, string id = "", int size = 3, string assetname = "sprites/block") : base(isNextPiece, size, id)
        {
            //false, true, true
            //true, true, false
            //false, false, false

            for (int x = 1; x < 3; x++)
            {
                pieceGrid[x, 0] = true;
                pieceGrid[x - 1, 1] = true;
            }

            color = Color.Green;
            pieceType = PieceType.ZMirror;
        }
    }
}
