using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mechanics.Characters;
using Mechanics.Developing.Data;
using Mechanics.DayNight;
using Mechanics.Income;
using Mechanics.Product.Provider;
using UnityEngine;
using Zenject;
using Random = System.Random;

namespace Mechanics.Product
{
    public class ContractMarketService : IContractMarketService
    {
        private const int OffersPerNight = 6;
        private const float Tier2ReputationRequired = 50f;
        private const float Tier3ReputationRequired = 120f;
        private const float PenaltyHeatBase = 5f;
        private const float InjuryChanceBase = 0.25f;

        private List<ContractData> _currentOffers = new();
        private HashSet<string> _contractsTakenThisNight = new();

        private IGameTimeService _gameTimeService;
        private IGameEconomyService _gameEconomyService;
        private IContractService _contractService;
        private IContractsProvider _contractsProvider;
        private ICharactersProvider _charactersProvider;

        public event Action OnOffersRefreshed;
        public event Action OnOfferTaken;

        [Inject]
        private void Construct(IGameTimeService gameTimeService, IGameEconomyService gameEconomyService,
            IContractService contractService, IContractsProvider contractsProvider,
            ICharactersProvider charactersProvider)
        {
            _gameTimeService = gameTimeService;
            _gameEconomyService = gameEconomyService;
            _contractService = contractService;
            _contractsProvider = contractsProvider;
            _charactersProvider = charactersProvider;
        }

        public async Task Init()
        {
            _gameTimeService.OnPhaseChanged += OnPhaseChanged;
            // if (_gameTimeService.CurrentPhase == DayNightPhase.Night)
            RefreshOffers();
        }
        
        private void OnPhaseChanged(DayNightPhase phase)
        {
            if (phase == DayNightPhase.Night)
            {
                _contractsTakenThisNight.Clear();
                RefreshOffers();
            }
            else if (phase == DayNightPhase.Day)
            {
                ApplyPenaltiesForUncompletedContracts();
            }
        }

        private void RefreshOffers()
        {
            _currentOffers = GenerateOffers(OffersPerNight);
            OnOffersRefreshed?.Invoke();
        }

        private List<ContractData> GenerateOffers(int count)
        {
            var result = new List<ContractData>();
            var types = Enum.GetValues(typeof(ContractType)).Cast<ContractType>().Where(t => t != ContractType.None).ToArray();
            var roles = new[]
            {
                RoleType.Assassin, RoleType.Handler, RoleType.Analyst, RoleType.Driver,
                RoleType.Medic, RoleType.Cleaner, RoleType.Gunsmith, RoleType.Technician
            };

            for (int i = 0; i < count; i++)
            {
                var tier = PickTier();
                var random = new Random();
                var type = types[random.Next(0, types.Length)];
                var requiredRoles = PickRequiredRoles(roles);
                var (reward, rep, heat) = GetRewardsByTier(tier);

                var offer = ContractData.CreateOffer(
                    Guid.NewGuid().ToString(),
                    $"{type} - Tier {(int)tier}",
                    type,
                    tier,
                    requiredRoles,
                    reward,
                    rep,
                    heat,
                    PenaltyHeatBase * (int)tier,
                    InjuryChanceBase
                );
                result.Add(offer);
            }

            return result;
        }

        private ContractTier PickTier()
        {
            var r = new Random().NextDouble();
            // var r = UnityEngine.Random.value;
            if (IsTierUnlocked(ContractTier.Tier3) && r < 0.2f) return ContractTier.Tier3;
            if (IsTierUnlocked(ContractTier.Tier2) && r < 0.5f) return ContractTier.Tier2;
            return ContractTier.Tier1;
        }

        private List<RoleType> PickRequiredRoles(RoleType[] roles)
        {
            Random random = new Random();
            var count = random.Next(1, 4);
            // var count = UnityEngine.Random.Range(1, 4);
            var result = new List<RoleType>();
            for (int i = 0; i < count; i++)
                // result.Add(roles[UnityEngine.Random.Range(0, roles.Length)]);
                result.Add(roles[random.Next(0,roles.Length)]);
            return result;
        }

        private (float reward, float rep, float heat) GetRewardsByTier(ContractTier tier)
        {
            return tier switch
            {
                ContractTier.Tier1 => (50f, 5f, 3f),
                ContractTier.Tier2 => (120f, 12f, 6f),
                ContractTier.Tier3 => (250f, 25f, 10f),
                _ => (50f, 5f, 3f)
            };
        }

        public IReadOnlyList<IContractData> GetAvailableOffers()
        {
            if (_gameTimeService.CurrentPhase != DayNightPhase.Night)
                return new List<IContractData>();

            return _currentOffers
                .Where(o => IsTierUnlocked(o.Tier))
                .Cast<IContractData>()
                .ToList();
        }

        public bool CanTakeOffer(IContractData offer)
        {
            if (_gameTimeService.CurrentPhase != DayNightPhase.Night)
                return false;
            return offer != null && _currentOffers.Contains((ContractData)offer) && IsTierUnlocked(offer.Tier);
        }

        public void TakeOffer(IContractData offer, List<ICharacterData> assignees, string ownerKey)
        {
            if (_gameTimeService.CurrentPhase != DayNightPhase.Night)
                return;
            var offerData = (ContractData)offer;
            if (!_currentOffers.Remove(offerData)) return;

            var executionData = new DevelopingData(assignees.Select(c => (CharacterData)c).ToList());
            var contract = ContractData.CreateFromOffer(offerData, executionData);

            _contractService.TakeContract(contract, ownerKey);
            _contractsTakenThisNight.Add(offer.Key);
            OnOfferTaken?.Invoke();
        }

        public bool IsTierUnlocked(ContractTier tier)
        {
            var rep = _gameEconomyService.GetReputation();
            return tier switch
            {
                ContractTier.Tier1 => true,
                ContractTier.Tier2 => rep >= Tier2ReputationRequired,
                ContractTier.Tier3 => rep >= Tier3ReputationRequired,
                _ => false
            };
        }

        private void ApplyPenaltiesForUncompletedContracts()
        {
            var keys = _contractsTakenThisNight.ToList();
            foreach (var key in keys)
            {
                var contract = _contractsProvider.GetContract(key);
                if (contract == null || contract.ContractState == ContractState.Completed)
                {
                    _contractsTakenThisNight.Remove(key);
                    continue;
                }

                _contractsTakenThisNight.Remove(key);
                var c = (ContractData)contract;
                _gameEconomyService.AddHeat(c.PenaltyHeat);

                if (c._executionData?.Developers != null && c._executionData.Developers.Count > 0 &&
                    UnityEngine.Random.value < c.InjuryChance)
                {
                    var victim = c._executionData.Developers[UnityEngine.Random.Range(0, c._executionData.Developers.Count)];
                    if (victim is CharacterData cd)
                        cd.SetInjured(true);
                }
            }
        }
    }
}
