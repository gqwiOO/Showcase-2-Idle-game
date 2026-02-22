using Cysharp.Threading.Tasks;
using Mechanics.Income.Service;
using Services.Screen;
using TMPro;
using UI.Buttons;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Mechanics.Income.Screens
{
    public class LaunderMoneyScreen : BaseScreen
    {
        [SerializeField] private Slider _amountSlider;
        [SerializeField] private TMP_Text _amountText;
        [SerializeField] private TMP_Text _cleanMoneyResultText;
        [SerializeField] private UIButton _launderButton;
        [SerializeField] private UIButton _closeButton;

        private ILaunderService _launderService;

        [Inject]
        private void Construct(ILaunderService launderService)
        {
            _launderService = launderService;
        }

        private void Start()
        {
            _launderButton.OnClicked += OnLaunderClicked;
            _closeButton.OnClicked += OnCloseClicked;
        }

        private void OnEnable()
        {
            RefreshFromService();
            if (_amountSlider != null)
                _amountSlider.onValueChanged.AddListener(OnSliderChanged);
            _launderService.OnMaxAmountChanged += RefreshFromService;
        }

        private void OnDisable()
        {
            if (_amountSlider != null)
                _amountSlider.onValueChanged.RemoveListener(OnSliderChanged);
            _launderService.OnMaxAmountChanged -= RefreshFromService;
        }

        private void OnDestroy()
        {
            _launderButton.OnClicked -= OnLaunderClicked;
            _closeButton.OnClicked -= OnCloseClicked;
        }

        private void RefreshFromService()
        {
            if (_amountSlider == null) return;
            var max = _launderService.GetMaxLaunderAmount();
            _amountSlider.minValue = 0f;
            _amountSlider.maxValue = Mathf.Max(0f, max);
            _amountSlider.value = _amountSlider.maxValue;
            UpdateAmountDisplay((int)_amountSlider.value);
            UpdateLaunderButton();
        }

        private void OnSliderChanged(float value)
        {
            UpdateAmountDisplay((int)value);
            UpdateLaunderButton();
        }

        private void UpdateAmountDisplay(int amount)
        {
            if (_amountText != null)
                _amountText.text = $"{amount}$";
            if (_cleanMoneyResultText != null)
                _cleanMoneyResultText.text = $"Clean: +{amount}$";
        }

        private void UpdateLaunderButton() =>
            _launderButton.SetInteractableState(_launderService.CanLaunder((float)(int)_amountSlider.value));

        private void OnLaunderClicked()
        {
            var amount = (float)(int)_amountSlider.value;
            if (_launderService.CanLaunder(amount))
            {
                _launderService.Launder(amount);
                Hide().Forget();
            }
        }

        private void OnCloseClicked() => Hide().Forget();
    }
}
