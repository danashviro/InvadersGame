using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Infrastructure.ObjectModel.Menus
{
    public class ToggleMenuItem : MenuItem
    {
        private bool m_Value;
        private string m_TrueMsg, m_FalseMsg;

        public event Action<bool> ValueChanged;

        public ToggleMenuItem(Game game, string i_Title, Vector2 i_Position, bool i_StartingValue, string i_TrueMsg, string i_FalseMsg, Color i_ActiveColor, Color i_InactiveColor) : base(game, i_Title, i_Position, i_ActiveColor, i_InactiveColor)
        {
            m_TrueMsg = i_TrueMsg;
            m_FalseMsg = i_FalseMsg;
            Value = i_StartingValue;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (m_InputManager.KeyPressed(Keys.PageUp) || m_InputManager.KeyPressed(Keys.PageDown))
            {
                Value = !Value;
            }
        }

        public bool Value
        {
            get
            {
                return m_Value;
            }

            set
            {
                m_Value = value;
                m_Information.UpdateInformation(m_Value ? m_TrueMsg : m_FalseMsg);
                ValueChanged?.Invoke(m_Value);
            }
        }
    }
}
