using UnityEngine;

namespace Mechanics.Characters.Views
{
    public abstract class BaseCharacterView: MonoBehaviour
    {
        protected ICharacterData _characterData;

        public virtual void Init(ICharacterData characterData)
        {
            _characterData = characterData;
        }
        public abstract void UpdateView();
    }
}