using Mechanics.Characters;
using TMPro;
using UnityEngine;
using Zenject;

namespace Mechanics.Income.View
{
    public class CharacterTotalIncomePerMonthView: MonoBehaviour
    {
        [SerializeField] private TMP_Text _textField;

        [SerializeField] private string suffix;
        private ICharacterData _characterData;
        private IGameEconomyService _gameEconomyService;
        
        [Inject]
        private void Construct(IGameEconomyService gameEconomyService)
        {
            _gameEconomyService = gameEconomyService;
        }

        public void Init(ICharacterData characterData)
        {
            _gameEconomyService.OnMyPlayerIncomeChanged += GameEconomyService_OnMyPlayerIncomeChanged;
            _characterData = characterData;
            GameEconomyService_OnMyPlayerIncomeChanged();
        }

        private void GameEconomyService_OnMyPlayerIncomeChanged()
        {
            var value = _gameEconomyService.GetCharacterIncomePerMonth(_characterData);
            SetText(value.ToString("0.00"));
        }

        private void SetText(string value) => _textField.text = value + suffix;
    }
}