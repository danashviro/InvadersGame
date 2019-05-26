using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Infrastructure.ObjectModel;
using Microsoft.Xna.Framework.Audio;

namespace Invaders.Screens
{
    public class GameOverScreen : GameScreen
    {
        private const string k_GameOverSoundName = @"Sounds\GameOver";
        private string m_Msg;
        private SoundEffect m_GameOverSound;

        public GameOverScreen(Game i_Game, string i_GameoverMsg)
            : base(i_Game)
        {
            m_Background = new Sprite(@"Sprites\BG_Space01_1024x768", i_Game);
            m_Background.TintColor = Color.Red;
            m_Msg = i_GameoverMsg;
        }

        public override void Initialize()
        {
            base.Initialize();
            m_GameOverSound = Game.Content.Load<SoundEffect>(k_GameOverSoundName);
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            m_GameOverSound.Play();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (InputManager.KeyPressed(Keys.Escape))
            {
                Game.Exit();
            }
            else if (InputManager.KeyPressed(Keys.Home))
            {
                ScreensManager.Push(new PlayScreen(Game));
                ScreensManager.SetCurrentScreen(new NextLevelScreen(Game, 0));
            }
            else if(InputManager.KeyPressed(Keys.T))
            {
                ScreensManager.SetCurrentScreen(new MainMenu(Game));
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            SpriteBatch.Begin();
            SpriteBatch.DrawString(m_Font, "Game Over", new Vector2(CenterOfViewPort.X - 80, CenterOfViewPort.Y - 150), Color.White, 0, Vector2.Zero, 2, SpriteEffects.None, 0);
            SpriteBatch.DrawString(m_Font, m_Msg, new Vector2(CenterOfViewPort.X - 80, CenterOfViewPort.Y - 80), Color.White);
            SpriteBatch.DrawString(m_Font, @"
press esc to exit
press Home for a new game
press T for main manu
", new Vector2(50, CenterOfViewPort.Y), Color.White);

            SpriteBatch.End();
        }
    }
}
