using UnityEngine;

namespace Core.Scripts.Debugging
{
    public static class Debugging
    {
        private static DebuggingSettings _settings;

        public static void Init(DebuggingSettings settings)
        {
            _settings = settings;
        }

        public static void Log<T>(T obj, string message)
        {
            if(CanSend())
            {
                string sendMessage = $"[{(typeof(T).Name)}] {message}.";
                Debug.Log(sendMessage);
            }
        }

        private static bool CanSend() 
            => _settings.IsEnabled;
    }
}