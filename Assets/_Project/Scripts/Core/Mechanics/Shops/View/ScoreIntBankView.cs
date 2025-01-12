namespace Core.Mechanics.Shops.View
{
    public class ScoreIntBankView : ScoreBankView<int>
    {
        protected override void GetProvider() 
            => InitBank(_scoreBanksProvider.GetIntBankProvider().Get(_bankId));
    }
}