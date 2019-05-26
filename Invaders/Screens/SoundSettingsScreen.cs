using System;
using Microsoft.Xna.Framework;
using Infrastructure.ObjectModel.Menus;

namespace Invaders.Screens
{
    public class SoundSettingsScreen : InvadersMenu
    {
        public SoundSettingsScreen(Game i_Game) : base(i_Game, "Sound Settings")
        {
            ToggleMenuItem toggleSound = new ToggleMenuItem(i_Game, "Toggle sound: ", new Vector2(100, 100), m_GameSettings.Muted, "Off", "On", m_ActiveMenuItemColor, m_InactiveMenuItemColor);
            toggleSound.ValueChanged += new Action<bool>((i_Value) => m_GameSettings.ToggleSound());
            AddMenuItem(toggleSound);
            RangeMenuItem backgroundMusicVol = new RangeMenuItem(i_Game, "Background Music Volume: ", new Vector2(100, 120), 100, 0, (int)(m_GameSettings.BackgroundMusicVol * 100), 10, m_ActiveMenuItemColor, m_InactiveMenuItemColor);
            backgroundMusicVol.ValueChanged += new Action<int>((i_Value) => m_GameSettings.BackgroundMusicVol = ((float)i_Value) / 100);
            AddMenuItem(backgroundMusicVol);
            RangeMenuItem soundEffectsVol = new RangeMenuItem(i_Game, "Sound Effects Volume: ", new Vector2(100, 140), 100, 0, (int)(m_GameSettings.SoundsEffectsVol * 100), 10, m_ActiveMenuItemColor, m_InactiveMenuItemColor);
            soundEffectsVol.ValueChanged += new Action<int>((i_Value) => m_GameSettings.SoundsEffectsVol = ((float)i_Value) / 100);
            AddMenuItem(soundEffectsVol);
            AddMenuItem(new ActionMenuItem(i_Game, "Done", new Vector2(100, 160), new Action(() => ExitScreen()), m_ActiveMenuItemColor, m_InactiveMenuItemColor));         
        }
    }
}
