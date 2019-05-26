using System;
using Microsoft.Xna.Framework;
using Infrastructure.ObjectModel;

namespace Invaders.InvadersObjects
{
    public class Bullet : Sprite, ICollidable2D
    {
        private const string k_AssetName = @"Sprites\Bullet";
        private static Random s_Random = new Random();

        public event Action<Bullet> NotifyDead;

        public IShooter Shooter { get; private set; }

        public Bullet(Game i_Game, IShooter i_Shooter, Vector2 i_Velocity, Vector2 i_Position, Color i_Color) : base(k_AssetName, i_Game, i_Position)
        {
            Shooter = i_Shooter;
            m_TintColor = i_Color;
            m_Velocity = i_Velocity;
        }

        public void init(Vector2 i_Position)
        {
            Position = fixPosition(i_Position);
            Enabled = true;
            IsActive = true;
            Visible = true;
            InitBounds();
        }

        private Vector2 fixPosition(Vector2 i_Position)
        {
            i_Position.X -= Width / 2;
            if (Velocity.Y < 0)
            {
                i_Position.Y -= Height;
            }

            return i_Position;
        }

        protected override void InitBounds()
        {
            base.InitBounds();
            m_Position = fixPosition(m_Position);
        }

        public override void Collided(ICollidable i_CollidingObject)
        {
            if (hitEnemy(i_CollidingObject) || (hitEnemyBullet(i_CollidingObject) && checkIfDestroy()) || (i_CollidingObject is Barrier)) 
            {
                disableAndNotify();
            }
        }

        private bool checkIfDestroy()
        {
            return s_Random.Next(0, 100) > 50;
        }

        private bool hitEnemy(ICollidable i_CollidingObject)
        {
            return (Shooter is PlayerSpaceship && (i_CollidingObject is MotherShip || i_CollidingObject is Enemy)) || (Shooter is Enemy && i_CollidingObject is PlayerSpaceship);
        }

        private bool hitEnemyBullet(ICollidable i_CollidingObject)
        {
            bool hitBullet = false;
            if (i_CollidingObject is Bullet)
            {
                IShooter collidingBulletShooter = (i_CollidingObject as Bullet).Shooter;
                hitBullet = (collidingBulletShooter is PlayerSpaceship && Shooter is Enemy) || (collidingBulletShooter is Enemy && Shooter is PlayerSpaceship);
            }

            return hitBullet;
        }

        private void disableAndNotify()
        {
            Disable();
            NotifyDead(this);
        }

        public void Disable()
        {
            Enabled = false;
            IsActive = false;
            Visible = false;
        }

        public override void Update(GameTime gameTime)
        {
            if (!Game.GraphicsDevice.Viewport.Bounds.Intersects(Bounds))
            {
                disableAndNotify();
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}