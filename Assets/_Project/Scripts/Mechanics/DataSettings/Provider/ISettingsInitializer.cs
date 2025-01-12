using System.Threading.Tasks;

namespace Mechanics.DataSettings.Provider
{
    public interface ISettingsInitializer
    {
        Task Init();
    }
}