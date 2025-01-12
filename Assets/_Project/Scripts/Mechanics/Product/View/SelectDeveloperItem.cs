using System;
using Mechanics.Characters;
using Mechanics.Characters.Views;
using UI.Buttons;
using UnityEngine;

namespace Mechanics.Product
{
    public class SelectDeveloperItem: MonoBehaviour
    {
        [SerializeField] 
        private UIButton _selectButton;
        [SerializeField] 
        private CharacterViewsAdapter _characterViewsAdapter;
        
        public ICharacterData CharacterData { get; private set; }
        public event Action<ICharacterData> OnSelected;


        private void Start() => _selectButton.OnClicked += SelectButton_OnClicked;
        private void OnDestroy() => _selectButton.OnClicked -= SelectButton_OnClicked;

        private void SelectButton_OnClicked() => OnSelected?.Invoke(CharacterData);
        
        public void Init(ICharacterData characterData)
        {
            CharacterData = characterData;
            
            _characterViewsAdapter.Init(characterData);
            _characterViewsAdapter.UpdateView();
        }   
    }
}