namespace Mechanics.DataSettings.Provider
{
    public interface ISettingsProvider
    {
        T GetSettings<T>() where T : ISettingsData;
        void AddSettings<T>(T settings) where T : ISettingsData;
    }
}