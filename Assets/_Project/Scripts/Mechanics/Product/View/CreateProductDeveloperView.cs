using System;
using Mechanics.Characters;
using Mechanics.Characters.Views;
using UI.Buttons;
using UnityEngine;

namespace Mechanics.Product
{
    public class CreateProductDeveloperView: MonoBehaviour
    {
        [SerializeField] private UIButton _unselectedDeveloper;
        [SerializeField] private CharacterViewsAdapter _characterViewsAdapter;
        
        public ICharacterData CharacterData { get; private set; }


        public event Action<ICharacterData> OnDeveloperUnselected;

        private void Start() => _unselectedDeveloper.OnClicked += UnselectedDeveloper_OnClicked;

        private void OnDestroy() => _unselectedDeveloper.OnClicked -= UnselectedDeveloper_OnClicked;

        private void UnselectedDeveloper_OnClicked() => OnDeveloperUnselected?.Invoke(CharacterData);

        public void Init(ICharacterData characterData)
        {
            CharacterData = characterData;
            _characterViewsAdapter.Init(characterData);
            _characterViewsAdapter.UpdateView();
        }
    }
}