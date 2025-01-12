using System;
using UI.Buttons;
using UnityEngine;

namespace UI.Toggles
{
    public abstract class BaseToggle: MonoBehaviour
    {
        [SerializeField] private UIButton _button;

        private bool _enabled;
        
        public event EventHandler OnButtonClicked;
        public event Action OnValidClick;

        private void Start()
        {
            _button.OnClicked += Button_OnClicked;
        }

        private void Button_OnClicked() =>
            OnButtonClicked?.Invoke(this,null);

        protected bool CanEnable() => !_enabled;
        protected bool CanDisable() => _enabled;

        public void PerformEnable()
        {
            if (!_enabled)
            {
                _enabled = true;
                PerformOnEnable();
                OnValidClick?.Invoke();
            }
        }

        protected abstract void PerformOnEnable();
        protected abstract void PerformOnDisable();
        
        public void PerformDisable()
        {
            if (_enabled)
            {
                _enabled = false;
                PerformOnDisable();
            }
        }

    }
}