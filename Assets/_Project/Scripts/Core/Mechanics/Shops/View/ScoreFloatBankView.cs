namespace Core.Mechanics.Shops.View
{
    public class ScoreFloatBankView : ScoreBankView<float>
    {
        protected override void GetProvider() 
            => InitBank(_scoreBanksProvider.GetFloatBankProvider().Get(_bankId));

        protected override void UpdateView(float value) => _scoreText.text = _prefix + value.ToString("0.00") + _sufix;
    }
}