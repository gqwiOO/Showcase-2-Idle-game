namespace Mechanics.Events
{
    public class CompanyEventData : BaseEventData
    {
        public string CompanyKey { get; private set; }
        
        public CompanyEventType EventType{ get; private set; }

        public string ProductKey { get; private set; }
        
        public float Value { get; private set; }
        
        public CompanyEventData(string companyKey, string productKey, string name, CompanyEventType companyEventType, float value) : base(name)
        {
            CompanyKey = companyKey;
            ProductKey = productKey;
            EventType = companyEventType;
            Value = value;
        }
    }
}