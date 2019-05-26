using System;
using Microsoft.Xna.Framework;
using Infrastructure.ObjectModel;
using Microsoft.Xna.Framework.Audio;

namespace Invaders.InvadersObjects
{
    public class EnemiesFormation : CompositeDrawableComponent<Enemy>, IComander
    {
        private const int k_NumOfEnemiesRows = 5;
        private const int k_MaxNumOfEnemiesCols = 14;
        private const int k_MinNumOfEnemiesCols = 9;
        private const float k_InitNumOfSecondsBetweenJumps = 0.5f;
        private const int k_FirstLineEnemiesWorth = 260;
        private const int k_MiddleLinesEnemiesWorth = 140;
        private const int k_LastLinesEnemiesWorth = 110;
        private const float k_PrecentToDecreaseWhenDeadEnemiesBufferFull = 4.0f;
        private const int k_DeadEnemiesFullBuffer = 4;
        private const float k_PrecentToDecreaseWhenGoingDown = 8.0f;
        private const string k_LevelWinSoundName = @"Sounds\LevelWin";
        private int m_NumOfCols;
        private int m_NumberOfEnemiesAlive;
        private int m_DeadBuffer;
        private TimeSpan m_SecondsToNextJump;
        private Action m_NotifyAllDead;
        private Enemy[][] m_Enemies;
        private Enemy m_MostRightEnemy, m_MostLeftEnemy;
        private SoundEffect m_LevelWinSound;

        public event Action NotifyTimeBetweenJumpsChanged;

        public TimeSpan SecondsBetweenJumps { get; private set; }

        public bool GoDown { get; private set; }

        public float JumpDistance { get; private set; }

        public bool TimeToJump { get; private set; }

        public bool TimeToShoot { get; private set; }

        public int Direction { get; private set; }

        public EnemiesFormation(Game i_Game, Action<int, IShooter> i_NotifyOneDead, Action i_TouchedEnd, Action i_NotifyAllDead) : base(i_Game)
        {
            m_Enemies = new Enemy[k_NumOfEnemiesRows][];
            m_NotifyAllDead = i_NotifyAllDead;
            buildEnemies(i_NotifyOneDead, i_TouchedEnd);
            SecondsBetweenJumps = TimeSpan.FromSeconds(k_InitNumOfSecondsBetweenJumps);
        }

        private void buildEnemies(Action<int, IShooter> i_NotifyOneDead, Action i_TouchedEnd)
        {
            int myUpdatePosition = UpdateOrder;
            for (int i = 0; i < k_NumOfEnemiesRows; i++)
            {
                m_Enemies[i] = new Enemy[k_MaxNumOfEnemiesCols];
                for (int j = 0; j < k_MaxNumOfEnemiesCols; j++)
                {
                    if (i == 0)
                    {
                        m_Enemies[i][j] = new Enemy(Game, Color.Pink, k_FirstLineEnemiesWorth, this, 0, 1);
                    }
                    else if (i == 1)
                    {
                        m_Enemies[i][j] = new Enemy(Game, Color.LightBlue, k_MiddleLinesEnemiesWorth, this, 2, 3);
                    }
                    else if(i == 2)
                    {
                        m_Enemies[i][j] = new Enemy(Game, Color.LightBlue, k_MiddleLinesEnemiesWorth, this, 3, 2);
                    }
                    else if(i == 3)
                    {
                        m_Enemies[i][j] = new Enemy(Game, Color.Yellow, k_LastLinesEnemiesWorth, this, 4, 5);
                    }
                    else if (i == 4)
                    {
                        m_Enemies[i][j] = new Enemy(Game, Color.Yellow, k_LastLinesEnemiesWorth, this, 5, 4);
                    }

                    m_Enemies[i][j].NotifyDead += i_NotifyOneDead;
                    m_Enemies[i][j].TouchedEnd += i_TouchedEnd;
                    NotifyTimeBetweenJumpsChanged += m_Enemies[i][j].UpdateCellAnimator;
                    Add(m_Enemies[i][j]);
                }
            }
        }

        public void InitByLevel(int i_Level)
        {
            m_NumOfCols = k_MinNumOfEnemiesCols + i_Level;
            SecondsBetweenJumps = TimeSpan.FromSeconds(k_InitNumOfSecondsBetweenJumps);
            m_SecondsToNextJump = SecondsBetweenJumps;
            for (int i = 0; i < k_NumOfEnemiesRows; i++)
            {
                for (int j = 0; j < k_MaxNumOfEnemiesCols; j++)
                {
                    m_Enemies[i][j].Init(i_Level);
                    m_Enemies[i][j].Enabled = m_Enemies[i][j].Visible = j < m_NumOfCols;                     
                }
            }

            initPositions();
            m_NumberOfEnemiesAlive = k_NumOfEnemiesRows * m_NumOfCols;
            m_DeadBuffer = 0;
            Direction = 1;
            reset();
        }

