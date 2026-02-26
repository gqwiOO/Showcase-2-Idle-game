using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Scripts.Extension.System;
using Mechanics.Characters;
using Mechanics.Companies;
using Mechanics.Config;
using Mechanics.Hiring.Data;
using PimDeWitte.UnityMainThreadDispatcher;
using Zenject;

namespace Mechanics.Hiring.Service
{
    public class HiringService : IHiringService
    {
        private readonly List<ICharacterData> _charactersToHire = new();
        private readonly ICompaniesService _companiesService;
        private readonly HiringConfig _hiringConfig;
        private readonly CharactersGeneratingSettings _generatingSettings;
        private readonly ICharactersProvider _charactersProvider;

        private ICharacterData _myCharacterData;

        public event Action<ICharacterData> OnHired;

        [Inject]
        public HiringService(
            ICompaniesService companiesService,
            ICharactersProvider charactersProvider,
            HiringConfig hiringConfig,
            CharactersGeneratingSettings generatingSettings)
        {
            _companiesService = companiesService;
            _charactersProvider = charactersProvider;
            _hiringConfig = hiringConfig;
            _generatingSettings = generatingSettings;
        }

        public async Task Init()
        {
            _myCharacterData = _charactersProvider.GetMyCharacter();
            _charactersToHire.Clear();
            _charactersToHire.AddRange(GenerateAvailableCharactersToHire());
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
            var generateAmount = _hiringConfig.MaxCandidatesCount;
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