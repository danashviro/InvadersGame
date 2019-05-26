using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Infrastructure.ObjectModel;
using Microsoft.Xna.Framework.Audio;

namespace Invaders.InvadersObjects
{
    public class Gun : CompositeDrawableComponent<Bullet>
    {
        private int m_MaxNumOfBullets;
        private IShooter m_Shooter;
        private Color m_BulletsColor;
        private Vector2 m_BulletsVelocity;
        private LinkedList<Bullet> m_AliveBullets, m_NonActiveBullet;
        private SoundEffect m_GunSoundEffect;

        public Gun(Game i_Game, IShooter i_Shooter, int i_MaxNumOfBullets, Vector2 i_BulletsVelocity, Color i_BulletsColor) : base(i_Game)
        {
            m_BulletsVelocity = i_BulletsVelocity;
            m_MaxNumOfBullets = i_MaxNumOfBullets;
            m_Shooter = i_Shooter;
            m_BulletsColor = i_BulletsColor;
            m_AliveBullets = new LinkedList<Bullet>();
            m_NonActiveBullet = new LinkedList<Bullet>();
        }

        public void Shoot(Vector2 i_BulletPosition)
        {
            if (m_MaxNumOfBullets > m_AliveBullets.Count)
            {
                Bullet bullet;
                if (m_NonActiveBullet.Count != 0)
                {
                    bullet = m_NonActiveBullet.First.Value;
                    m_NonActiveBullet.Remove(bullet);
                    bullet.init(i_BulletPosition);
                }
                else
                {
                    bullet = new Bullet(Game, m_Shooter, m_BulletsVelocity, i_BulletPosition, m_BulletsColor);
                    bullet.NotifyDead += removeBullet;
                    Add(bullet);
                }

                m_GunSoundEffect.CreateInstance().Play();
                m_AliveBullets.AddLast(bullet);
            }
        }

        private void removeBullet(Bullet i_BulletToRemove)
        {
            m_NonActiveBullet.AddLast(i_BulletToRemove);
            m_AliveBullets.Remove(i_BulletToRemove);
        }

        public void InitBulletSound(string i_SoundName)
        {
            m_GunSoundEffect = Game.Content.Load<SoundEffect>(i_SoundName);
        }

        public void Init(int i_Level)
        {
            m_MaxNumOfBullets = i_Level + 1;
            Init();
        }

        public void Init()
        {
            foreach (Bullet bullet in m_AliveBullets)
            {
                bullet.Disable();
                m_NonActiveBullet.AddLast(bullet);
            }

            m_AliveBullets.Clear();
        }
    }
}