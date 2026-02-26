using Mechanics.CompaniesRating.Data;
using Mechanics.CompaniesRating.Generator;
using Mechanics.Hiring.Data;
using UnityEngine;
using Zenject;

namespace Mechanics.Config
{
    public class GameConfigInstaller : MonoInstaller
    {
        [Header("Mechanics")]
        [SerializeField] private GameEconomyConfig _economyConfig;
        [SerializeField] private ContractMarketConfig _contractMarketConfig;
        [SerializeField] private ContractExecutionConfig _contractExecutionConfig;
        [SerializeField] private HiringConfig _hiringConfig;
        [SerializeField] private DefaultPlayerConfig _defaultPlayerConfig;
        [SerializeField] private ContractProductLimitsConfig _contractProductLimitsConfig;

        [Header("Data generation")]
        [SerializeField] private CharactersGeneratingSettingsSO _charactersGeneratingSettings;

        public override void InstallBindings()
        {
            BindConfigs();
            BindDataGenerationSettings();
        }

        private void BindConfigs()
        {
            if (_economyConfig != null)
                Container.Bind<GameEconomyConfig>().FromInstance(_economyConfig).AsSingle();
            if (_contractMarketConfig != null)
                Container.Bind<ContractMarketConfig>().FromInstance(_contractMarketConfig).AsSingle();
            if (_contractExecutionConfig != null)
                Container.Bind<ContractExecutionConfig>().FromInstance(_contractExecutionConfig).AsSingle();
            if (_hiringConfig != null)
                Container.Bind<HiringConfig>().FromInstance(_hiringConfig).AsSingle();
            if (_defaultPlayerConfig != null)
                Container.Bind<DefaultPlayerConfig>().FromInstance(_defaultPlayerConfig).AsSingle();
            if (_contractProductLimitsConfig != null)
                Container.Bind<ContractProductLimitsConfig>().FromInstance(_contractProductLimitsConfig).AsSingle();
        }

        private void BindDataGenerationSettings()
        {
            if (_charactersGeneratingSettings != null)
            {
                Container.Bind<CharactersGeneratingSettingsSO>().FromInstance(_charactersGeneratingSettings).AsSingle();
                Container.Bind<CharactersGeneratingSettings>().FromMethod(ctx => ctx.Container.Resolve<CharactersGeneratingSettingsSO>().GetSettings()).AsSingle();
            }
        }
    }
}
