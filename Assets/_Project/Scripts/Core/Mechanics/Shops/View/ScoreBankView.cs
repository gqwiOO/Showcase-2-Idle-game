using Core.Mechanics.Shops.Provider;
using Core.Storage;
using Core.Storage.Bank;
using TMPro;
using UnityEngine;
using Zenject;

namespace Core.Mechanics.Shops.View
{
    public abstract class ScoreBankView<TData>: MonoBehaviour
    {
        [SerializeField] protected TMP_Text _scoreText;
        [SerializeField] protected string _prefix;
        [SerializeField] protected string _sufix;
        
        [SerializeField] protected BankId _bankId;
        
        private IDataBank<TData> _scoreBank;
        protected IBanksProvidersProvider _scoreBanksProvider;

        [Inject]
        private void Construct(IBanksProvidersProvider scoreBanksProvider)
        {
            _scoreBanksProvider = scoreBanksProvider;
        }

        protected abstract void GetProvider();
        protected void InitBank(IDataBank<TData> bank) => _scoreBank = bank;

        private void Start()
        {
            GetProvider();
            _scoreBank.OnChanged += ScoreBank_OnScoreChanged;
            UpdateView(_scoreBank.GetValue());
        }

        private void OnDestroy() 
            => _scoreBank.OnChanged -= ScoreBank_OnScoreChanged;

        private void ScoreBank_OnScoreChanged(object sender, DataBankEvent<TData> dataBankEvent) 
            => UpdateView(dataBankEvent.CurrentValue);

        protected virtual void UpdateView(TData value) 
            => _scoreText.text = _prefix + value + _sufix;
    }
}