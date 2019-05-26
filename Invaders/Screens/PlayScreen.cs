using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Infrastructure.ObjectModel;
using Infrastructure.Managers;
using Invaders.InvadersObjects;

namespace Invaders.Screens
{
    public class PlayScreen : GameScreen
    {
        private const int k_NumOfBarriers = 4;
        private PlayerSpaceship m_Player1, m_Player2;
        private int m_NumOfAlivePlayers;
        private TextInformationDispaly m_Player1Score, m_Player2Score;
        private LineOfSprites m_Player1Lives, m_Player2Lives;
        private EnemiesFormation m_Enemies;
        private Barrier[] m_Barriers;
        private int m_Level = 0;
        private GameSettings m_GameSettings;

        public PlayScreen(Game i_Game) : base(i_Game)
        {
            if (Game.Services.GetService<ICollisionsManager>() != null)
            {
                Game.Services.RemoveService(typeof(ICollisionsManager));
            }

            m_GameSettings = Game.Services.GetService<GameSettings>();
            new CollisionsManager(i_Game);
            m_Background = new Sprite(@"Sprites\BG_Space01_1024x768", i_Game);
            MotherShip motherShip = new MotherShip(i_Game);
            motherShip.NotifyDead += addScore;
            Add(motherShip);
            m_Player1Score = new TextInformationDispaly(i_Game, "P1 Score: ", "0", Color.RoyalBlue);
            m_Player1Lives = new LineOfSprites(i_Game, @"Sprites\Ship01_32x32", 3, new Color(Color.White, 0.5f), new Vector2(0.5f));
            Add(m_Player1Score);
            Add(m_Player1Lives);
            m_Player1 = new PlayerSpaceship(@"Sprites\Ship01_32x32", i_Game, Keys.H, Keys.K, Keys.U, true);
            m_Player1.LivesChanged += m_Player1Lives.ChangeNumOfSprites;
            m_Player1.NotifyDead += decreaseOnePlayer;
            m_Player1.ScoreChanged += m_Player1Score.UpdateInformation;
            Add(m_Player1);
            m_NumOfAlivePlayers = m_GameSettings.NumOfPlayers;
            if (m_NumOfAlivePlayers == 2)
            {
                m_Player2Score = new TextInformationDispaly(i_Game, "P2 Score: ", "0", Color.Green);
                m_Player2Lives = new LineOfSprites(i_Game, @"Sprites\Ship02_32x32", 3, new Color(Color.White, 0.5f), new Vector2(0.5f));
                Add(m_Player2Score);
                Add(m_Player2Lives);
                m_Player2 = new PlayerSpaceship(@"Sprites\Ship02_32x32", i_Game, Keys.A, Keys.D, Keys.W, false);
                m_Player2.LivesChanged += m_Player2Lives.ChangeNumOfSprites;
                m_Player2.NotifyDead += decreaseOnePlayer;
                m_Player2.ScoreChanged += m_Player2Score.UpdateInformation;
                Add(m_Player2);
            }

            m_Barriers = new Barrier[k_NumOfBarriers];
            for (int i = 0; i < k_NumOfBarriers; i++)
            {
                m_Barriers[i] = new Barrier(i_Game);
                Add(m_Barriers[i]);
            }

            m_Enemies = new EnemiesFormation(i_Game, addScore, endGame, levelUp);
            Add(m_Enemies);
        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.KeyPressed(Keys.P))
            {
                ScreensManager.SetCurrentScreen(new PauseScreen(Game));
            }
            else
            {
                base.Update(gameTime);
            }
        }

        private void initByLevel()
        {
            initPositions();
            m_Enemies.InitByLevel(m_Level % 6);
            for (int i = 0; i < k_NumOfBarriers; i++)
            {
                m_Barriers[i].InitByLevel(m_Level % 6);
            }

            m_Player1.Init();
            if (m_GameSettings.NumOfPlayers == 2)
            {
                m_Player2.Init();
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            initByLevel();
        }

        private void decreaseOnePlayer()
        {
            if (--m_NumOfAlivePlayers == 0)
            {
                endGame();
            }
        }

        private void addScore(int i_Score, IShooter i_Shooter)
        {
            PlayerSpaceship shooter = i_Shooter as PlayerSpaceship;
            if (shooter != null)
            {
                shooter.AddScore(i_Score);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        private void levelUp()
        {
            m_Level++;
            initByLevel();
            ScreensManager.SetCurrentScreen(new NextLevelScreen(Game, m_Level));
        }

        private void endGame()
        {
            ExitScreen();
            ScreensManager.SetCurrentScreen(new GameOverScreen(Game, getGameOverMsg()));
        }

        private string getGameOverMsg()
        {
            string msg = null;
            if (m_GameSettings.NumOfPlayers == 1)
            {
                msg = string.Format("Player one Score: {0}", m_Player1.Score);
            }
            else if (m_GameSettings.NumOfPlayers == 2)
            {
                msg = string.Format(@"
Player one Score: {0}
Player two score: {1}", m_Player1.Score, m_Player2.Score);
            }

            return msg;
        }

        private void initPositions()
        {
            m_Player1.Position = new Vector2(20, (float)(GraphicsDevice.Viewport.Height) -48);
            m_Player1Lives.InitPositions(new Vector2((float)(GraphicsDevice.Viewport.Width) -55, 5));
            m_Player1Score.Position = Vector2.Zero;

            if (m_GameSettings.NumOfPlayers == 2)
            {
                m_Player2.Position = new Vector2(50, (float)(GraphicsDevice.Viewport.Height) -48);
                m_Player2Lives.InitPositions(new Vector2((float)(GraphicsDevice.Viewport.Width) -55, 25));
                m_Player2Score.Position = new Vector2(0, 30);
            }

            float x = (GraphicsDevice.Viewport.Width / 2) - 154;
            m_Barriers[0].Position = new Vector2(x, m_Player1.Position.Y - 64);
            m_Barriers[1].Position = new Vector2(x + 88, m_Player1.Position.Y - 64);
            m_Barriers[2].Position = new Vector2(x + (88 * 2), m_Player1.Position.Y - 64);
            m_Barriers[3].Position = new Vector2(x + (88 * 3), m_Player1.Position.Y - 64);
        }
    }
}