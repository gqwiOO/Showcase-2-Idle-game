using Core.Storage.JsonStorage;
using Mechanics.CompaniesRating.Data;

namespace Mechanics.CompaniesRating.Json
{
    public class CompaniesGeneratingSettingsJsonStorage: BaseJsonResourcesStorage<CompaniesGeneratingSettings>
    {
        public override string Path
        {
            get { return "Data/Settings/CompaniesSettings"; }
        }
    }
}