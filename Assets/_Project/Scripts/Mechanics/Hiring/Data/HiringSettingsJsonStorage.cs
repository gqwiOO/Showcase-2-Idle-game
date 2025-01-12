using Core.Storage.JsonStorage;

namespace Mechanics.Hiring.Data
{
    public class HiringSettingsJsonStorage: BaseJsonResourcesStorage<CharactersGeneratingSettings>
    {
        public override string Path { get => PathContainer.HiringGeneratingSettings; }
    }
}