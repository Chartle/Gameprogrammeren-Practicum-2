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
    class TPiece : Piece
    {
       
        public TPiece(bool isNextPiece, int columns = 3, int rows = 2, string id = "", string assetname = "sprites/block") : base(columns, rows, isNextPiece, id, assetname)
        {
            for (int x = 0; x < 3; x++)
                pieceArray[x, 0] = true;
            pieceArray[1, 1] = true;

            center = new Vector2(1, 1);
            color = Color.Red;
            pieceType = PieceType.T;
        }
    }
}
