using Core.Mechanics.Shops.Model;
using UnityEngine;

namespace Core.Mechanics.Shops.Asset
{
    [CreateAssetMenu(fileName = "ShopItemAsset")]
    public class ShopItemDataAsset : GuidAsset,IShopItemData
    {
        [field: SerializeField]
        public int Price { get; private set; }
    }

    public class GuidAsset: ScriptableObject
    {
        [field: SerializeField]
        public string Key { get; private set; }
    }
}