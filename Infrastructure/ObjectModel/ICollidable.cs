using System;

namespace Infrastructure.ObjectModel
{
    public interface ICollidable
    {
        event EventHandler<EventArgs> PositionChanged;

        event EventHandler<EventArgs> SizeChanged;

        event EventHandler<EventArgs> VisibleChanged;

        event EventHandler<EventArgs> Disposed;

        bool CheckCollision(ICollidable i_Source);

        void Collided(ICollidable i_Collidable);

        bool Visible { get; }

        bool IsActive { get; }
    }
}
