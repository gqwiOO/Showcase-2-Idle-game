using Core.Scripts.Pools;
using Core.Storage;
using Core.Storage.Bank;
using TMPro;
using UI.Buttons;
using UnityEngine;

namespace Core.Mechanics.Shops.View
{
    public abstract class BaseShopItemView<TShopItem>: MonoBehaviour where TShopItem : IShopItemData
    {
        [field: SerializeField] 
        public PoolObject PoolObject { get; private set; }

        [Header("Price")]
        
        [SerializeField] 
        private TMP_Text _priceText;

        [SerializeField] 
        private string _pricePrefix;

        [SerializeField] 
        private string _priceSufix;

        [Header("Components")]
        
        [SerializeField] 
        private UIButton _buyButton;

        private IDataBank<int> _shopWallet;
        private TShopItem _shopItemData;

        public bool CanBePurchased 
            => _shopWallet.CanSpend(_shopItemData.Price);

        private void OnEnable() => Subscribe();

        private void OnDisable() => Unsubscribe();

        private void Subscribe()
        {
            _shopWallet.OnChanged += ShopWallet_OnScoreChanged;
            _buyButton.OnClicked += BuyButton_OnClicked;
        }
        private void Unsubscribe()
        {
            _shopWallet.OnChanged -= ShopWallet_OnScoreChanged;
            _buyButton.OnClicked += BuyButton_OnClicked;
        }

        protected virtual void BuyButton_OnClicked()
        {
            if (_shopWallet.CanSpend(_shopItemData.Price))
                _shopWallet.Spend(_shopItemData.Price);
            
        }


        private void ShopWallet_OnScoreChanged(object sender, DataBankEvent<int> dataBankEvent)
        {
            if (dataBankEvent.CurrentValue < _shopItemData.Price)
                DisableBuyButton();
            else
                EnableBuyButton();
        }

        private void DisableBuyButton() 
            => _buyButton.SetInteractableState(false);

        private void EnableBuyButton() 
            => _buyButton.SetInteractableState(true);
        
        public virtual void Init(IDataBank<int> scoreBank, TShopItem shopItemData)
        {
            _shopItemData = shopItemData;
            _shopWallet = scoreBank;

            SetPrice(_shopItemData.Price);
        }

        protected virtual void SetPrice(int value) 
            => _priceText.text = _pricePrefix + value + _priceSufix;
    }
}