using System.Collections.Generic;
using Gameplay.Enums;
using UnityEngine;

namespace Gameplay.Configs.Sprites
{
    [CreateAssetMenu(fileName = "SpritesConfig", menuName = "Configs/GameStaticData/SpritesConfig")]
    public class SpritesConfig : ScriptableObject
    {
        [SerializeField] private List<CurrencySprites> _currencySprites;
        [SerializeField] private List<ShopSprites> _shopSprites;
        
        public Sprite GetCurrencySprite(ECurrencyType type)
        {
            return _currencySprites.Find(x => x.Type == type).Sprite;
        }
        
        public Sprite GetShopSprite(EShopProductType type)
        {
            return _shopSprites.Find(x => x.ProductType == type).Sprite;
        }
    }
}