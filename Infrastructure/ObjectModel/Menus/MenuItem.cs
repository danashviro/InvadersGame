using Microsoft.Xna.Framework;
using Infrastructure.Managers;
using Infrastructure.ObjectModel;

namespace Infrastructure.ObjectModel.Menus
{
    public class MenuItem : DrawableGameComponent
    {
        protected TextInformationDispaly m_Information;
        protected IInputManager m_InputManager;
        protected Color m_ActiveColor, m_InactiveColor;

        public MenuItem(Game game, string i_Title, Vector2 i_Position, Color i_ActiveColor, Color i_InactiveColor) : base(game)
        {
            m_Information = new TextInformationDispaly(game, i_Title, string.Empty, i_InactiveColor);
            m_Information.Position = i_Position;
            m_ActiveColor = i_ActiveColor;
            m_InactiveColor = i_InactiveColor;
            Selected = false;
        }

        public override void Initialize()
        {
            base.Initialize();
            m_Information.Initialize();
            m_InputManager = Game.Services.GetService<IInputManager>();
        }

        public bool Selected
        {
            get
            {
                return Enabled;
            }

            set
            {
                Enabled = value;
                m_Information.TintColor = Enabled ? m_ActiveColor : m_InactiveColor;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            m_Information.Draw(gameTime);
        }
    }
}
