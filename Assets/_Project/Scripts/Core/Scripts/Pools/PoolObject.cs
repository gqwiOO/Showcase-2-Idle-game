using System;
using UnityEngine;

namespace Core.Scripts.Pools
{


    public class PoolObject : MonoBehaviour, IPoolObject
    {
        [SerializeField]
        private GameObject _owner;

        private IPool _pool;

        public event EventHandler<IPoolObject> OnPushed;

        public IPool Pool { get => _pool; }

        public virtual void Initialize(IPool pool)
        {
            _pool = pool;
        }

        public void Push()
        {
            Pool?.Push(this);
            OnPushed?.Invoke(this, this);
        }

        public T GetOwner<T>()
        {
            return _owner.GetComponent<T>();
        }

        public GameObject GetOwner()
        {
            return _owner;
        }
    }
}
