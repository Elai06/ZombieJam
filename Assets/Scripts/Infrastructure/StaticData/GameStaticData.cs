using Gameplay.Configs;
using Gameplay.Configs.Boosters;
using Gameplay.Configs.Cards;
using Gameplay.Configs.Level;
using Gameplay.Configs.Region;
using Gameplay.Configs.Shop;
using Gameplay.Configs.Sprites;
using Infrastructure.Windows;
using UnityEngine;
using Zenject;

namespace Infrastructure.StaticData
{
    [CreateAssetMenu(fileName = "GameStaticData", menuName = "Configs/GameStaticData")]
    public class GameStaticData : ScriptableObjectInstaller
    {
        [SerializeField] private WindowsStaticData _windowsStaticData;
        [SerializeField] private RegionConfig _regionConfig;
        [SerializeField] private SpritesConfig spritesSpritesConfig;
        [SerializeField] private LevelConfig _levelConfig;
        [SerializeField] private ZombieConfig _zombieConfig;
        [SerializeField] private CardsConfig _cardsConfig;
        [SerializeField] private ShopConfig _shopConfig;
        [SerializeField] private AdsConfig _adsConfig;
        [SerializeField] private BoostersConfig _boostersConfig;

        public RegionConfig RegionConfig => _regionConfig;

        public SpritesConfig SpritesConfig => spritesSpritesConfig;

        public LevelConfig LevelConfig => _levelConfig;

        public ZombieConfig ZombieConfig => _zombieConfig;

        public CardsConfig CardsConfig => _cardsConfig;

        public ShopConfig ShopConfig => _shopConfig;

        public AdsConfig AdsConfig => _adsConfig;

        public BoostersConfig BoostersConfig => _boostersConfig;

        public WindowData GetWindowData(WindowType windowType)
        {
            return _windowsStaticData.GetWindows().Find(x => x.WindowType == windowType);
        }
    }
}