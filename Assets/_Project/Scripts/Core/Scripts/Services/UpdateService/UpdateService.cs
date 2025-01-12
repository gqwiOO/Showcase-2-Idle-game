using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Scripts.Services.UpdateService
{
    public class UpdateService : MonoBehaviour, IUpdateService
    {
        private readonly HashSet<IUpdatable> _updateUpdatableItems = new ();
        private readonly HashSet<IUpdatable> _fixedUpdatableItems = new ();
        private readonly HashSet<IUpdatable> _lateUpdatableItems = new ();
        
        public void Update()
        {
            foreach (IUpdatable item in _updateUpdatableItems)
                item.Tick(Time.deltaTime);
        }

        private void LateUpdate()
        {
            foreach (IUpdatable item in _lateUpdatableItems)
                item.Tick(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            foreach (IUpdatable item in _fixedUpdatableItems)
                item.Tick(Time.deltaTime);
        }

        public void Add(IUpdatable updatable) => GetHashSetByUpdateType(updatable.UpdateType).Add(updatable);

        private HashSet<IUpdatable> GetHashSetByUpdateType(UpdateType updateType)
        {
            switch (updateType)
            {
                case UpdateType.Update:
                    return _updateUpdatableItems;
                case UpdateType.LateUpdate:
                    return _lateUpdatableItems;
                case UpdateType.FixedUpdate:
                    return _fixedUpdatableItems;
                default:
                    throw new ArgumentOutOfRangeException(nameof(updateType), updateType, null);
            }
        }

        public void Remove(IUpdatable updatable) 
            => _updateUpdatableItems.Remove(updatable);
    }
}