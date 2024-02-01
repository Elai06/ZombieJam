using System.Collections.Generic;
using Gameplay.Configs.Zombies;
using Gameplay.Enums;
using UnityEngine;

namespace Gameplay.Configs.Sprites
{
    [CreateAssetMenu(fileName = "SpritesConfig", menuName = "Configs/GameStaticData/SpritesConfig")]
    public class SpritesConfig : ScriptableObject
    {
        [SerializeField] private List<CurrencySprites> _currencySprites;
        [SerializeField] private List<ShopSprites> _shopSprites;
        [SerializeField] private List<ZombieCardsBackground> _cardsBackgrounds;
        [SerializeField] private List<ZombieIconCards> _zombieIcons;
        [SerializeField] private List<ClassIcon> _classIcons;

        public Sprite GetCurrencySprite(ECurrencyType type)
        {
            return _currencySprites.Find(x => x.Type == type).Sprite;
        }

        public Sprite GetShopSprite(EShopProductType type)
        {
            return _shopSprites.Find(x => x.ProductType == type).Sprite;
        }

        public Sprite GetZombieIcon(EZombieNames type)
        {
            return _zombieIcons.Find(x => x.ZombieName == type).Icon;
        }

        public Sprite GetCardsBackground(EUnitClass type)
        {
            return _cardsBackgrounds.Find(x => x.Class == type).Sprite;
        }
        
        public Sprite GetClassIcon(EUnitClass type)
        {
            return _classIcons.Find(x => x.Class == type).Sprite;
        }
    }
}