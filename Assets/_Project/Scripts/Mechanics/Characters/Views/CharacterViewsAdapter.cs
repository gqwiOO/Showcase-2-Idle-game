using System.Collections.Generic;
using UnityEngine;

namespace Mechanics.Characters.Views
{
    public class CharacterViewsAdapter: BaseCharacterView
    {
        [SerializeField] private List<BaseCharacterView> _views;
        
        public override void Init(ICharacterData characterData)
        {
            base.Init(characterData);
            _views.ForEach(view => view.Init(characterData));
        }

        public override void UpdateView()
        {
            _views.ForEach(view => view.UpdateView());
        }
    }
}