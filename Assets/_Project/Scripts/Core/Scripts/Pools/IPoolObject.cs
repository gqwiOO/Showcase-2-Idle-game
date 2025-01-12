using System;
using UnityEngine;

namespace Core.Scripts.Pools
{
    public interface IPoolObject
    {
        event EventHandler<IPoolObject> OnPushed;

        IPool Pool { get; }
        void Initialize(IPool pool);
        void Push();

        T GetOwner<T>();
        GameObject GetOwner();
    }
}
