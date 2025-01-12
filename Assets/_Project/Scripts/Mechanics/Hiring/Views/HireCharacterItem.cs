using System;
using Mechanics.Characters;
using Mechanics.Characters.Views;
using UI.Buttons;
using UnityEngine;

namespace Mechanics.Hiring.Views
{
    public class HireCharacterItem: MonoBehaviour
    {
        [SerializeField] private CharacterViewsAdapter _characterViewsAdapter;
        [SerializeField] private UIButton _hireButton;
        
        private ICharacterData _characterData;

        public string CharacterKey => _characterData.Key;

        public event Action<ICharacterData> OnHire; 
        
        public void Init(ICharacterData characterData)
        {
            _characterData = characterData;
            _characterViewsAdapter.Init(characterData);
            _characterViewsAdapter.UpdateView();
        }

        private void Start() => _hireButton.OnClicked += HireButton_OnClicked;

        private void OnDestroy() => _hireButton.OnClicked -= HireButton_OnClicked;

        private void HireButton_OnClicked()
        {
            OnHire?.Invoke(_characterData);
        }
    }
}