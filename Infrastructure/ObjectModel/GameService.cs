//// Guy Ronen © 2008-2011
using Microsoft.Xna.Framework;

namespace Infrastructure.ObjectModel
{
    public abstract class GameService : RegisteredComponent
    {
        public GameService(Game i_Game, int i_UpdateOrder)
            : base(i_Game, i_UpdateOrder)
        {
            RegisterAsService(); 
        }

        public GameService(Game i_Game)
            : base(i_Game)
        {
            RegisterAsService();
        }

        protected virtual void RegisterAsService()
        {
            Game.Services.AddService(this.GetType(), this);
        }
    }
}
