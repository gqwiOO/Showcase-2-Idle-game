using System;
using Mechanics.Characters;
using Mechanics.Characters.Views;
using UI.Buttons;
using UnityEngine;

namespace Mechanics.Companies
{
    public class CompanyEmployeeView: MonoBehaviour
    {
        [SerializeField] private CharacterViewsAdapter _characterViewsAdapter;

        [SerializeField] private UIButton _manageEmployeeButton;
        
        private ICharacterData _characterData;

        public event Action<ICharacterData> OnManageClicked;

        public void Init(ICharacterData characterData)
        {
            _characterData = characterData;
            
            _characterViewsAdapter.Init(_characterData);
            _characterViewsAdapter.UpdateView();
        }
        
        private void Start()
        {
            _manageEmployeeButton.OnClicked += ManageEmployeeButton_OnClicked;
        }

        private void OnDestroy()
        {
            _manageEmployeeButton.OnClicked -= ManageEmployeeButton_OnClicked;
        }

        private void ManageEmployeeButton_OnClicked() 
            => OnManageClicked?.Invoke(_characterData);
    }
}