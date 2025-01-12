using TMPro;
using UnityEngine;

namespace Mechanics.Companies
{
    public class CompanyNameView : BaseCompanyView
    {
        [SerializeField] private TMP_Text _text;
        public override void UpdateView()
        {
            _text.text = _companyData.Name;
        }
    }
}