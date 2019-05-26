using System;
using Infrastructure.ObjectModel.Menus;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Infrastructure.ObjectModel;

namespace Invaders.Screens
{
    public abstract class InvadersMenu : Menu
    {
        private string k_MenuMoveSoundName = @"Sounds\MenuMove";
        protected Color m_ActiveMenuItemColor = Color.Red;
        protected Color m_InactiveMenuItemColor = Color.White;
        protected GameSettings m_GameSettings;

        public InvadersMenu(Game i_Game, string i_Title) : base(i_Game, i_Title)
        {
            m_GameSettings = Game.Services.GetService<GameSettings>();
            m_Background = new Sprite(@"Sprites\BG_Space01_1024x768", i_Game);
        }

        public override void Initialize()
        {
            base.Initialize();
            SoundEffect menuMoveSoundEffect = Game.Content.Load<SoundEffect>(k_MenuMoveSoundName);
            IndexChanged += new Action(() => menuMoveSoundEffect.CreateInstance().Play());
        }
    }
}
