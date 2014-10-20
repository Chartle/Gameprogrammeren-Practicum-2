using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Practicum2.gameobjects;
using Practicum2.gameobjects.pieces;
using Microsoft.Xna.Framework.Input;
namespace Practicum2.states
{
    class OnePlayerState : PlayingState
    {
        TetrisGrid pieceGrid;
        SpriteGameObject bgGrid, topOverlay;
        Piece piece1, piece2;
        TextGameObject pausedText, scoreText, levelText, nextPieceText;
        bool paused = false;

        public OnePlayerState()
        {
            // Initialize and add the pieces, piece grid and create the background
            piece1 = new StraightPiece(false, "piece1");
            piece1.MaxMoveTime = 1f;
            
            piece2 = new StraightPiece(true, "piece2");
            piece2.MaxMoveTime = 1f;
            
            piece2.PieceType = piece2.RandomPiece();

            pieceGrid = new TetrisGrid(16, 25, 0, "pieceGrid");
            pieceGrid.CellWidth = 30;
            pieceGrid.CellHeight = 30;
            pieceGrid.Position = new Vector2(0, -90);

            for (int i = 2; i < pieceGrid.Columns - 2; i++) 
            {
                for (int j = 5; j < pieceGrid.Rows; j++)
                {
                    bgGrid = new SpriteGameObject("sprites/block",-1000);
                    bgGrid.Position = pieceGrid.Position + new Vector2(pieceGrid.CellWidth * i, pieceGrid.CellHeight * j);
                    this.Add(bgGrid);
                }
            }

            // TopOverlay is a white sprite that covers the part of the piece that is above the playing field

            topOverlay = new SpriteGameObject("sprites/topOverlay", 100, "topOverlay");
            topOverlay.Position = new Vector2(30, 0);

            pausedText = new TextGameObject("fonts/MainMenuFont");
            pausedText.Visible = paused;
            pausedText.Text = "Paused";
            pausedText.Position = new Vector2(Tetris.Screen.X * 2 / 3 + 50, Tetris.Screen.Y / 2);

            scoreText = new TextGameObject("fonts/SpelFont", 10);
            scoreText.Position = new Vector2(Tetris.Screen.X * 4 / 5 - 40, Tetris.Screen.Y / 5 * 3);

            levelText = new TextGameObject("fonts/SpelFont", 10);
            levelText.Position = new Vector2(Tetris.Screen.X * 4 / 5 - 40, Tetris.Screen.Y / 5 * 3 + 30);

            nextPieceText = new TextGameObject("fonts/SpelFont", 10);
            nextPieceText.Position = new Vector2(Tetris.Screen.X * 4 / 5 - 40, Tetris.Screen.Y / 12);
            nextPieceText.Text = "Next piece: ";

            this.Add(pieceGrid);
            this.Add(piece2);
            this.Add(topOverlay);
            this.Add(pausedText);
            this.Add(scoreText);
            this.Add(levelText);
            this.Add(nextPieceText);
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            // If the P key is pressed, pause the game
            if (inputHelper.KeyPressed(Keys.P))
            {
                paused = !paused;
                pausedText.Visible = paused;
            }
            base.HandleInput(inputHelper);
        }

        public override void Update(GameTime gameTime)
        {
            pausedText.Visible = paused;
            if (!paused)
            {
                scoreText.Text = "Score: " + pieceGrid.Score;
                levelText.Text = "Level: " + pieceGrid.Level;

                // Update the maximum move time according to the level
                piece1.MaxMoveTime = 0.55f - ((float)pieceGrid.Level / 10.0f) * 0.5f;
                if (piece1.MaxMoveTime < 0.05f)
                    piece1.MaxMoveTime = 0.05f;

                base.Update(gameTime);

                // When there are no objects on the field, add a piece with the same piece type as the 'next piece', and randomize the piece type of the 'next piece'
                if (pieceGrid.ObjCounter == 0)
                {
                    this.Remove(piece2);
                    PieceType newPieceType = piece2.PieceType;
                    piece2.PieceType = piece2.RandomPiece();

                    // these switches don't work in a method
                    switch (piece2.PieceType)
                    {
                        case PieceType.Straight:
                            piece2 = new StraightPiece(true, "piece2");
                            break;

                        case PieceType.T:
                            piece2 = new TPiece(true, "piece2");
                            break;

                        case PieceType.Block:
                            piece2 = new BlockPiece(true, "piece2");
                            break;

                        case PieceType.L:
                            piece2 = new LPiece(true, "piece2");
                            break;

                        case PieceType.LMirror:
                            piece2 = new LMirrorPiece(true, "piece2");
                            break;

                        case PieceType.Z:
                            piece2 = new ZPiece(true, "piece2");
                            break;

                        case PieceType.ZMirror:
                            piece2 = new ZMirrorPiece(true, "piece2");
                            break;

                        default:
                            piece2 = null;
                            break;
                    }

                    switch (newPieceType)
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
                    this.Add(piece2);

                    // Checks if there's a piece at the top of the field
                    bool canAddPiece = true;
                    for (int x = 0; x < piece1.Columns; x++ )
                    {
                        for(int y = 0; y < piece1.Rows; y++)
                        {
                            if(pieceGrid.GetBool(x + 6,y + 3 - piece1.LowestBlock))
                            {
                                canAddPiece = false;
                            }
                        }
                    }
                    
                    if(canAddPiece)
                    {
                        pieceGrid.Add(piece1, 6, 3 - piece1.LowestBlock);
                    }
                    // If a piece cannot be placed, go to the game over menu, play the game over song, and reset everything in this state.
                    else
                    {
                        Tetris.GameStateManager.SwitchTo("gameOverState");
                        Tetris.AssetManager.PlayMusic("audio/gameOverSong");
                        this.Reset();
                    }
                }
            }
        }

        public override int GetScore()
        {
            return pieceGrid.Score; 
        }

        public override void SetScore(int score)
        {
            pieceGrid.Score = score;
        }
    }
}
