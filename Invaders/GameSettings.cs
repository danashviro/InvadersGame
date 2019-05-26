using Microsoft.Xna.Framework;
using Infrastructure.ObjectModel;
using Infrastructure.Managers;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace Invaders
{
    public class GameSettings : GameService
    {
        public int NumOfPlayers { get; set; }

        public bool MouseVisible
        {
            get { return m_MouseVisible; }

            set
            {
                Game.IsMouseVisible = value;
                m_MouseVisible = value;
            }
        }

        public bool AllowWindowResizing
        {
            get
            {
                return m_AllowWindowResizing;
            }

            set
            {
                Game.Window.AllowUserResizing = value;
                m_AllowWindowResizing = value;
            }
        }

        public bool FullScreenMode
        {
            get { return m_FullScreenMode; }

            set
            {
                m_GraphicsDeviceManager.ToggleFullScreen();
                m_FullScreenMode = value;
            }
        }

        private bool m_MouseVisible;
        private bool m_AllowWindowResizing;
        private bool m_FullScreenMode;
        private GraphicsDeviceManager m_GraphicsDeviceManager;
        private float m_MusicVolume;
        private float m_EffectsVolume;

        public GameSettings(Game i_Game) : base(i_Game)
        {
            NumOfPlayers = 1;
            MouseVisible = false;
            AllowWindowResizing = false;
            MediaPlayer.IsRepeating = true;
            SoundsEffectsVol = BackgroundMusicVol = 1;
        }

        public override void Initialize()
        {
            base.Initialize();
            Muted = false;
            BackgroundMusicVol = 0.5f;
            SoundsEffectsVol = 1;
            m_GraphicsDeviceManager = Game.Services.GetService<GraphicsDeviceManager>();
            m_FullScreenMode = false;
        }

        public bool Muted { get; private set; }

        public float SoundsEffectsVol
        {
            get
            {
                return m_EffectsVolume;
            }

            set
            {
                m_EffectsVolume = SoundEffect.MasterVolume = value;
            }
        }

        public void ToggleSound()
        {
            Muted = !Muted;
            SoundEffect.MasterVolume = Muted ? 0 : SoundsEffectsVol;
            MediaPlayer.Volume = Muted ? 0 : BackgroundMusicVol;
        }

        public float BackgroundMusicVol
        {
            get
            {
                return m_MusicVolume;
            }

            set
            {
                m_MusicVolume = MediaPlayer.Volume = value;
            }
        }
    }
}
