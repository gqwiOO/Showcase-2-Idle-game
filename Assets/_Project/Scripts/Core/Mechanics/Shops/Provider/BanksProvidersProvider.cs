namespace Core.Mechanics.Shops
{
    public class BanksProvidersProvider : IBanksProvidersProvider
    {
        private IBanksProvider<int> _intProvider;
        private IBanksProvider<float> _floatProvider;
        public IBanksProvider<int> GetIntBankProvider() 
            => _intProvider;

        public BanksProvidersProvider()
        {
            SetIntBankProvider(new BanksProvider<int>());
            SetFloatBankProvider(new BanksProvider<float>());
        }

        public IBanksProvider<float> GetFloatBankProvider() 
            => _floatProvider;
        
        public void SetIntBankProvider(IBanksProvider<int> provider) 
            => _intProvider = provider;

        public void SetFloatBankProvider(IBanksProvider<float> provider)
            => _floatProvider = provider;
    }
}