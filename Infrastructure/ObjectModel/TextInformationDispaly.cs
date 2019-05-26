using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Infrastructure.ObjectModel
{
    public class TextInformationDispaly : Sprite
    {
        private const string k_AssetName = @"Fonts\ComicSansMS";
        private SpriteFont m_Font;
        private string m_InformationTitle;
        private string m_Information;

        public TextInformationDispaly(Game i_Game, string i_InformationTitle, string i_Information, Color i_Color) : base(k_AssetName, i_Game)
        {
            m_InformationTitle = i_InformationTitle;
            m_Information = i_Information;
            TintColor = i_Color;
        }

        protected override void LoadContent()
        {
            m_Font = Game.Content.Load<SpriteFont>(k_AssetName);

            if (m_SpriteBatch == null)
            {
                m_SpriteBatch = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;

                if (m_SpriteBatch == null)
                {
                    m_SpriteBatch = new SpriteBatch(Game.GraphicsDevice);
                    m_UseSharedBatch = false;
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (!m_UseSharedBatch)
            {
                m_SpriteBatch.Begin();
            }

            m_SpriteBatch.DrawString(m_Font, m_InformationTitle + m_Information, Position, TintColor);

            if (!m_UseSharedBatch)
            {
                m_SpriteBatch.End();
            }            
        }

        public void UpdateInformation(string i_NewInformation)
        {
            m_Information = i_NewInformation;
        }

        protected override void InitBounds()
        {
        }
    }
}
