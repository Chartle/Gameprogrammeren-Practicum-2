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
        public ZMirrorPiece(bool isNextPiece, string id = "", int size = 3, string assetname = "sprites/block")
            : base(isNextPiece, id, size)
        {
            for (int x = 1; x < 3; x++)
            {
                pieceArray[x, 0] = true;
                pieceArray[x - 1, 1] = true;
            }
            pieceArray[0, 2] = true;

            color = Color.Green;
            pieceType = PieceType.ZMirror;
        }
    }
}
