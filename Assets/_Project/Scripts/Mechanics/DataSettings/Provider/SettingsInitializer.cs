using System.Threading.Tasks;
using Zenject;

namespace Mechanics.DataSettings.Provider
{
    public class SettingsInitializer : ISettingsInitializer
    {
        public Task Init()
        {
            return Task.CompletedTask;
        }
    }
}