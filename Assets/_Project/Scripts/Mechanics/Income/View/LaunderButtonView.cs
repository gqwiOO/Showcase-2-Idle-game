using Cysharp.Threading.Tasks;
using Mechanics.Income.Service;
using Services.Screen.Interfaces;
using UI.Buttons;
using UnityEngine;
using Zenject;

namespace Mechanics.Income.View
{
    public class LaunderButtonView : MonoBehaviour
    {
        [SerializeField] private UIButton _button;

        private ILaunderService _launderService;
        private IMainScreenService _mainScreenService;

        [Inject]
        private void Construct(ILaunderService launderService, IMainScreenService mainScreenService)
        {
            _launderService = launderService;
            _mainScreenService = mainScreenService;
        }

        private void Start()
        {
            _button.OnClicked += OnClicked;
            _launderService.OnMaxAmountChanged += UpdateInteractable;
            UpdateInteractable();
        }

        private void OnDestroy()
        {
            _button.OnClicked -= OnClicked;
            _launderService.OnMaxAmountChanged -= UpdateInteractable;
        }

        private void UpdateInteractable() =>
            _button.SetInteractableState(_launderService.GetMaxLaunderAmount() > 0);

        private void OnClicked() => _mainScreenService.ShowLaunderMoneyScreen().Forget();
    }
}
