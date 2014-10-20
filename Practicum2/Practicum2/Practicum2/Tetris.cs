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
using System.IO;

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
            // Set the screen size, and add the states
            base.LoadContent();
            screen = new Point(600, 800);
            this.SetFullScreen(false);

            gameStateManager.AddGameState("onePlayerState", new OnePlayerState());
            gameStateManager.AddGameState("mainMenuState", new MainMenuState());
            gameStateManager.AddGameState("gameOverState", new GameOverState());
            gameStateManager.SwitchTo("mainMenuState");
            assetManager.PlayMusic("audio/menuSong");
        }
    }
}
