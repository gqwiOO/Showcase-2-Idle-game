using TMPro;
using UnityEngine;

namespace Mechanics.Characters.Views
{
    public class CharacterBlackRoleView : BaseCharacterView
    {
        [SerializeField] private TMP_Text _textField;

        public override void UpdateView()
        {
            if (_textField != null)
                _textField.text = _characterData.BlackRole.ToString();
        }
    }
}
