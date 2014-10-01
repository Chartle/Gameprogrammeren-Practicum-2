using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Practicum2.gameobjects
{
    class MiniPiece : SpriteGameObject
    {
        GameObjectGrid parentGrid;
        Color color;
 
        public MiniPiece(Color color, string id, string assetname = "sprites/block") : base(assetname, 0, id)
        {
            this.color = color;
            
            Debug.Print("made a minipiece with color " + color);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!visible || sprite == null)
                return;
            sprite.Draw(spriteBatch, this.GlobalPosition, origin, color);
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            if(inputHelper.KeyPressed(Keys.S) && !hasUpdated)
            {
                if (canMoveDown())
                {
                    moveDown();
                }
            }
        }

        public void moveDown()
        {
            int x = (int)(position.X / parentGrid.CellWidth);
            int y = (int)(position.Y / parentGrid.CellHeight);
            GameObject obj = this as GameObject;
            parentGrid.Add(null, x, y);
            parentGrid.Add(obj, x, y + 1);
            hasUpdated = true;
        }

        private bool canMoveDown()
        {
            GameObjectList state = Tetris.GameStateManager.GetGameState("onePlayerState") as GameObjectList;
            parentGrid = state.Find("pieceGrid") as GameObjectGrid;
            return position.Y/parentGrid.CellHeight < parentGrid.Rows-1;
        }

        public override void Update(GameTime gameTime)
        {
            hasUpdated = false;
            base.Update(gameTime);
        }
    }
}
