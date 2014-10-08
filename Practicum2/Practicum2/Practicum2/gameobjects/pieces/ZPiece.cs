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
    class ZPiece : Piece
    {
        public ZPiece(bool isNextPiece, string id = "", int size = 3, string assetname = "sprites/block"): base(isNextPiece, id, size)
        {
            for (int x = 0; x < 2; x++)
            {
                pieceArray[x, 0] = true;
                pieceArray[x + 1, 1] = true;
            }

            color = Color.Red;
            pieceType = PieceType.Z;
        }
    }
}
