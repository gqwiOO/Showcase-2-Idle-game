using Mechanics.Income;
using TMPro;
using UnityEngine;
using Zenject;

namespace Mechanics.Companies
{
    public class CompanyIncomeView: BaseCompanyView
    {
        [SerializeField] 
        private TMP_Text _field;
        [SerializeField] 
        private string _prefix;
        [SerializeField] 
        private string _suffix;

        private IGameEconomyService _gameEconomyService;
        private ICompanyData _companyData1;

        [Inject]
        private void Construct(IGameEconomyService gameEconomyService)
        {
            _gameEconomyService = gameEconomyService;
        }
        
        public override void UpdateView()
        {
            _field.text = _prefix + _gameEconomyService.GetCompanyIncomePerMonth(_companyData.Key) + _suffix;
        }
    }
}