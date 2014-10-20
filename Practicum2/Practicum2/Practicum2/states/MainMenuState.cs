using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Practicum2.gameobjects;

namespace Practicum2.states
{
    class MainMenuState : GameObjectList
    {
        TextGameObject mainMenuText;

        public MainMenuState()
        {
            // Initialize and add the text

            mainMenuText = new TextGameObject("fonts/MainMenuFont");
            mainMenuText.Text = "TETRIS\n\nPress space to play";
            mainMenuText.Position = new Vector2(100, 100);
            this.Add(mainMenuText);
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            base.HandleInput(inputHelper);
            // When the space key is pressed, go to the playing state, play the main song and set the score to 0.
            if (inputHelper.KeyPressed(Keys.Space))
            {
                Tetris.GameStateManager.SwitchTo("onePlayerState");
                Tetris.AssetManager.PlayMusic("audio/tetrisSong");
                PlayingState playingstate = Tetris.GameStateManager.CurrentGameState as PlayingState;
                playingstate.SetScore(0);
            }
        }
    }
}
