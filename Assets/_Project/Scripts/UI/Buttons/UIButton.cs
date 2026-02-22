using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Buttons
{
    public class UIButton: MonoBehaviour
    {
        [SerializeField] private Button button;

        [Header("Properties")]
        
        [SerializeField] 
        private bool playSound = true;
         
        public event Action OnClicked; 

        private void OnValidate() => GetButton();

        [Button]
        private void GetButton() 
            => button ??= GetComponent<Button>();

        private void Start() 
            => button.onClick.AddListener(Button_OnClicked);

        private void Button_OnClicked()
        {
            ClickHook();
            OnClicked?.Invoke();
        }

        public void SetInteractableState(bool state) 
            => button.interactable = state;

        protected virtual void ClickHook() { }
    }
}