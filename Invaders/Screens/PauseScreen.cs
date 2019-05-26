using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Infrastructure.ObjectModel;

namespace Invaders.Screens
{
    public class PauseScreen : GameScreen
    {
        public PauseScreen(Game i_Game) : base(i_Game)
        {
            m_BlackTintAlpha = 0.4f;
            IsOverlayed = true;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (InputManager.KeyPressed(Keys.R))
            {
                ExitScreen();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            SpriteBatch.Begin();
            SpriteBatch.DrawString(m_Font, @"
      PAUSE
Press R to resume", CenterOfViewPort, Color.White);

            SpriteBatch.End();
        }
    }
}
