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
        private const int MAX_SPECIALISTS_CANDIDATES = 10;
        
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
            var generateAmount = MAX_SPECIALISTS_CANDIDATES;
            var crewRoles = GetCrewRoles();

            var peacefulRoles = GetPeacefulRoles();
            Random random = new System.Random();
            for (int i = 0; i < generateAmount; i++)
            {
                CharacterSkill skill = (CharacterSkill)(random.Next(1, Enum.GetValues(typeof(CharacterSkill)).Length));
                RoleType blackRole = crewRoles[random.Next(0, crewRoles.Length)];
                PeacefulRoleType peacefulRole = peacefulRoles[random.Next(0, peacefulRoles.Length)];
                ICharacterData characterData = new CharacterData(
                    _generatingSettings.Names.PickRandom(),
                    _generatingSettings.GetAge(skill),
                    blackRole,
                    peacefulRole,
                    _generatingSettings.GetSalary(skill),
                    "",
                    skill);

                result.Add(characterData);
                _charactersProvider.AddCharacter(characterData);
            }

            return result;
        }

        private static RoleType[] GetCrewRoles()
        {
            var values = (RoleType[])Enum.GetValues(typeof(RoleType));
            var list = new List<RoleType>();
            foreach (var r in values)
            {
                if (r != RoleType.None)
                    list.Add(r);
            }
            return list.ToArray();
        }

        private static PeacefulRoleType[] GetPeacefulRoles()
        {
            var values = (PeacefulRoleType[])Enum.GetValues(typeof(PeacefulRoleType));
            var list = new List<PeacefulRoleType>();
            foreach (var r in values)
            {
                if (r != PeacefulRoleType.None)
                    list.Add(r);
            }
            return list.ToArray();
        }
    }
}