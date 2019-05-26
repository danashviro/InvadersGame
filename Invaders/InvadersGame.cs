using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Infrastructure.Managers;
using Invaders.Screens;

namespace Invaders
{
    public class InvadersGame : Game
    {
        private GraphicsDeviceManager m_Graphics;
        private InputManager m_InputManager;
        private GameSettings m_GameSettings;

        public InvadersGame()
        {
            m_Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Services.AddService(m_Graphics);
            m_InputManager = new InputManager(this);
            m_GameSettings = new GameSettings(this);
            ScreensMananger screenManager = new ScreensMananger(this);
            screenManager.SetCurrentScreen(new WelcomeScreen(this));
        }

        protected override void Initialize()
        {
            base.Initialize();            
        }

        protected override void Update(GameTime gameTime)
        {
            if (IsActive)
            {
                if (m_InputManager.KeyPressed(Keys.M))
                {
                    m_GameSettings.ToggleSound();
                }

                base.Update(gameTime);
            }
        }

        protected override void Draw(GameTime i_GameTime)
        {
            m_Graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(i_GameTime);
        }
    }
}
