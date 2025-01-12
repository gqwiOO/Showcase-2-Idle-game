using TMPro;
using UnityEngine;

namespace Mechanics.Characters.Views
{
    public class CharacterNameView: BaseCharacterView
    {
        [SerializeField] private TMP_Text _textField;
        
        public override void UpdateView()
        {
            _textField.text = _characterData.Name;
        }
    }
}