using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Infrastructure.ObjectModel;
using Microsoft.Xna.Framework.Media;

namespace Invaders.Screens
{
    public class WelcomeScreen : GameScreen
    {
        public WelcomeScreen(Game i_Game) : base(i_Game)
        {
            m_Background = new Sprite(@"Sprites\BG_Space01_1024x768", i_Game);
            Add(m_Background);
        }

        public override void Initialize()
        {
            base.Initialize();
            MediaPlayer.Play(Game.Content.Load<Song>(@"Sounds\BGMusic"));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (InputManager.KeyPressed(Keys.Escape))
            {
                Game.Exit();
            }
            else if (InputManager.KeyPressed(Keys.Enter))
            {
                ScreensManager.Push(new PlayScreen(Game));
                ScreensManager.SetCurrentScreen(new NextLevelScreen(Game, 0));
            }
            else if (InputManager.KeyPressed(Keys.T))
            {
                ScreensManager.SetCurrentScreen(new MainMenu(Game));
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            SpriteBatch.Begin();
            SpriteBatch.DrawString(m_Font, "Welcome", new Vector2(CenterOfViewPort.X - 80, CenterOfViewPort.Y - 150), Color.White, 0, Vector2.Zero, 2, SpriteEffects.None, 0);
            SpriteBatch.DrawString(m_Font, @"
press Enter for a new game
press esc to exit
press T for main manu", new Vector2(CenterOfViewPort.X - 80, CenterOfViewPort.Y - 80), Color.White);
            SpriteBatch.End();
        }
    }
}
