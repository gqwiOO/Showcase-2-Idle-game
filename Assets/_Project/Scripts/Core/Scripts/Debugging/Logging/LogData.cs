using System;
using System.Collections.Generic;

namespace Core.Scripts.Debugging.Logging
{
    [Serializable]
    public class LogData
    {
        public List<string> logs;

        public void Append(string message) 
            => logs.Add(message);
    }
}