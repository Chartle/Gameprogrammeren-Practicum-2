using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Practicum2.gameobjects
{
    class Piece : GameObjectGrid
    {
        PieceType pieceType;
        MiniPiece miniPiece1, miniPiece2;
        public Piece(PieceType pieceType) : base(4,4)
        {
            this.pieceType = pieceType;
            setType(this.pieceType);

            miniPiece1 = new MiniPiece(Color.Blue, "minipiece0");
            miniPiece2 = new MiniPiece(Color.Red, "minipiece1");
            this.Add(miniPiece1, 5, 2);
            this.Add(miniPiece2, 2, 4);
        }

        protected void setType(PieceType pieceType)
        {
            //char[,] miniPiecesC = char[4,4];
            /*switch(pieceType)
            {
                case PieceType.Straight:
                    for (int i = 0; i < 4; i++)
                    {
                        miniPiece = new MiniPiece(Color.Blue, "Minipiece" + i);
                        Debug.Print("putting minipiece on 0," + i);
                        this.Add(miniPiece, 0, i);
                    }*/
                    //break;
            //}
        }
    }
}
