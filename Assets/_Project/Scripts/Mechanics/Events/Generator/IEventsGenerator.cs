namespace Mechanics.Events
{
    public interface IEventsGenerator
    {
        void Init(CompanyEventsSettingsData companyEventsSettingsData);
        CompanyEventData GenerateCompanyEvent(string companyKey);
    }
}