using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Practicum2.states;

namespace Practicum2
{
    public class Tetris : GameEnvironment
    {
        static void Main()
        {
            Tetris game = new Tetris();
            game.Run();
        }

        public Tetris()
        {
            Content.RootDirectory = "Content";
            this.IsMouseVisible = false;
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            screen = new Point(600, 800);
            this.SetFullScreen(false);

            gameStateManager.AddGameState("onePlayerState", new OnePlayerState());
            gameStateManager.AddGameState("mainMenuState", new MainMenuState());
            gameStateManager.SwitchTo("mainMenuState");
        }
    }
}
