using System.Collections.Generic;
using Gameplay.Boosters;
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
        [SerializeField] private List<ParameterIcon> _parameterIcons;
        [SerializeField] private List<BoosterIcon> _boosterIcons;
        [SerializeField] private List<TargetIcons> _targetIcons;

        public Sprite GetCurrencySprite(ECurrencyType type)
        {
            return _currencySprites.Find(x => x.Type == type).Sprite;
        }

        public Sprite GetShopSprite(EShopProductType type)
        {
            return _shopSprites.Find(x => x.ProductType == type).Sprite;
        }

        public ZombieIconCards GetZombieIcon(EZombieNames type)
        {
            return _zombieIcons.Find(x => x.ZombieName == type);
        }

        public ZombieCardsBackground GetCardsBackground(EUnitClass type)
        {
            return _cardsBackgrounds.Find(x => x.Class == type);
        }

        public Sprite GetClassIcon(EUnitClass type)
        {
            return _classIcons.Find(x => x.Class == type).Sprite;
        }

        public Sprite GetParameterIcon(EParameter type)
        {
            return _parameterIcons.Find(x => x.ParameterType == type).Icon;
        }

        public Sprite GetBoosterIcon(EBoosterType boosterType)
        {
            return _boosterIcons.Find(x => x.BoosterType == boosterType).Sprite;
        }

        public Sprite GetTargetIcon(EWaveType waveType)
        {
            return _targetIcons.Find(x => x.WaveType == waveType).Icon;
        }
    }
}