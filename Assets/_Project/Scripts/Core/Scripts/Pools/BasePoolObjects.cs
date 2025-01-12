using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Scripts.Pools
{
    public abstract class BasePoolObjects : MonoBehaviour, IPool
    {
        [SerializeField]
        private string _key;
        [SerializeField]

        private List<IPoolObject> _pulledObjects = new List<IPoolObject>();

        public abstract event EventHandler<IPoolObject> OnPushed;
        public abstract event EventHandler<IPoolObject> OnPulled;

        public string Key { get => _key; }
        public List<IPoolObject> PulledObjects { get => _pulledObjects; }

        public abstract void Init();
        public abstract IPoolObject Pull();
        public abstract void Push(IPoolObject poolObject);
        public abstract bool IsHasObject();

        protected virtual void AddPulledObjects(IPoolObject poolObject)
        {
            _pulledObjects.Add(poolObject);
        }

        protected virtual void RemovePulledObjects(IPoolObject poolObject)
        {
            _pulledObjects.Remove(poolObject);
        }

        public abstract IPoolObject GetOriginalObject();
    }
}
