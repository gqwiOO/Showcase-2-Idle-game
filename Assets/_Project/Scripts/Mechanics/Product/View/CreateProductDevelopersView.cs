using System;
using System.Collections.Generic;
using System.Linq;
using Core.Scripts.Pools;
using Mechanics.Characters;
using UnityEngine;

namespace Mechanics.Product
{
    public class CreateProductDevelopersView: MonoBehaviour
    {
        [SerializeField] private PoolGameObjects _pool;
        [SerializeField] private RectTransform _addButtonObject;
        [SerializeField] private RectTransform _container;
        [SerializeField] private List<CreateProductDeveloperView> _views = new ();


        public event Action<ICharacterData> OnDeveloperUnselected; 


        private void Start()
        {
            _pool.Init();
        }

        public void Add(ICharacterData characterData)
        {
            var itemView = GetViewItem();
            itemView.Init(characterData);
            itemView.transform.SetParent(_container);
            itemView.gameObject.SetActive(true);
            _views.Add(itemView);
            itemView.OnDeveloperUnselected += CreateProductDeveloperView_OnDeveloperUnselected;
            _addButtonObject.transform.SetAsLastSibling();
        }

        private void CreateProductDeveloperView_OnDeveloperUnselected(ICharacterData obj)
        {
            Remove(obj);
        }

        public void Remove(ICharacterData characterData)
        {
            var item = _views.FirstOrDefault(item => item.CharacterData == characterData);
            if (item == null)
                return;
            
            PushView(characterData, item);
        }

        private void PushView(ICharacterData characterData, CreateProductDeveloperView item)
        {
            _views.Remove(item);
            item.OnDeveloperUnselected -= CreateProductDeveloperView_OnDeveloperUnselected;
            
            //TODO : replace getcomponent with better solution
            _pool.Push(item.GetComponent<PoolObject>());
            OnDeveloperUnselected?.Invoke(characterData);
        }

        public void ResetView()
        {
            var viewsCopy = new List<CreateProductDeveloperView>(_views);
            foreach (var createProductDeveloperView in viewsCopy)
                PushView(createProductDeveloperView.CharacterData, createProductDeveloperView);
        }

        private CreateProductDeveloperView GetViewItem()
        {
            return _pool.Pull().GetOwner<CreateProductDeveloperView>();
        }
    }
}