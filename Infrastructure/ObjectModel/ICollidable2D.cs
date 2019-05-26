using Microsoft.Xna.Framework;

namespace Infrastructure.ObjectModel
{
    public interface ICollidable2D : ICollidable
    {
        Rectangle Bounds { get; }

        Vector2 Velocity { get; }

        Color[] Pixels { get; }
    }
}
