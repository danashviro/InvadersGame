using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Infrastructure.Managers;
using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.Animators;
using Microsoft.Xna.Framework.Audio;

namespace Invaders.InvadersObjects
{
    public class PlayerSpaceship : Sprite, ICollidable2D, IShooter
    {
        private const int k_Speed = 145;
        private const int k_InitNumOfLives = 3;
        private const int k_MaxNumOfBullets = 3;
        private const int k_CollidScore = -1100;
        private const string k_GunShootSoundName = @"Sounds\SSGunShot";
        private const string k_LifeDieSoundName = @"Sounds\LifeDie";
        private int m_Lives;
        private Gun m_Gun;
        private bool m_HasDraw = false;
        private bool m_UseMouse;
        private IInputManager m_InputManager;
        private Vector2 m_InitialPosition;
        private Keys m_LeftKey;
        private Keys m_RightKey;
        private Keys m_ShootKey;
        private bool m_PositionInitialized = false;
        private SoundEffect m_LifeDieSoundEffect;

        public event Action NotifyDead;

        public event Action<string> ScoreChanged;

        public event Action<int> LivesChanged;

        public int Score { get; private set; }

        public PlayerSpaceship(string i_AssetName, Game i_Game, Keys i_LeftKey, Keys i_RightKey, Keys i_ShootKey, bool i_UseMouse) : base(i_AssetName, i_Game)
        {
            Score = 0;
            m_Lives = k_InitNumOfLives;
            m_Gun = new Gun(i_Game, this, k_MaxNumOfBullets, new Vector2(0, -155), Color.Red);
            m_RestrictedToWindow = true;
            m_LeftKey = i_LeftKey;
            m_RightKey = i_RightKey;
            m_ShootKey = i_ShootKey;
            m_UseMouse = i_UseMouse;
        }

        public override void Initialize()
        { 
            m_InputManager = Game.Services.GetService(typeof(IInputManager)) as IInputManager;
            base.Initialize();
            m_LifeDieSoundEffect = Game.Content.Load<SoundEffect>(k_LifeDieSoundName);
            RotationOrigin = SourceRectangleCenter;
            initAnimations();
            m_Gun.Initialize();
            m_Gun.InitBulletSound(k_GunShootSoundName);
        }

        public override Vector2 Position
        {
            get
            {
                return base.Position;
            }

            set
            {
                if (!m_PositionInitialized)
                {
                    m_PositionInitialized = true;
                    m_InitialPosition = value;
                }

                base.Position = value;
            }
        }

        private void initAnimations()
        {
            BlinkAnimator blinkAnimator = new BlinkAnimator("hitByBulletAnimator", 6, TimeSpan.FromSeconds(2.5));
            CompositeAnimator destroyed = new CompositeAnimator(
                "destroyedAnimator", 
                TimeSpan.FromSeconds(2.5), 
                this,
                new FadeAnimator("fadeAnimator", TimeSpan.FromSeconds(2.5)),
                new RotateAnimator("rotateAnimator", TimeSpan.FromSeconds(2.5), 4));
            blinkAnimator.Finished += new Action(() => { Position = m_InitialPosition; IsActive = true; Velocity = Vector2.Zero; });
            destroyed.Finished += remove;
            Animations.Add(blinkAnimator);
            Animations.Add(destroyed);
            Animations.Resume();
            blinkAnimator.Pause();
            destroyed.Pause();
        }

        protected override void LoadContent()
        {
            UseNonPremultipliedSpriteBatch();
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (IsActive)
            {
                getInputFromKeyboard();
                if (m_UseMouse)
                {
                    getInputFromMouse();
                }
            } 
            else
            {
                Velocity = Vector2.Zero;
            }  
                
            base.Update(gameTime);
            m_Gun.Update(gameTime);
        }

        private void getInputFromMouse()
        {
            if(m_InputManager.ButtonPressed(eInputButtons.Left))
            {
                m_Gun.Shoot(new Vector2(Position.X + (Width / 2), Position.Y));
            }

            if (m_HasDraw)
            {
                Position = new Vector2(Position.X + m_InputManager.MousePositionDelta.X, Position.Y);
            }
        }

        private void getInputFromKeyboard()
        {
            if (m_InputManager.KeyboardState.IsKeyDown(m_LeftKey))
            {
                m_Velocity.X = k_Speed * -1;
            }
            else if (m_InputManager.KeyboardState.IsKeyDown(m_RightKey)) 
            {
                m_Velocity.X = k_Speed;
            }
            else
            {
                m_Velocity.X = 0;
            }

            if (m_InputManager.KeyPressed(m_ShootKey))
            {
                m_Gun.Shoot(new Vector2(Position.X + (Width / 2), Position.Y));
            }
        }  

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            m_HasDraw = true;
            m_Gun.Draw(gameTime);
        }

        public void AddScore(int i_ToAdd)
        {
            Score += i_ToAdd;
            if (Score < 0)
            {
                Score = 0;
            }

            ScoreChanged(Score.ToString());
        }

        public override void Collided(ICollidable i_CollidingObject)
        {
            if (!(i_CollidingObject is PlayerSpaceship))
            {
                if (i_CollidingObject is Bullet)
                {        
                    Lives--;
                    m_LifeDieSoundEffect.CreateInstance().Play();
                }
                else if (i_CollidingObject is Enemy)
                {
                    Lives = 0;
                    m_LifeDieSoundEffect.CreateInstance().Play();
                }

                AddScore(k_CollidScore);
                if (m_Lives == 0)
                {
                    Animations["destroyedAnimator"].Resume();
                }
                else 
                {
                    Animations["hitByBulletAnimator"].Restart();
                }

                IsActive = false;
            }
        }

        private void remove()
        {
            Dispose(true);
            Game.Components.Remove(this);
            Visible = false;
            NotifyDead.Invoke();
        }

        private int Lives
        {
            get
            {
                return m_Lives;
            }

            set
            {
                m_Lives = value;
                LivesChanged(m_Lives);
            }
        }

        public void Init()
        {
            m_Gun.Init();
        }
    }
}