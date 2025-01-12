using Cysharp.Threading.Tasks;
using Mechanics.Characters;
using Mechanics.Companies;
using Mechanics.Product;

namespace Services.Screen.Interfaces
{
    public interface IMainScreenService
    {
        UniTask ShowCharacterProductsScreen(string characterKey);
        UniTask ShowProductScreen(IProductData productData);
        UniTask ShowHiringScreen();
        UniTask ShowConfirmHiringScreen(ICharacterData characterData);
        UniTask ShowCreateProductScreen();
        UniTask HideScreen<T>() where T : BaseScreen;
        UniTask HideAllScreens();
        UniTask HideAllScreensExceptOf<T>() where T : BaseScreen;
        UniTask ShowCompanyScreen(ICompanyData companyData);
        UniTask ShowCompaniesRatingScreen();
    }
}