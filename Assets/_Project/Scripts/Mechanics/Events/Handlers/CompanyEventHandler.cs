using System;
using Mechanics.Product;
using Mechanics.Product.Provider;
using Services.Screen.Interfaces;
using UnityEngine;
using Zenject;

namespace Mechanics.Events
{
    public class CompanyEventHandler : MonoBehaviour,  ICompanyEventHandler
    {
        private ICompanyEventInvoker _companyEventInvoker;
        private IMainScreenService _mainScreenService;
        private IProductService _productService;

        [Inject]
        private void Construct(ICompanyEventInvoker companyEventInvoker, IMainScreenService mainScreenService, IProductService productService)
        {
            _productService = productService;
            _mainScreenService = mainScreenService;
            _companyEventInvoker = companyEventInvoker;
        }

        private void Start() => SubscribeOnInvoker();

        public void SubscribeOnInvoker()
        {
            _companyEventInvoker.OnEventInvoked += HandleEvent;
        }

        public void HandleEvent(object sender, CompanyEventData e)
        {
            float incomeChange = e.Value;

            _productService.ChangeProductIncome(e.ProductKey, incomeChange);
            
            _mainScreenService.ShowCompanyEventScreen(e);
        }

        public void UnsubscribeFromInvoker()
        {
            _companyEventInvoker.OnEventInvoked -= HandleEvent;
        }
    }
}       