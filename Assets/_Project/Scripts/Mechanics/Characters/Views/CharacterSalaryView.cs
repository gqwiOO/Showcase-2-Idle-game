using TMPro;
using UnityEngine;

namespace Mechanics.Characters.Views
{
    public class CharacterSalaryView: BaseCharacterView
    {
        [SerializeField] private TMP_Text _textField;
        [SerializeField] private string _suffix;
        
        public override void UpdateView()
        {
            _textField.text = _characterData.Salary + _suffix;
        }
    }
}