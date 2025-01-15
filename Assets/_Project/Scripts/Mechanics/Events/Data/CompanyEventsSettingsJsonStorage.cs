using Core.Storage.JsonStorage;

namespace Mechanics.Events
{
    public class CompanyEventsSettingsJsonStorage: BaseJsonResourcesStorage<CompanyEventsSettingsData>
    {
        public override string Path => "Data/Settings/CompanyEventsSettings";
    }
}