using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Practicum2.gameobjects
{
    class Piece : GameObjectGrid
    {
        public Piece(PieceType pieceType) : base(4,4)
        {
            
        }

        protected void setType(PieceType pieceType)
        {
            //char[,] miniPiecesC = char[4,4];
            switch(pieceType)
            {
                case PieceType.Straight:
                    
                    break;
            }
        }
    }
}
