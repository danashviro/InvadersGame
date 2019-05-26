using System;
using Microsoft.Xna.Framework;
using Infrastructure.ObjectModel.Menus;

namespace Invaders.Screens
{
    public class ScreenSettingsScreen : InvadersMenu
    {
        public ScreenSettingsScreen(Game i_Game) : base(i_Game, "Screen Settings")
        {
            ToggleMenuItem mouseVisibilty = new ToggleMenuItem(i_Game, "Mouse visability: ", new Vector2(100, 100), m_GameSettings.MouseVisible, "Visible", "Invisible", m_ActiveMenuItemColor, m_InactiveMenuItemColor);
            mouseVisibilty.ValueChanged += new Action<bool>((i_Value) => m_GameSettings.MouseVisible = i_Value);
            AddMenuItem(mouseVisibilty);
            ToggleMenuItem windowResizing = new ToggleMenuItem(i_Game, "Allow Window Resizing: ", new Vector2(100, 120), m_GameSettings.AllowWindowResizing, "On", "Off", m_ActiveMenuItemColor, m_InactiveMenuItemColor);
            windowResizing.ValueChanged += new Action<bool>((i_Value) => m_GameSettings.AllowWindowResizing = i_Value);
            AddMenuItem(windowResizing);
            ToggleMenuItem fullScreenMode = new ToggleMenuItem(i_Game, "Full Screen Mode: ", new Vector2(100, 140), m_GameSettings.FullScreenMode, "On", "Off", m_ActiveMenuItemColor, m_InactiveMenuItemColor);
            fullScreenMode.ValueChanged += new Action<bool>((i_Value) => m_GameSettings.FullScreenMode = i_Value);
            AddMenuItem(fullScreenMode);
            AddMenuItem(new ActionMenuItem(i_Game, "Done", new Vector2(100, 160), ExitScreen, m_ActiveMenuItemColor, m_InactiveMenuItemColor));
        }
    }
}
