using TMPro;
using UnityEngine;

namespace Mechanics.Events.Views.Company
{
    public class CompanyEventNameView : BaseCompanyEventView
    {
        [SerializeField] private TMP_Text _text;
        
        public override void UpdateView()
        {
            _text.text = _companyEventData.EventName;
        }
    }
}