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
    class BlockPiece : Piece
    {
        public BlockPiece(bool isNextPiece, string id = "", int size = 4, string assetname = "sprites/block") : base(isNextPiece, size, id)
        {
            for (int x = 0; x < 2; x++)
            {
                for (int y = 0; y < 2; y++)
                {
                    pieceGrid[x, y] = true;
                }
            }
            color = Color.Yellow;
            pieceType = PieceType.Block;
        }
    }
}
