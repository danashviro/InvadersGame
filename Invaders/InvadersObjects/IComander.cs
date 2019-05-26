using System;

namespace Invaders.InvadersObjects
{
    public interface IComander
    {
        bool TimeToJump { get; }

        bool TimeToShoot { get; }

        bool GoDown { get; }

        int Direction { get; }

        float JumpDistance { get; }

        TimeSpan SecondsBetweenJumps { get; }

        void DecreaseOneEnemy(Enemy i_Enemy);
    }
}
