namespace Mechanics.Events
{
    public abstract class BaseEventData
    {
        public string EventName { get; protected set; }
        
        public BaseEventData(string name)
        {
            EventName = name;
        }
    }
}