using System.Threading.Tasks;
using Core.Scripts.Services.UpdateService;
using Mechanics.Companies;
using PimDeWitte.UnityMainThreadDispatcher;
using Zenject;

namespace Mechanics.Events
{
    public class EventsService : IEventsService, IUpdatable
    {
        private CompanyEventsSettingsJsonStorage _companyEventsSettingsJsonStorage = new ();
        
        private CompanyEventsSettingsData _settings;

        // TODO : Remote
        private readonly float _eventIntervalSeconds = 10f;
        private float _currentEventIntervalSeconds = 0f;
        public UpdateType UpdateType => UpdateType.Update;
        
        private IUpdateService _updateService;

        private IEventsGenerator _eventsGenerator;
        private ICompaniesProvider _companiesProvider;
        private ICompanyEventInvoker _companyEventInvoker;

        [Inject]
        private void Construct(IUpdateService updateService, IEventsGenerator eventsGenerator, ICompaniesProvider companiesProvider,
            ICompanyEventInvoker companyEventInvoker)
        {
            _companyEventInvoker = companyEventInvoker;
            _companiesProvider = companiesProvider;
            _eventsGenerator = eventsGenerator;
            _updateService = updateService;
        }
        
        public async Task Init()
        {
            await UnityMainThreadDispatcher.Instance().
                EnqueueAsync(() => _companyEventsSettingsJsonStorage.Load());
            _settings = _companyEventsSettingsJsonStorage.Get();

            _eventsGenerator.Init(_settings);
            _updateService.Add(this);
        }

        public void Tick(float tickTime)
        {
            if (_currentEventIntervalSeconds < _eventIntervalSeconds)
                _currentEventIntervalSeconds += tickTime;
            else
            {
                _currentEventIntervalSeconds = 0f;
                CallEvent();
            }
        }

        private void CallEvent()
        {
            CompanyEventData eventData = _eventsGenerator.GenerateCompanyEvent(_companiesProvider.GetMyCompany().Key);
            _companyEventInvoker.InvokeEvent(eventData);
        }
    }
}