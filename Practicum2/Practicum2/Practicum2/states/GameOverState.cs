using System.IO;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Practicum2.gameobjects;


namespace Practicum2.states
{
    class GameOverState : GameObjectList
    {
        TextGameObject gameOverText, scoreText, gotoText;
        PlayingState previouslyPlayedState;
        
        int score;

        public GameOverState()
        {
            // initialize and add all the text

            gameOverText = new TextGameObject("fonts/MainMenuFont");
            gameOverText.Text = "GAME OVER";
            gameOverText.Position = new Vector2(Tetris.Screen.X / 2 - (gameOverText.Size.X/2), Tetris.Screen.Y / 5);

            scoreText = new TextGameObject("fonts/MainMenuFont");
            scoreText.Text = string.Empty;
            
            gotoText = new TextGameObject("fonts/SpelFont");
            gotoText.Text = "Press escape to go back to the main menu";
            gotoText.Position = new Vector2(Tetris.Screen.X / 2 - (gotoText.Size.X / 2), Tetris.Screen.Y / 3 * 2);

            this.Add(gameOverText);
            this.Add(scoreText);
            this.Add(gotoText);
        }

        public override void Update(GameTime gameTime)
        {            
            // Get the score, and display it
            previouslyPlayedState = Tetris.GameStateManager.PreviousGameState as PlayingState;
            if (previouslyPlayedState != null)
            {
                score = previouslyPlayedState.GetScore();
                scoreText.Text = "Score: " + score;
                scoreText.Position = new Vector2(Tetris.Screen.X / 2 - (scoreText.Size.X / 2), Tetris.Screen.Y / 3);
            } 
            base.Update(gameTime);
        }
    }
}
