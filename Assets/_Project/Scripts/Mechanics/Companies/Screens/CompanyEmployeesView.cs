using System.Collections.Generic;
using System.Linq;
using Core.Scripts.Pools;
using Mechanics.Characters;
using UnityEngine;
using Zenject;

namespace Mechanics.Companies
{
    public class CompanyEmployeesView: MonoBehaviour
    {
        [SerializeField]
        private PoolGameObjects _pool;

        [SerializeField] private RectTransform _container;
        
        private readonly List<CompanyEmployeeView> _views = new();
        private ICompanyData _companyData;
        private ICharactersProvider _charactersProvider;

        [Inject]
        private void Construct(ICharactersProvider charactersProvider)
        {
            _charactersProvider = charactersProvider;
        }
        

        public void Init(ICompanyData companyData)
        {
            _companyData = companyData;

            _pool.Init();
            GenerateEmployeesViews();
        }

        public void Show() => gameObject.SetActive(true);

        public void Hide() => gameObject.SetActive(false);

        private void GenerateEmployeesViews()
        {
            EnsureViewCount(_companyData.Employees.Count());

            for (int i = 0; i < _companyData.Employees.Count(); i++)
            {
                _views[i].gameObject.SetActive(true);
                var characterData = _charactersProvider.GetCharacterByKey(_companyData.Employees.ElementAt(i));
                _views[i].Init(characterData);
            }

            for (int i = _companyData.Employees.Count(); i < _views.Count; i++)
                _views[i].gameObject.SetActive(false);
        }
        
        private void EnsureViewCount(int requiredCount)
        {
            while (_views.Count < requiredCount)
            {
                var view = GetEmployeeItem();
                view.gameObject.SetActive(true);
                view.transform.SetParent(_container, false);
                _views.Add(view);
            }
        }
        
        private CompanyEmployeeView GetEmployeeItem()
        {
            var result = _pool.Pull().GetOwner<CompanyEmployeeView>();
            result.OnManageClicked += CompanyEmployeeView_OnManageClicked;
            return result;
        }
        
        private void CompanyEmployeeView_OnManageClicked(ICharacterData obj)
        {
            //TODO : Open manage employee screen
        }
    }
}