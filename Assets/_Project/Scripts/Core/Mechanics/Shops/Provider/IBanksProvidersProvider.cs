namespace Core.Mechanics.Shops
{
    public interface IBanksProvidersProvider
    {
        public IBanksProvider<int> GetIntBankProvider();
        public void SetIntBankProvider(IBanksProvider<int> provider);
        public IBanksProvider<float> GetFloatBankProvider();
        public void SetFloatBankProvider(IBanksProvider<float> provider);
    }
}