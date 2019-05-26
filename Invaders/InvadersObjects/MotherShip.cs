using System;
using Microsoft.Xna.Framework;
using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.Animators;
using Microsoft.Xna.Framework.Audio;

namespace Invaders.InvadersObjects
{
    public class MotherShip : Sprite, ICollidable2D
    {
        private const int k_Speed = 110;
        private const int k_Worth = 850;
        private const string k_AssetName = @"Sprites\MotherShip_32x120";
        private const string k_KillSoundName = @"Sounds\MotherShipKill";
        private TimeSpan m_TimeBetweenSpawns = TimeSpan.FromSeconds(1f);
        private TimeSpan m_TimeUntilNextSpawn;
        private Random m_Random;
        private bool m_IsInGame = false;
        private SoundEffect m_KillSoundEffect;

        public event Action<int, IShooter> NotifyDead;

        public MotherShip(Game i_Game) : base(k_AssetName, i_Game)
        {
            IsActive = false;
            m_Random = new Random();
            m_TimeUntilNextSpawn = m_TimeBetweenSpawns;
            Visible = false;
        }

        public override void Initialize()
        {
            UseNonPremultipliedSpriteBatch();
            base.Initialize();
            initAnimations();
            RotationOrigin = SourceRectangleCenter;
            m_KillSoundEffect = Game.Content.Load<SoundEffect>(k_KillSoundName);
        }

        private void initAnimations()
        {
            CompositeAnimator dyingAnimator = new CompositeAnimator(
                "dyingAnimator",
                TimeSpan.FromSeconds(2.2),
                this,
                new FadeAnimator("fader", TimeSpan.FromSeconds(2.2)),
                new ShrinkAnimator("shrinker", TimeSpan.FromSeconds(2.2)),
                new BlinkAnimator("blinker", 6, TimeSpan.FromSeconds(2.2)));
            dyingAnimator.Finished += new Action(() => { m_IsInGame = false; Visible = false; });
            Animations.Add(dyingAnimator);
            dyingAnimator.Pause();
            Animations.Resume();
        }

        public override void Update(GameTime gameTime)
        {
            if (!m_IsInGame)
            {
                m_TimeUntilNextSpawn -= gameTime.ElapsedGameTime;
                if (m_TimeUntilNextSpawn.TotalSeconds < 0)
                {
                    m_TimeUntilNextSpawn = m_TimeBetweenSpawns;
                    if (shouldAppear())
                    {
                        appear();
                    }
                }
            }
            else if(IsActive)
            {
                if (Position.X > Game.GraphicsDevice.Viewport.Width)
                {
                    IsActive = false;
                    m_IsInGame = false;
                    Visible = false;
                }
            }

            base.Update(gameTime);
        }

        private void appear()
        {
            m_Velocity = new Vector2(k_Speed, 0);
            IsActive = true;
            Visible = true;
            m_IsInGame = true;
            Position = new Vector2(0f - Texture.Width, Texture.Height);
        }

        private bool shouldAppear()
        {
            return m_Random.Next(0, 100) < 20;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public override void Collided(ICollidable i_CollidingObject)
        {
            if (i_CollidingObject is Bullet)
            {
                Bullet bullet = i_CollidingObject as Bullet;
                m_KillSoundEffect.CreateInstance().Play();
                NotifyDead(k_Worth, bullet.Shooter);
                IsActive = false;
                Velocity = Vector2.Zero;
                Animations["dyingAnimator"].Restart();
            }
        }
    }
}
