using System;
using System.Collections.Generic;
using Core.Scripts.Debugging;

namespace Mechanics.DataSettings.Provider
{
    public class SettingsProvider : ISettingsProvider
    {
        private Dictionary<Type, ISettingsData> _settingsDictionary = new Dictionary<Type, ISettingsData>();
        
        public T GetSettings<T>()  where T : ISettingsData
        {
            _settingsDictionary.TryGetValue(typeof(T), out var result);
            if(result == null)
                Debugging.Log(this,$" Data {typeof(T).Name} does not contains in dictionary");
            return (T)result;
        }

        public void AddSettings<T>(T settings)  where T : ISettingsData
        {   
            _settingsDictionary.TryAdd(typeof(T), settings);
        }
    }
}