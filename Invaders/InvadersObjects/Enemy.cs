using System;
using Microsoft.Xna.Framework;
using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.Animators;
using Microsoft.Xna.Framework.Audio;

namespace Invaders.InvadersObjects
{
    public class Enemy : Sprite, ICollidable2D, IShooter
    {
        public static readonly int sr_Width = 32;
        public static readonly int sr_Height = 32;
        private const string k_AssetName = @"Sprites\Enemies";
        private const string k_KillSoundName = @"Sounds\EnemyKill";
        private const string k_GunShootSoundName = @"Sounds\EnemyGunShot";
        private static Random s_Random = new Random();
        private IComander m_Comander;
        private Gun m_Gun;
        private int m_BaseWorth;
        private int m_Worth;
        private int[] m_TextureCellsIdx;

        public event Action<int, IShooter> NotifyDead;

        public event Action TouchedEnd;

        public Enemy(Game i_Game, Color i_Color, int i_Worth, IComander i_Comander, params int[] i_TextureCellsIdx) : base(k_AssetName, i_Game)
        {
            m_BaseWorth = m_Worth = i_Worth;
            m_Comander = i_Comander;
            m_Gun = new Gun(i_Game, this, 1, new Vector2(0, 155), Color.Blue);
            m_TintColor = i_Color;
            m_TextureCellsIdx = i_TextureCellsIdx;
        }

        private CompositeAnimator m_DyingAnimator;
        private SoundEffect m_KillSoundEffect;

        private void initAnimations()
        {
            Animations.Add(new CellAnimator(m_Comander.SecondsBetweenJumps, TimeSpan.Zero, m_TextureCellsIdx));
            ShrinkAnimator shrinkAnimator = new ShrinkAnimator("ShrinkAnimator", TimeSpan.FromSeconds(1.2));
            RotateAnimator rotateAnimator = new RotateAnimator("RotateAnimator", TimeSpan.FromSeconds(1.2), 6);
            m_DyingAnimator = new CompositeAnimator("DieAnimator", TimeSpan.FromSeconds(1.2), this, shrinkAnimator, rotateAnimator);
            m_DyingAnimator.ResetAfterFinish = false;
            Animations.Add(m_DyingAnimator);
            m_DyingAnimator.Finished += remove;
            Animations.Resume();
            m_DyingAnimator.Pause();
        }

        public override void Initialize()
        {
            base.Initialize();
            initAnimations();
            RotationOrigin = SourceRectangleCenter;
            m_KillSoundEffect = Game.Content.Load<SoundEffect>(k_KillSoundName);
            m_Gun.Initialize();
            m_Gun.InitBulletSound(k_GunShootSoundName);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            m_Gun.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            if (m_Comander.TimeToJump)
            {
                if (m_Comander.GoDown)
                {
                    Position += new Vector2(0, Height / 2);
                }
                else
                {
                    Position += new Vector2(m_Comander.Direction * m_Comander.JumpDistance, 0);
                }
            }

            if (this.IsActive && this.Enabled)
            {
                if (m_Comander.TimeToShoot && toShoot())
                {
                    m_Gun.Shoot(new Vector2(Position.X + (SourceRectangle.Width / 2), Position.Y + SourceRectangle.Height));
                }

                if (Position.Y + SourceRectangle.Height > Game.GraphicsDevice.Viewport.Height)
                {
                    TouchedEnd();
                }
            }

            base.Update(gameTime);
            m_Gun.Update(gameTime);
        }

        protected override void InitSourceRectangle()
        {
            base.InitSourceRectangle();

            this.SourceRectangle = new Rectangle(
                m_TextureCellsIdx[0] * sr_Width,
                0,
                sr_Width,
                sr_Height);
        }

        private bool toShoot()
        {
            return s_Random.Next(0, 100) < 3;
        }

        public void UpdateCellAnimator()
        {
            CellAnimator cellAnimator = Animations["CelAnimation"] as CellAnimator;
            if(cellAnimator != null)
            {
                cellAnimator.CellTime = m_Comander.SecondsBetweenJumps;
            }
        }

        public override void Collided(ICollidable i_CollidingObject)
        {
            if (i_CollidingObject is Bullet)
            {
                Bullet collidingBullet = i_CollidingObject as Bullet;
                if (collidingBullet.Shooter is PlayerSpaceship)
                {
                    Animations["DieAnimator"].Resume();
                    m_KillSoundEffect.CreateInstance().Play();
                    NotifyDead.Invoke(m_Worth, collidingBullet.Shooter);
                    IsActive = false;
                    m_Comander.DecreaseOneEnemy(this);
                }
            }
        }

        public void Init(int i_Level)
        {
            m_Worth = m_BaseWorth + (i_Level * 120);
            m_Gun.Init(i_Level);
            m_DyingAnimator.Pause();
            UpdateCellAnimator();
            Animations.Revert();
            IsActive = true;
        }

        private void remove()
        {
            Enabled = false;
            Visible = false;
        }
    }
}
