using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
namespace Practicum2.states
{
    class MainMenuState : GameObjectList
    {
        public MainMenuState()
        {
            TextGameObject mainMenuText = new TextGameObject("fonts/MainMenuFont");
            mainMenuText.Text = "TETRIS\n\nPress space to play";
            mainMenuText.Position = new Vector2(100, 100);
            this.Add(mainMenuText);
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            base.HandleInput(inputHelper);
            if(inputHelper.KeyPressed(Keys.Space))
            {
                GameEnvironment.GameStateManager.SwitchTo("onePlayerState");
            }
        }
    }
}