        public override void Initialize()
        {
            base.Initialize();
            m_LevelWinSound = Game.Content.Load<SoundEffect>(k_LevelWinSoundName);
        }

        private void initPositions()
        {
            float y, x;
            y = Enemy.sr_Height * 3.0f;
            for (int i = 0; i < k_NumOfEnemiesRows; i++)
            {
                x = 0f;
                for (int j = 0; j < m_NumOfCols; j++)
                {
                    x += Enemy.sr_Width * 1.6f;
                    m_Enemies[i][j].Position = new Vector2(x, y);
                }

                y += Enemy.sr_Height * 1.6f;
            }

            m_MostLeftEnemy = m_Enemies[0][0];
            m_MostRightEnemy = m_Enemies[0][m_NumOfCols - 1];
        }

        public void DecreaseOneEnemy(Enemy i_Enemy)
        {
            m_NumberOfEnemiesAlive--;
            if (m_NumberOfEnemiesAlive == 0)
            {
                m_LevelWinSound.CreateInstance().Play();
                m_NotifyAllDead();              
            }

            m_DeadBuffer++;
            if (m_DeadBuffer == k_DeadEnemiesFullBuffer)
            {
                decreaseTimeBetweenJumpsByPercantage(k_PrecentToDecreaseWhenDeadEnemiesBufferFull);
                m_DeadBuffer = 0;
            }

            if(i_Enemy == m_MostLeftEnemy)
            {
                findMostLeftEnemy();
            }
            else if(i_Enemy == m_MostRightEnemy)
            {
                findMostRightEnemy();
            }
        }

        public override void Update(GameTime i_GameTime)
        {
            reset();
            m_SecondsToNextJump -= i_GameTime.ElapsedGameTime;
            if (m_SecondsToNextJump.TotalSeconds - (m_SecondsToNextJump.TotalSeconds / 2.0) < 0)
            {
                TimeToShoot = true;
            }

            if (m_SecondsToNextJump.TotalSeconds <= 0)
            {
                TimeToJump = true;
                findMarginForNextJump();
                if (JumpDistance == 0)
                {
                    GoDown = true;
                    Direction *= -1;
                }

                m_SecondsToNextJump += SecondsBetweenJumps;
            }

            base.Update(i_GameTime);
        }

        private void reset()
        {
            if (GoDown)
            {
                decreaseTimeBetweenJumpsByPercantage(k_PrecentToDecreaseWhenGoingDown);
            }

            GoDown = false;
            TimeToShoot = false;
            TimeToJump = false;
        }

        private void findMarginForNextJump()
        {
            float distanceToNearestWall;

            if (Direction == 1)
            {
                distanceToNearestWall = (float)Game.GraphicsDevice.Viewport.Width - (m_MostRightEnemy.Position.X + Enemy.sr_Width);
            }
            else
            {
                distanceToNearestWall = m_MostLeftEnemy.Position.X;
            }

            JumpDistance = distanceToNearestWall > Enemy.sr_Width ? Enemy.sr_Width : distanceToNearestWall;
        }

        private void findMostLeftEnemy()
        {
            bool done = false;

            for (int i = 0; i < m_NumOfCols && !done; i++)
            {
                for (int j = 0; j < k_NumOfEnemiesRows && !done; j++)
                {
                    if (m_Enemies[j][i].Enabled && m_Enemies[j][i].IsActive)
                    {
                        m_MostLeftEnemy = m_Enemies[j][i];
                        done = true;
                    }
                }
            }
        }
      
        private void findMostRightEnemy()
        {
            bool done = false;
            for (int i = m_NumOfCols - 1; i >= 0 && !done; i--)
            {
                for (int j = 0; j < k_NumOfEnemiesRows && !done; j++)
                {
                    if (m_Enemies[j][i].Enabled && m_Enemies[j][i].IsActive)
                    {
                        m_MostRightEnemy = m_Enemies[j][i];
                        done = true;
                    }
                }
            }
        }

        public void decreaseTimeBetweenJumpsByPercantage(float i_Percentage)
        {
            SecondsBetweenJumps = TimeSpan.FromSeconds(SecondsBetweenJumps.TotalSeconds - (SecondsBetweenJumps.TotalSeconds * (i_Percentage / 100.0f)));
            NotifyTimeBetweenJumpsChanged.Invoke();
        }
    }
}
