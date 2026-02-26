using System;
using System.Collections.Generic;
using System.Linq;
using Core.Scripts.Debugging;
using Core.Scripts.Pools;
using Cysharp.Threading.Tasks;
using Mechanics.Characters;
using Services.Screen;
using UI.Buttons;
using UnityEngine;

namespace Mechanics.Product
{
    public class SelectEmployeesScreen: BaseScreen
    {
        [SerializeField] private PoolGameObjects _pool;
        [SerializeField] private UIButton closeButton;
        [SerializeField] private RectTransform _container;
        [SerializeField] private List<SelectDeveloperItem> _views;

        private int _currentScreenSessionSelectedDevelopersCount = 0;
        private int _maxEmployeesToTake;
        
        public event Action<ICharacterData> OnDeveloperSelected; 
        public void Init(List<ICharacterData> availableEmployees, int maxEmployeesToTake)
        {
            _currentScreenSessionSelectedDevelopersCount = 0;
            _maxEmployeesToTake = maxEmployeesToTake;
            _pool.Init();

            InitViewItems(availableEmployees);
        }
        
        private void InitViewItems(List<ICharacterData> employees)
        {
            EnsureViewCount(employees.Count);

            for (int i = 0; i < employees.Count; i++)
            {
                _views[i].gameObject.SetActive(true);
                _views[i].Init(employees[i]);
            }

            for (int i = employees.Count; i < _views.Count; i++)
                _views[i].gameObject.SetActive(false);
        }

        private void EnsureViewCount(int requiredCount)
        {
            while (_views.Count < requiredCount)
            {
                var view = GetCharacterItem();
                view.gameObject.SetActive(true);
                view.transform.SetParent(_container);
                view.OnSelected += DeveloperItem_OnSelected;
                _views.Add(view);
            }
        }

        private void DeveloperItem_OnSelected(ICharacterData employee)
        {
            if (!CanSelectDeveloper(employee))
            {
                Debugging.Log(this,$"Can't select developer for project : Limit is reached(limit = {_maxEmployeesToTake})");
                return;
            }
            
            RemoveDeveloperFromList(employee);
            _currentScreenSessionSelectedDevelopersCount += 1;
            OnDeveloperSelected?.Invoke(employee);
        }

        private bool CanSelectDeveloper(ICharacterData employee)
        {
            return _currentScreenSessionSelectedDevelopersCount < _maxEmployeesToTake &&
                   employee.ContractsInExecutionCount < 1;
        }

        private void RemoveDeveloperFromList(ICharacterData employee)
        {
            var selectDeveloperItem = _views.FirstOrDefault(view => view.CharacterData == employee);
            selectDeveloperItem.gameObject.SetActive(false);
            _views.Remove(selectDeveloperItem);
        }

        private SelectDeveloperItem GetCharacterItem() 
            => _pool.Pull().GetOwner<SelectDeveloperItem>();

        private void Start() 
            => closeButton.OnClicked += CloseScreen;

        private void OnDestroy() 
            => closeButton.OnClicked -= CloseScreen;

        private void CloseScreen() => Hide().Forget();
    }
}