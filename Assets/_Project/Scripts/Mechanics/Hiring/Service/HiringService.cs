using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Scripts.Extension.System;
using Mechanics.Characters;
using Mechanics.Companies;
using Mechanics.Hiring.Data;
using PimDeWitte.UnityMainThreadDispatcher;
using Zenject;

namespace Mechanics.Hiring.Service
{
    public class HiringService : IHiringService
    {
        private const int MAX_FREE_EMPLOYEES = 20;
        
        private readonly HiringSettingsJsonStorage _hiringSettingsJsonStorage = new();

        private List<ICharacterData> _charactersToHire = new();
        private CharactersGeneratingSettings _generatingSettings;
        private ICompaniesService _companiesService;


        private ICharacterData _myCharacterData;
        private ICharactersProvider _charactersProvider;


        public event Action<ICharacterData> OnHired;
        
        [Inject]
        private void Construct(ICompaniesService companiesService, ICharactersProvider charactersProvider)
        {
            _charactersProvider = charactersProvider;
            _companiesService = companiesService;
        }

        public async Task Init()
        {
            await UnityMainThreadDispatcher.Instance().EnqueueAsync(() =>_hiringSettingsJsonStorage.Load());

            _myCharacterData = _charactersProvider.GetMyCharacter();

            _generatingSettings = _hiringSettingsJsonStorage.Get();

            _charactersToHire = GenerateAvailableCharactersToHire();
        }

        public void Hire(ICharacterData characterData)
        {
            _companiesService.AddEmployeeToCompany(characterData, _myCharacterData.CompanyKey);
            _charactersToHire.Remove(characterData);
            OnHired?.Invoke(characterData);
            
        }

        public void Hire(string characterKey)
        {
            
        }

        public List<ICharacterData> GetAllAvailableCharactersToHire() 
            => _charactersToHire;

        public List<ICharacterData> GenerateAvailableCharactersToHire()
        {
            List<ICharacterData> result = new();
            var generateAmount = MAX_FREE_EMPLOYEES;

            Random random = new System.Random();
            for (int i = 0; i < generateAmount; i++)
            {
                CharacterSkill skill = (CharacterSkill)(random.Next(1, Enum.GetValues(typeof(CharacterSkill)).Length));
                ICharacterData characterData = new CharacterData(
                    _generatingSettings.Names.PickRandom(),
                    _generatingSettings.GetAge(skill)
                    ,RoleType.Developer,
                    _generatingSettings.GetSalary(skill),
                    "",
                    skill);
                
                result.Add(characterData);
                
                _charactersProvider.AddCharacter(characterData);
            }

            return result;
        }
    }
}