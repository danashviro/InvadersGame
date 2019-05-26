using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Infrastructure.ObjectModel.Menus
{
    public class RangeMenuItem : MenuItem
    {
        private int m_MaxValue, m_MinValue, m_JumpValue, m_Value;

        public event Action<int> ValueChanged;

        public RangeMenuItem(Game game, string i_Title, Vector2 i_Position, int i_MaxValue, int i_MinValue, int i_StartValue, int i_JumpValue, Color i_ActiveColor, Color i_InactiveColor) : base(game, i_Title, i_Position, i_ActiveColor, i_InactiveColor)
        {
            m_MaxValue = i_MaxValue;
            m_MinValue = i_MinValue;
            m_JumpValue = i_JumpValue;
            Value = i_StartValue;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (m_InputManager.KeyPressed(Keys.PageUp))
            {
                Value += m_JumpValue;     
            }
            else if(m_InputManager.KeyPressed(Keys.PageDown))
            {
                Value -= m_JumpValue;
            }
        }

        public int Value
        {
            get
            {
                return m_Value;
            }

            private set
            {
                m_Value = value;
                if(m_Value > m_MaxValue)
                {
                    m_Value = m_MinValue;
                }
                else if(m_Value < m_MinValue)
                {
                    m_Value = m_MaxValue;
                }

                m_Information.UpdateInformation(m_Value.ToString());
                ValueChanged?.Invoke(m_Value);
            }
        }
    }
}
