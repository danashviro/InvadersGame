//// Guy Ronen © 2008-2011 
using Infrastructure.ObjectModel;

namespace Infrastructure.Managers
{
    public interface ICollisionsManager
    {
        void AddObjectToMonitor(ICollidable i_Collidable);
    }
}
