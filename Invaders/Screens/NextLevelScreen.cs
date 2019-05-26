using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Infrastructure.ObjectModel;

namespace Invaders.Screens
{
    public class NextLevelScreen : GameScreen
    {
        private int m_Level;
        private TimeSpan m_ScreenTimer;
        private int m_RemainingSecs;

        public NextLevelScreen(Game i_Game, int i_Level) : base(i_Game)
        {
            m_Background = new Sprite(@"Sprites\BG_Space01_1024x768", i_Game);
            m_Level = i_Level;
            m_ScreenTimer = TimeSpan.FromSeconds(3.0);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            m_ScreenTimer -= gameTime.ElapsedGameTime;
            if (m_ScreenTimer < TimeSpan.Zero)
            {
                ExitScreen();
            }
            else
            {
                m_RemainingSecs = m_ScreenTimer.Seconds + 1;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            SpriteBatch.Begin();
            SpriteBatch.DrawString(m_Font, m_RemainingSecs.ToString(), CenterOfViewPort, Color.Aqua);
            SpriteBatch.DrawString(m_Font, string.Format("Next level: {0}", m_Level + 1), new Vector2(50, 50), Color.Aqua);
            SpriteBatch.End();
        }  
    }
}