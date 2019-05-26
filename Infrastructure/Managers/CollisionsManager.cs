//// Guy Ronen © 2008-2011
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Infrastructure.ObjectModel;

namespace Infrastructure.Managers
{
    public class CollisionsManager : GameService, ICollisionsManager
    {
        protected readonly List<ICollidable> m_Collidables = new List<ICollidable>();

        public CollisionsManager(Game i_Game) : 
            base(i_Game, int.MaxValue)
        {
        } 

        protected override void RegisterAsService()
        {
            Game.Services.AddService(typeof(ICollisionsManager), this);
        }

        public void AddObjectToMonitor(ICollidable i_Collidable)
        {
            if (!this.m_Collidables.Contains(i_Collidable))
            {
                m_Collidables.Add(i_Collidable);
                i_Collidable.PositionChanged += collidable_Changed;
                i_Collidable.SizeChanged += collidable_Changed;
                i_Collidable.VisibleChanged += collidable_Changed;
                i_Collidable.Disposed += collidable_Disposed;
            }
        }

        private void collidable_Disposed(object sender, EventArgs e)
        {
            ICollidable collidable = sender as ICollidable;

            if (collidable != null
                &&
                this.m_Collidables.Contains(collidable))
            {
                collidable.PositionChanged -= collidable_Changed;
                collidable.SizeChanged -= collidable_Changed;
                collidable.VisibleChanged -= collidable_Changed;
                collidable.Disposed -= collidable_Disposed;
                m_Collidables.Remove(collidable);
            }
        }

        private void collidable_Changed(object sender, EventArgs e)
        {
            if (sender is ICollidable)
            {
                checkCollision(sender as ICollidable);
            }
        }
        
        private void checkCollision(ICollidable i_Source)
        {
            if (i_Source.Visible && i_Source.IsActive) 
            {
                List<ICollidable> collidedComponents = new List<ICollidable>();

                foreach (ICollidable target in m_Collidables)
                {
                    if (i_Source != target && target.Visible && target.IsActive) 
                    {
                        if (i_Source.CheckCollision(target))
                        {
                            collidedComponents.Add(target);
                        }
                    }
                }

                foreach (ICollidable target in collidedComponents)
                {
                    if (i_Source.Visible && i_Source.IsActive) 
                    {
                        i_Source.Collided(target);
                        target.Collided(i_Source);
                    }
                }
            }
        }
    }
}
