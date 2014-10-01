using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Practicum2.gameobjects;

namespace Practicum2.states
{
    class OnePlayerState : GameObjectList
    {
        GameObjectGrid pieceGrid;
        SpriteGameObject bgGrid;
        Piece testPiece;
        TextGameObject debugText;

        public OnePlayerState()
        {
            testPiece = new Piece(PieceType.Straight);
            testPiece.MaxMoveTime = 5000;

            pieceGrid = new GameObjectGrid(12, 20, 0, "pieceGrid");
            pieceGrid.CellWidth = 30;
            pieceGrid.CellHeight = 30;
            pieceGrid.Position = new Vector2(60, 60);
            this.Add(pieceGrid); 
            
            for (int i = 0; i < pieceGrid.Columns; i++)
            {
                for (int j = 0; j < pieceGrid.Rows; j++)
                {
                    bgGrid = new SpriteGameObject("sprites/block",-100);
                    bgGrid.Position = pieceGrid.Position + new Vector2(pieceGrid.CellWidth * i, pieceGrid.CellHeight * j);
                    this.Add(bgGrid);
                }
            }
            pieceGrid.Add(testPiece, 0, 0);

            debugText = new TextGameObject("fonts/MainMenuFont");
            this.Add(debugText);
        }

        public override void Update(GameTime gameTime)
        {
            debugText.Text = "" + testPiece.MaxMoveTime;
        }
    }
}
