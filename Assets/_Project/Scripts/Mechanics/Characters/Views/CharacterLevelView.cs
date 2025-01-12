using TMPro;
using UnityEngine;

namespace Mechanics.Characters.Views
{
    public class CharacterLevelView : BaseCharacterView
    {
        [SerializeField] 
        private TMP_Text _textField;
        public override void UpdateView()
        {
            _textField.text = _characterData.CharacterSkill.ToString();
        }
    }
}