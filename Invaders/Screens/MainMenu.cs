using System;
using Microsoft.Xna.Framework;
using Infrastructure.ObjectModel.Menus;

namespace Invaders.Screens
{
    public class MainMenu : InvadersMenu
    {
        public MainMenu(Game i_Game) : base(i_Game, "Main menu")
        {
            ToggleMenuItem playersItem = new ToggleMenuItem(i_Game, "Players: ", new Vector2(100, 100), m_GameSettings.NumOfPlayers == 2, "Two", "One", m_ActiveMenuItemColor, m_InactiveMenuItemColor);
            playersItem.ValueChanged += new Action<bool>((i_Value) => m_GameSettings.NumOfPlayers = i_Value ? 2 : 1);
            AddMenuItem(playersItem);
            AddMenuItem(new ActionMenuItem(i_Game, "Screen Settings", new Vector2(100, 120), new Action(() => ScreensManager.SetCurrentScreen(new ScreenSettingsScreen(i_Game))), m_ActiveMenuItemColor, m_InactiveMenuItemColor));
            AddMenuItem(new ActionMenuItem(i_Game, "Sound Settings", new Vector2(100, 140), new Action(() => ScreensManager.SetCurrentScreen(new SoundSettingsScreen(i_Game))), m_ActiveMenuItemColor, m_InactiveMenuItemColor));
            AddMenuItem(new ActionMenuItem(i_Game, "Play", new Vector2(100, 160), new Action(() => { ScreensManager.Push(new PlayScreen(i_Game)); ScreensManager.SetCurrentScreen(new NextLevelScreen(i_Game, 0)); }), m_ActiveMenuItemColor, m_InactiveMenuItemColor));
            AddMenuItem(new ActionMenuItem(i_Game, "Quit", new Vector2(100, 180), new Action(() => i_Game.Exit()), m_ActiveMenuItemColor, Color.White));
        }
    }
}
