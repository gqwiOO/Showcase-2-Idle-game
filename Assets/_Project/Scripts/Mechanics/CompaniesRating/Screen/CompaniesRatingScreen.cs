using System.Collections.Generic;
using Core.Scripts.Pools;
using Cysharp.Threading.Tasks;
using Mechanics.Companies;
using Mechanics.CompaniesRating.View;
using Mechanics.Income;
using Services.Screen;
using UnityEngine;
using Zenject;

namespace Mechanics.CompaniesRating.Screen
{
    public class CompaniesRatingScreen: BaseScreen
    {
        [SerializeField] private List<CompanyRatingViewItem> _views;
        [SerializeField] private PoolGameObjects _pool;
        [SerializeField] private RectTransform _container;
        private ICompaniesProvider _companiesProvider;
        private IGameEconomyService _gameEconomyService;

        [Inject]
        private void Construct(ICompaniesProvider companiesProvider, IGameEconomyService gameEconomyService)
        {
            _gameEconomyService = gameEconomyService;
            _companiesProvider = companiesProvider;
        }
        public async UniTask Init()
        {
            _pool.Init();
            var companies = _companiesProvider.GetAllCompanies();
            InitViewItems(companies);
        }
        
        private void InitViewItems(IEnumerable<ICompanyData> companies)
        {
            var companiesCopyList = SortCompaniesByIncomeDescending(companies);
            
            EnsureViewCount(companiesCopyList.Count);

            for (int i = 0; i < companiesCopyList.Count; i++)
            {
                _views[i].gameObject.SetActive(true);
                _views[i].Init(companiesCopyList[i], i + 1);
            }

            for (int i = companiesCopyList.Count; i < _views.Count; i++)
                _views[i].gameObject.SetActive(false);
        }
        
        private List<ICompanyData> SortCompaniesByIncomeDescending(IEnumerable<ICompanyData> companies)
        {
            var companiesCopyList = new List<ICompanyData>(companies);
            companiesCopyList.Sort((companyA, companyB) =>
                _gameEconomyService.GetCompanyIncomePerMonth(companyB.Key)
                    .CompareTo(_gameEconomyService.GetCompanyIncomePerMonth(companyA.Key))
            );
            return companiesCopyList;
        }
        
        private void EnsureViewCount(int requiredCount)
        {
            while (_views.Count < requiredCount)
            {
                var view = GetItem();
                view.gameObject.SetActive(true);
                view.transform.SetParent(_container, false);
                _views.Add(view);
            }
        }

        private CompanyRatingViewItem GetItem() 
            => _pool.Pull().GetOwner<CompanyRatingViewItem>();
    }
}