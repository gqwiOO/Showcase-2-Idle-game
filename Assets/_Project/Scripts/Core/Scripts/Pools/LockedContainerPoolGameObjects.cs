using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Core.Scripts.Pools
{
    public class LockedContainerPoolGameObjects : BasePoolObjects
    {
        [SerializeField]
        private List<PoolObject> _originalsObjects;
        [SerializeField]
        private Transform _container;

        private List<IPoolObject> _containObjects;

        public override event EventHandler<IPoolObject> OnPushed;
        public override event EventHandler<IPoolObject> OnPulled;

        private DiContainer _diContainer;

        [Inject]
        public void Construct(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }

        public override void Init()
        {
            _containObjects = new List<IPoolObject>();
            CreatePoolObjects();
        }

        public override bool IsHasObject()
        {
            return _containObjects.Count > 0;
        }

        public override IPoolObject Pull()
        {
            IPoolObject result = default;
            if (IsHasObject() == true)
            {
                result = _containObjects[UnityEngine.Random.Range(0, _containObjects.Count)];

                AddPulledObjects(result as PoolObject);
                _containObjects.Remove(result);

                result.GetOwner().transform.SetParent(null);

                OnPulled?.Invoke(this, result);
            }

            return result;
        }

        public override void Push(IPoolObject poolObject)
        {
            if (PulledObjects?.Contains(poolObject) == true)
            {
                RemovePulledObjects(poolObject as PoolObject);
            }

            poolObject.GetOwner().SetActive(false);
            poolObject.GetOwner().transform.SetParent(_container.transform);

            _containObjects.Add(poolObject);
            OnPushed?.Invoke(this, poolObject);
        }


        protected virtual PoolObject InstantiateObject(PoolObject prefab, Transform parent)
        {
            PoolObject result = null;
            try
            {
                result = _diContainer.InstantiatePrefab(prefab, parent).GetComponent<PoolObject>();
            }
            catch (Exception ex)
            {
                Debug.LogError(gameObject.name);
                result = Instantiate(prefab, parent);
            }
            result.Initialize(this);
            return result;
        }

        private void CreatePoolObjects()
        {
            _originalsObjects?.ForEach(x =>
            {
                CreatePoolObject(x);
            });
        }

        private void CreatePoolObject(PoolObject prefab)
        {
            PoolObject spawned = InstantiateObject(prefab, _container.transform);
            Push(spawned);
        }

        public override IPoolObject GetOriginalObject()
        {
            return _originalsObjects[0];
        }
    }
}