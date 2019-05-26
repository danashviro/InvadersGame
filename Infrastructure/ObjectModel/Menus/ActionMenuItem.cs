using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Infrastructure.ObjectModel.Menus
{
    public class ActionMenuItem : MenuItem
    {
        private Action m_Action;

        public ActionMenuItem(Game game, string i_Title, Vector2 i_Position, Action i_Action, Color i_ActiveColor, Color i_InactiveColor) : base(game, i_Title, i_Position, i_ActiveColor, i_InactiveColor)
        {
            m_Action = i_Action;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (m_InputManager.KeyPressed(Keys.Enter))
            {
                m_Action();
            }
        }
    }
}
