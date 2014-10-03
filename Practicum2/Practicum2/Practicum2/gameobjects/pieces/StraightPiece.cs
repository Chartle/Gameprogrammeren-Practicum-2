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
       
        public StraightPiece(bool isNextPiece, int columns = 1, int rows = 4, string id = "", string assetname = "sprites/block") : base(columns, rows, isNextPiece, id, assetname)
        {

            for (int y = 0; y < 4; y++)
                pieceArray[0, y] = true;
            center = new Vector2(1, 1);
            color = Color.Blue;
        }
    }
}
