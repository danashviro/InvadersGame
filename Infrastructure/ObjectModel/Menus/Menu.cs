using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Infrastructure.ObjectModel.Menus
{
    public class Menu : GameScreen
    {
        protected List<MenuItem> m_MenuItems;
        private int m_SelectedIndex;
        protected string m_Title;
        public event Action IndexChanged;

        public Menu(Game i_Game, string i_Title) : base(i_Game)
        {
            m_MenuItems = new List<MenuItem>();
            m_Title = i_Title;
        }

        protected void AddMenuItem(MenuItem i_MenuItem)
        {
            m_MenuItems.Add(i_MenuItem);
            this.Add(i_MenuItem);
            if (m_MenuItems.Count == 1)
            {
                SelectedIndex = 0;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (InputManager.KeyPressed(Keys.Up))
            {
                SelectedIndex--;
            }
            else if(InputManager.KeyPressed(Keys.Down))
            {
                SelectedIndex++;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();
            SpriteBatch.DrawString(m_Font, m_Title, new Vector2(50, 50), Color.White);
            SpriteBatch.End();
            base.Draw(gameTime);
        }

        private int SelectedIndex
        {
            get
            {
                return m_SelectedIndex;
            }

            set
            {
                if(m_SelectedIndex!=value)
                {
                    m_MenuItems[m_SelectedIndex].Selected = false;
                    m_SelectedIndex = value;
                    if (m_SelectedIndex == m_MenuItems.Count)
                    {
                        m_SelectedIndex = 0;
                    }
                    else if (m_SelectedIndex < 0)
                    {
                        m_SelectedIndex = m_MenuItems.Count - 1;
                    }
                    IndexChanged?.Invoke();
                }

                m_MenuItems[m_SelectedIndex].Selected = true;
            }
        }


    }
}
