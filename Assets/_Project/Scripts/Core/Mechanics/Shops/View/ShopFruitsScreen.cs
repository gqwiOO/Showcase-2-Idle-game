using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Mechanics.Shops.Asset;
using Core.Mechanics.Shops.Example;
using Core.Mechanics.Shops.Provider;
using Core.Scripts.Pools;
using Core.Storage.Bank;
using Cysharp.Threading.Tasks;
using Services.Screen;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Core.Mechanics.Shops.View
{
    public class ShopFruitsScreen: BaseScreen
    {
        [SerializeField] private List<ShopItemDataAsset> _itemsAssets;

        [SerializeField] private bool _updateOnOpen;
        
        [SerializeField] private PoolGameObjects _pool;
        
        [ReadOnly]
        [SerializeField] private List<FruitShopItemView> _shopItems;

        [SerializeField] private RectTransform _itemsContainer;
        
        private IDataBank<int> _scoreBank;
        private IBanksProvidersProvider _scoreBanksProvider;


        [Inject]
        private void Construct(IBanksProvidersProvider scoreBanksProvider)
        {
            _scoreBanksProvider = scoreBanksProvider;
        }

        private void Start()
        {
            _scoreBank = _scoreBanksProvider.GetIntBankProvider().Get(BankId.FruitsBank);
        }

        public override async UniTask Open()
        {
            await base.Open();
            if (_updateOnOpen)
            {
                _pool.Init();
                await Init();
            }
        }

        private void Clear()
        {
            _shopItems?.ForEach(item => _pool.Push(item.PoolObject));
            _shopItems?.Clear();
        }

        private async Task Init()
        {
            _itemsAssets.ForEach(item =>
            {
                var itemView = _pool.Pull().GetOwner<FruitShopItemView>();
                itemView.transform.SetParent(_itemsContainer);
                itemView.Init(_scoreBank, item);
                itemView.gameObject.SetActive(true);

            });
        }
    }
}