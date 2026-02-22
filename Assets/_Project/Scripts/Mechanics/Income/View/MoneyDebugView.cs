using Mechanics.Income;
using TMPro;
using UI.Buttons;
using UnityEngine;
using Zenject;

namespace Mechanics.Income.View
{
    public class MoneyDebugView : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _amountInputField;
        [SerializeField] private UIButton _addCleanButton;
        [SerializeField] private UIButton _addDirtyButton;

        private IGameEconomyService _gameEconomyService;

        [Inject]
        private void Construct(IGameEconomyService gameEconomyService)
        {
            _gameEconomyService = gameEconomyService;
        }

        private void OnEnable()
        {
            _addCleanButton.OnClicked += OnAddCleanClicked;
            _addDirtyButton.OnClicked += OnAddDirtyClicked;
        }

        private void OnDisable()
        {
            _addCleanButton.OnClicked -= OnAddCleanClicked;
            _addDirtyButton.OnClicked -= OnAddDirtyClicked;
        }

        private bool TryParseAmount(out float amount)
        {
            amount = 0f;
            return float.TryParse(_amountInputField.text, out amount) && amount > 0;
        }

        private void OnAddCleanClicked()
        {
            if (TryParseAmount(out float amount))
                _gameEconomyService.AddCleanMoney(amount);
        }

        private void OnAddDirtyClicked()
        {
            if (TryParseAmount(out float amount))
                _gameEconomyService.AddDirtyMoney(amount);
        }
    }
}
