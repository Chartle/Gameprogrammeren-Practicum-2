using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Practicum2.gameobjects;
using Practicum2.gameobjects.pieces;
namespace Practicum2.states
{
    class OnePlayerState : GameObjectList
    {
        TetrisGrid pieceGrid;
        SpriteGameObject bgGrid;
        Piece piece1, piece2;
        TextGameObject debugText;

        public OnePlayerState()
        {
            piece1 = new LPiece(false, "piece1");
            piece1.MaxMoveTime = 1f;

            piece2 = new StraightPiece(true, "piece2");
            piece2.MaxMoveTime = 1f;
            
            pieceGrid = new TetrisGrid(16, 22, 0, "pieceGrid");
            pieceGrid.CellWidth = 30;
            pieceGrid.CellHeight = 30;
            pieceGrid.Position = new Vector2(0, 0);
            this.Add(pieceGrid);

            for (int i = 2; i < pieceGrid.Columns - 2; i++) 
            {
                for (int j = 2; j < pieceGrid.Rows; j++)
                {
                    bgGrid = new SpriteGameObject("sprites/block",-100);
                    bgGrid.Position = pieceGrid.Position + new Vector2(pieceGrid.CellWidth * i, pieceGrid.CellHeight * j);
                    this.Add(bgGrid);
                }
            }

            pieceGrid.Add(piece1, 8, 0);
            this.Add(piece2);
            debugText = new TextGameObject("fonts/MainMenuFont");
            this.Add(debugText);
        }

        public override void Update(GameTime gameTime)
        {
            debugText.Text = "" + piece1.MoveTime;
            
            base.Update(gameTime);

            if(pieceGrid.ObjCounter == 0)
            {
                piece2.PieceType = piece2.RandomPiece();

                PieceType newPieceType = piece2.PieceType;
                switch(newPieceType)
                {
                    case PieceType.Straight:
                        piece1 = new StraightPiece(false, "piece1");
                        break;
                        
                    case PieceType.T:
                        piece1 = new TPiece(false, "piece1");
                        break;

                    case PieceType.Block:
                        piece1 = new BlockPiece(false, "piece1");
                        break;

                    case PieceType.L:
                        piece1 = new LPiece(false, "piece1");
                        break;

                    case PieceType.LMirror:
                        piece1 = new LMirrorPiece(false, "piece1");
                        break;

                    case PieceType.Z:
                        piece1 = new ZPiece(false, "piece1");
                        break;

                    case PieceType.ZMirror:
                        piece1 = new ZMirrorPiece(false, "piece1");
                        break;

                    default:
                        piece1 = null;
                        break;
                }
                
                pieceGrid.Add(piece1, 6, 0);
            }
        }
    }
}
