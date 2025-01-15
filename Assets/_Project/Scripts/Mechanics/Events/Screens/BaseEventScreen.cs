using System;
using Cysharp.Threading.Tasks;
using Services.Screen;
using UI.Buttons;
using UnityEngine;

namespace Mechanics.Events.Screens
{
    public abstract class BaseEventScreen<TEventData>: BaseScreen where TEventData : BaseEventData
    {
        [SerializeField] private UIButton _closeButton;
        
        protected TEventData _data;

        protected virtual void Start()
        {
            _closeButton.OnClicked += OnCloseButton;
        }

        public virtual async UniTask Init(TEventData data)
        {
            _data = data;
            InitView();
        }
        
        private void OnCloseButton() => Hide().Forget();

        protected abstract void InitView();
        protected virtual void OnDestroy() => _closeButton.OnClicked -= OnCloseButton;
    }
}