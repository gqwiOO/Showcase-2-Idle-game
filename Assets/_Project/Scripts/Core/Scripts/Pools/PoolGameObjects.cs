using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Core.Scripts.Pools
{
    public class PoolGameObjects : BasePoolObjects
    {
        [SerializeField]
        private PoolObject _originalObject;
        [SerializeField]
        private int _startSizePool;
        [SerializeField]
        private Transform _container;
        [SerializeField]
        private bool _isLockedPool;

        [Header("ReadOnly")]
        [SerializeField]
        private bool _isInited;

        private Queue<IPoolObject> _objectQueue;

        private DiContainer _diContainer;

        public override event EventHandler<IPoolObject> OnPushed;
        public override event EventHandler<IPoolObject> OnPulled;

        public Queue<IPoolObject> ObjectQueue { get => _objectQueue; }
        public PoolObject OriginalObject { get => _originalObject; }
        public bool IsInited { get => _isInited; }

        [Inject]
        public void Construct(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }

        private void Awake()
        {
            if (_isInited)
            {
                AddPreloadedObjectsToPool();
            }
        }

        private void Reset()
        {
            _container = transform;
        }

        private void AddPreloadedObjectsToPool()
        {
            _objectQueue = new Queue<IPoolObject>();
            
            foreach (Transform children in _container.transform)
            {
                var obj = children.GetComponent<IPoolObject>();
                if (obj != null)
                {
                    obj.Initialize(this);
                    _objectQueue.Enqueue(obj);
                }
            }
        }

        public virtual void Init(PoolObject originalObject)
        {
            SetOriginalObject(originalObject);
            Init();
        }

        [Button]
        public override void Init()
        {
            if (_isInited == false)
            {
                _objectQueue = new Queue<IPoolObject>();

                for (int i = 0; i < _startSizePool; i++)
                {
                    CreatePoolObject();
                }
                _isInited = true;
            }

            if (_objectQueue == null)
            {
                AddPreloadedObjectsToPool();
            }
        }


        public override void Push(IPoolObject poolObject)
        {
            if (poolObject == null || poolObject.GetOwner() == null)
            {
                return;
            }

            if (PulledObjects?.Contains(poolObject) == true)
            {
               RemovePulledObjects(poolObject as PoolObject);
            }

            if (ObjectQueue.Contains(poolObject) == false)
            {
                poolObject.GetOwner()?.SetActive(false);
                poolObject.GetOwner()?.transform?.SetParent(_container?.transform);
                poolObject.Initialize(this);

                ObjectQueue.Enqueue(poolObject);

                OnPushed?.Invoke(this, poolObject);
            }
        }
        public override IPoolObject Pull()
        {
            IPoolObject result = default;
            if (IsHasObject() == true)
            {
                if (ObjectQueue.Count == 0)
                {
                    CreatePoolObject();
                }

                result = ObjectQueue.Dequeue();

                AddPulledObjects(result as PoolObject);

                result.GetOwner().transform.SetParent(null);

                OnPulled?.Invoke(this, result);
            }

            return result;

        }
        private void CreatePoolObject()
        {
            PoolObject spawned = InstantiateObject(_originalObject, _container.transform);
            Push(spawned);
        }

        protected virtual PoolObject InstantiateObject(PoolObject prefab, Transform parent)
        {
            PoolObject result = null;
            try
            {
                result = _diContainer.InstantiatePrefab(prefab, parent).GetComponent<PoolObject>();
            }
            catch(Exception ex)
            {
                Debug.LogError(gameObject.name);
                result = Instantiate(prefab, parent);
            }
            result.Initialize(this);
            return result;
        }

        protected void SetOriginalObject(PoolObject originalObject)
        {
            _originalObject = originalObject;
        }

        protected void SetSizePool(int value)
        {
            _startSizePool = value;
        }

        public override bool IsHasObject()
        {
            bool result = true;

            if (_isLockedPool == true)
            {
                if (_objectQueue.Count == 0)
                {
                    result = false;
                }
            }

            return result;
        }

        public override IPoolObject GetOriginalObject()
        {
            return _originalObject;
        }
    }
}
