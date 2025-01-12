using System.Threading.Tasks;
using Mechanics.CompaniesRating.Json;
using Mechanics.Hiring.Data;
using Mechanics.Product.JSON;
using PimDeWitte.UnityMainThreadDispatcher;
using Zenject;

namespace Mechanics.DataSettings.Provider
{
    public class SettingsInitializer : ISettingsInitializer
    {
        private ISettingsProvider _settingsProvider;

        [Inject]
        private void Construct(ISettingsProvider settingsProvider)
        {
            _settingsProvider = settingsProvider;
        }

        public async Task Init()
        {
            var characterJsonStorage = new HiringSettingsJsonStorage();
            await UnityMainThreadDispatcher.Instance().EnqueueAsync(() => characterJsonStorage.Load());
            
            _settingsProvider.AddSettings(characterJsonStorage.Get());
            
            var productsGeneratingSettingsJsonStorage = new ProductsGeneratingSettingsJsonStorage();
            await UnityMainThreadDispatcher.Instance().EnqueueAsync(() => productsGeneratingSettingsJsonStorage.Load());
            _settingsProvider.AddSettings(productsGeneratingSettingsJsonStorage.Get());

            var companiesGeneratingSettingsJsonStorage = new CompaniesGeneratingSettingsJsonStorage();
            await UnityMainThreadDispatcher.Instance().EnqueueAsync(() => companiesGeneratingSettingsJsonStorage.Load());
            _settingsProvider.AddSettings(companiesGeneratingSettingsJsonStorage.Get());
        }
    }
}