using System;
using System.Collections.Generic;

namespace Mechanics.Events
{
    [Serializable]
    public class CompanyEventsSettingsData
    {
        public List<CompanyEventSetting> EventSettings;
        
        public Dictionary<CompanyEventType, float> EventSettingsValues;
    }
    
    [Serializable]
    public class CompanyEventSetting
    {
        public string EventName;
        public CompanyEventType EventType;
    }

    public enum CompanyEventType
    {
        None = 0,
        ProductIncomeGrowth = 1,
        ProductIncomeDecrease = 2,
    }
}