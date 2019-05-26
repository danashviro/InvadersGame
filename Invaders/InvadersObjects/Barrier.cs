using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Infrastructure.ObjectModel;
using Microsoft.Xna.Framework.Audio;

namespace Invaders.InvadersObjects
{
    public class Barrier : Sprite, ICollidable2D 
    {
        private const string k_AssetName = @"Sprites\Barrier_44x32";
        private const string k_HitSoundName = @"Sounds\BarrierHit";
        private const int k_Speed = 45;
        private static Texture2D s_BaseTexture;
        private float m_DistnaceTravelled = 0;
        private float m_DistanceTillChangeDir;
        private SoundEffect m_HitSoundEffect;

        public Barrier(Game i_Game) : base(k_AssetName, i_Game)
        {
        }

        public override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);
            m_DistnaceTravelled += Math.Abs(Velocity.X * (float)i_GameTime.ElapsedGameTime.TotalSeconds);
            if (m_DistnaceTravelled >= m_DistanceTillChangeDir)
            {
                Velocity *= -1;
                m_DistnaceTravelled -= m_DistanceTillChangeDir;
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            m_DistanceTillChangeDir = Width / 2;
            m_HitSoundEffect = Game.Content.Load<SoundEffect>(k_HitSoundName);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            if (s_BaseTexture == null)
            {
                s_BaseTexture = Texture;
            }

            resetTexture();
        }

        private void resetTexture()
        {
            Texture = s_BaseTexture;
            InitPixels();
            Texture2D texture = new Texture2D(Game.GraphicsDevice, Texture.Width, Texture.Height);
            texture.SetData(Pixels);
            Texture = texture;
        }

        public override void Collided(ICollidable i_Collidable)
        {
            if (i_Collidable is Bullet)
            {
                collideWithBullet(i_Collidable as Bullet);
            }
            else if (i_Collidable is Enemy)
            {
                collideWithEnemy(i_Collidable as Enemy);
            }
        }

        private void collideWithEnemy(Enemy i_Enemy)
        {
            deleteSharedPixels(i_Enemy.Bounds, i_Enemy.Pixels);
        }

        private void collideWithBullet(Bullet i_Bullet)
        {
            int y = i_Bullet.Bounds.Y + (int)(Math.Sign(i_Bullet.Velocity.Y) * i_Bullet.Bounds.Height * 0.7);
            Rectangle bulletRectangle = new Rectangle(i_Bullet.Bounds.X, y, i_Bullet.Bounds.Width, i_Bullet.Bounds.Height);
            deleteSharedPixels(bulletRectangle, i_Bullet.Pixels);
        }

        protected override bool deleteSharedPixels(Rectangle i_SourceBounds, Color[] i_SourcePixels)
        {
            bool pixelCollided = base.deleteSharedPixels(i_SourceBounds, i_SourcePixels);
            if (pixelCollided)
            {
                m_HitSoundEffect.CreateInstance().Play();
            }

            return pixelCollided;
        }

        public void InitByLevel(int i_Level)
        {
            resetTexture();
            if (i_Level == 0)
            {
                Velocity = Vector2.Zero;
            }
            else
            {
                Velocity = new Vector2(k_Speed - ((k_Speed * 7f * (i_Level - 1)) / 100f), 0);
            }
        }
    }
}
