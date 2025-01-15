namespace Mechanics.Events
{
    public interface IEventHandler<TEvent> where TEvent : BaseEventData
    {
        void HandleEvent(object sender, TEvent eventData);

        public void SubscribeOnInvoker();
        public void UnsubscribeFromInvoker();
    }

    public interface ICompanyEventHandler : IEventHandler<CompanyEventData>
    {
    }
}