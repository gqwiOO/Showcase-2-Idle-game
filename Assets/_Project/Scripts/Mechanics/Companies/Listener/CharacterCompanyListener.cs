using System;
using Mechanics.Characters;
using Zenject;

namespace Mechanics.Companies
{
    public class CharacterCompanyListener : ICharacterCompanyListener, IInitializable, IDisposable
    {
        private ICharacterData _myCharacter;
        
        private ICompaniesService _companiesService;
        private ICharactersProvider _charactersProvider;

        public event Action<ICompanyData> OnMyCharacterCompanyDataChanged;

        [Inject]
        private void Construct(ICompaniesService companiesService, ICharactersProvider charactersProvider)
        {
            _charactersProvider = charactersProvider;
            _companiesService = companiesService;
        }

        public void Initialize()
        {
            _myCharacter = _charactersProvider.GetMyCharacter();
            _companiesService.OnAnyCompanyUpdated += CompaniesService_OnAnyCompanyUpdated;
        }

        public void Dispose() 
            => _companiesService.OnAnyCompanyUpdated -= CompaniesService_OnAnyCompanyUpdated;

        private void CompaniesService_OnAnyCompanyUpdated(ICompanyData data)
        {
            if (_myCharacter.CompanyKey == data.Key)
                OnMyCharacterCompanyDataChanged?.Invoke(data);
        }
    }
}