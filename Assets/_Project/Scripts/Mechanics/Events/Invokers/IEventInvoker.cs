using System;

namespace Mechanics.Events
{
    public interface IEventInvoker<TEvent> where TEvent: BaseEventData
    {
        void InvokeEvent(TEvent eventData);

        event EventHandler<TEvent> OnEventInvoked;
    }
}