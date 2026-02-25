using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Mechanics.Characters;
using Mechanics.Companies;
using Mechanics.CompaniesRating.Screen;
using Mechanics.DayNight;
using Mechanics.Hiring.Screens;
using Mechanics.Income.Screens;
using Mechanics.MainMenu.Screens;
using Mechanics.Product;
using Services.Screen.Interfaces;

namespace Services.Screen
{
    public class MainScreensService : BaseScreensService, IMainScreenService
    {
        public async UniTask ShowCharacterProductsScreen(string characterKey)
        {
            var screen = GetScreen<CharacterProductsScreen>();
            await screen.Init(characterKey);
            await screen.Open();
        }

        public async UniTask ShowContractScreen(IContractData contractData)
        {
            var screen = GetScreen<ProductManageScreen>();
            await screen.Init(contractData);
            await screen.Open();
        }

        public async UniTask ShowHiringScreen()
        {
            var screen = GetScreen<HiringScreen>();
            await screen.Init();
            await screen.Open();
        }

        public async UniTask ShowConfirmHiringScreen(ICharacterData characterData)
        {
            var screen = GetScreen<HiringConfirmationScreen>();
            await screen.Init(characterData);
            await screen.Open();
        }

        public async UniTask ShowCreateProductScreen()
        {
            var screen = GetScreen<CreateProductScreen>();
            await screen.Open();
            await screen.Init();
        }

        public async UniTask HideScreen<T>() where T : BaseScreen
        {
            var screen = GetScreen<CharacterProductsScreen>();
            await screen.Hide();
        }

        public async UniTask HideAllScreens()
        {
            List<UniTask> tasks = new();
            GetAllScreensExceptOf(GetScreen<MainMenuScreen>()).ForEach(screen => tasks.Add(screen.Hide()));
            
            await UniTask.WhenAll(tasks);
        }

        public async UniTask HideAllScreensExceptOf<T>() where T : BaseScreen
        {
            List<UniTask> tasks = new();
            GetAllScreensExceptOf(GetScreen<T>()).ForEach(screen => tasks.Add(screen.Hide()));
            await UniTask.WhenAll(tasks);
        }

        public async UniTask ShowCompanyScreen(ICompanyData companyData)
        {
            var screen = GetScreen<CompanyScreen>();
            screen.Init(companyData);
            await screen.Open();
        }

        public async UniTask ShowCompaniesRatingScreen()
        {
            var screen = GetScreen<CompaniesRatingScreen>();
            screen.Init();
            await screen.Open();
        }

        public async UniTask ShowDayResultScreen(DaySummaryData data)
        {
            var screen = GetScreen<DayResultScreen>();
            screen.Init(data);
            await screen.Open();
        }

        public async UniTask ShowLaunderMoneyScreen()
        {
            var screen = GetScreen<LaunderMoneyScreen>();
            await screen.Open();
        }

        public async UniTask ShowContractMarketScreen()
        {
            var screen = GetScreen<ContractMarketScreen>();
            await screen.Open();
        }

        public async UniTask ShowTakeContractScreen(IContractData offer)
        {
            var screen = GetScreen<TakeContractScreen>();
            screen.Init(offer);
            await screen.Open();
        }
    }
}