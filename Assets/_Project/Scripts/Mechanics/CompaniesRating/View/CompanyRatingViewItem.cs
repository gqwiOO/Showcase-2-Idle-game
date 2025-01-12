using Mechanics.Companies;
using TMPro;
using UnityEngine;

namespace Mechanics.CompaniesRating.View
{
    public class CompanyRatingViewItem: MonoBehaviour
    {
        [SerializeField] private CompanyViewsAdapter _companyViewsAdapter;
        [SerializeField] private TMP_Text _companyRatingField;
        private ICompanyData _companyData;

        public void Init(ICompanyData companyData, int companyRatingPlace)
        {
            _companyData = companyData;
            _companyViewsAdapter.Init(_companyData);
            _companyViewsAdapter.UpdateView();

            _companyRatingField.text = companyRatingPlace.ToString();
        }
    }
}