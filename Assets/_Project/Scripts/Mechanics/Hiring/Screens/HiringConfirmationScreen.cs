using Cysharp.Threading.Tasks;
using Mechanics.Characters;
using Mechanics.Characters.Views;
using Mechanics.Hiring.Service;
using Services.Screen;
using UI.Buttons;
using UnityEngine;
using Zenject;

namespace Mechanics.Hiring.Screens
{
    public class HiringConfirmationScreen: BaseScreen
    {
        [SerializeField] private CharacterViewsAdapter _characterViewsAdapter;

        [SerializeField] private UIButton _confirmButton;
        [SerializeField] private UIButton _closeButton;
        private IHiringService _hiringService;
        private ICharacterData _characterData;


        [Inject]
        private void Construct(IHiringService hiringService)
        {
            _hiringService = hiringService;
        }

        public async UniTask Init(ICharacterData characterData)
        {
            _characterData = characterData;
            _characterViewsAdapter.Init(characterData);
            _characterViewsAdapter.UpdateView();
        }

        private void Start()
        {
            _confirmButton.OnClicked += ConfirmButton_OnClicked;
            _closeButton.OnClicked += CloseButton_OnClicked;
        }

        private void CloseButton_OnClicked() 
            => Hide().Forget();

        private void OnDestroy()
        {
            _confirmButton.OnClicked -= ConfirmButton_OnClicked;
            _closeButton.OnClicked -= CloseButton_OnClicked;
        }

        private void ConfirmButton_OnClicked()
        {
            _hiringService.Hire(_characterData);
            Hide().Forget();
        }
    }
}