using System;
using Core.Scripts.Debugging;
using Core.Scripts.Debugging.Logging;

namespace Mechanics.Events
{
    public class CompanyEventInvoker : ICompanyEventInvoker
    {
        public event EventHandler<CompanyEventData> OnEventInvoked;
        
        public void InvokeEvent(CompanyEventData eventData)
        {
            Debugging.Log(this,  "Invoke event: " + eventData.EventName);
            OnEventInvoked?.Invoke(this, eventData);    
        }
    }
}