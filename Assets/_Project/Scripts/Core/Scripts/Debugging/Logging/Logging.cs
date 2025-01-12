namespace Core.Scripts.Debugging.Logging
{
    public class Logging
    {
        private LogData _logData;
        private LoggingSettings _settings;

        public Logging(LoggingSettings settings)
        {
            _settings = settings;
        }

        public void Log(string message, LoggingLevel loggingLevel)
        {
            if(CanLog(loggingLevel))
                _logData.Append(message);
        }

        private bool CanLog(LoggingLevel level)
        {
            return level >= _settings.LoggingLevel;
        }
    }
}