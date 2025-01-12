using Services.Screen.Interfaces;
using UI.Buttons;
using UnityEngine;
using Zenject;

namespace Mechanics.CompaniesRating.View
{
    public class OpenCompanyRatingScreenButton: MonoBehaviour
    {
        [SerializeField] private UIButton _button;
        private IMainScreenService _mainScreenService;


        [Inject]
        private void Construct(IMainScreenService mainScreenService)
        {
            _mainScreenService = mainScreenService;
        }
        
        private void Start() => _button.OnClicked += Button_OnClicked;
        private void OnDestroy() => _button.OnClicked -= Button_OnClicked;
        private void Button_OnClicked() 
            => _mainScreenService.ShowCompaniesRatingScreen();
    }
}