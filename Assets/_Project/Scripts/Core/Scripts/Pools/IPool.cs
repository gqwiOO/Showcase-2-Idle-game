using System;

namespace Core.Scripts.Pools
{
    public interface IPool
    {
        event EventHandler<IPoolObject> OnPushed;
        event EventHandler<IPoolObject> OnPulled;

        void Push(IPoolObject poolObject);
        IPoolObject Pull();
        bool IsHasObject();
    }
}
