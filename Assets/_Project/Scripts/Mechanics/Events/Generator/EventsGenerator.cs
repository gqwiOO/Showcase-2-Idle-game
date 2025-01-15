using System;
using System.Linq;
using System.Text;
using Core.Extentions;
using Mechanics.Companies;
using Mechanics.Product.Provider;
using Zenject;

namespace Mechanics.Events
{
    public class EventsGenerator : IEventsGenerator
    {
        private readonly Random _random = new Random();
        private IProductsProvider _productsProvider;
        private ICompaniesProvider _companiesProvider;
        private CompanyEventsSettingsData _companyEventsSettingsData;

        [Inject]
        private void Construct(IProductsProvider productsProvider, ICompaniesProvider companiesProvider)
        {
            _companiesProvider = companiesProvider;
            _productsProvider = productsProvider;
        }

        public void Init(CompanyEventsSettingsData companyEventsSettingsData)
        {
            _companyEventsSettingsData = companyEventsSettingsData;
        }

        public CompanyEventData GenerateCompanyEvent(string companyKey)
        {
            var products = _productsProvider.GetAllProductOfCompany(companyKey);
            if (!products.Any())
                throw new InvalidOperationException("No products found for the given company.");

            var product = products[_random.Next(products.Count)];
            var eventType = (CompanyEventType)_random.Next(1, Enum.GetValues(typeof(CompanyEventType)).Length);
            
            var eventSettingsValue = _companyEventsSettingsData.EventSettingsValues[eventType];
            
            float minEventSettingsValue = eventSettingsValue - Math.Abs(eventSettingsValue) * 0.2f;
            float maxEventSettingsValue = eventSettingsValue + Math.Abs(eventSettingsValue) * 0.2f;
            
            var eventValue = FloatExtensions.Random(minEventSettingsValue, maxEventSettingsValue);

            return new CompanyEventData(companyKey, product.Key,GetEventName(product.Name,eventType,eventValue), eventType,eventValue);
        }

        public string GetEventName(string productName, CompanyEventType eventType, float value)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"Game {productName} ");
            switch (eventType)
            {
                case CompanyEventType.None:
                    break;
                case CompanyEventType.ProductIncomeGrowth:
                    stringBuilder.Append($"income increased by {(value * 100).ToString("F1")}%");
                    break;
                case CompanyEventType.ProductIncomeDecrease:
                    stringBuilder.Append($"income decreased by {(value * 100).ToString("F1")}%");
                    break;
            }

            stringBuilder.Append(" from last month. Great work!");
            
            return stringBuilder.ToString();
        }
    }
}