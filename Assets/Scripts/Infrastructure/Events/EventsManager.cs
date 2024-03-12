using Gameplay.Ad;
using Gameplay.Boosters;
using Gameplay.Cards;
using Gameplay.Configs.Region;
using Gameplay.Configs.Zombies;
using Gameplay.Enums;
using Gameplay.Level;
using Gameplay.PlayerTimes;
using Gameplay.Shop;
using Gameplay.Windows.Gameplay;
using Infrastructure.Windows;
using UnityEngine;

namespace Infrastructure.Events
{
    public class EventsManager : IEventsManager
    {
        private readonly IWindowService _windowService;
        private readonly IPlayerTimesService _playerTimesService;
        private readonly ILevelModel _levelModel;
        private readonly IRegionManager _regionManager;
        private readonly IShopModel _shopModel;
        private readonly ICardsModel _cardsModel;
        private readonly IAdsService _adsService;
        private readonly IBoostersManager _boostersManager;
        private IGameplayModel _gameplayModel;

        public EventsManager(IWindowService windowService, IPlayerTimesService playerTimesService,
            ILevelModel levelModel, IRegionManager regionManager, IShopModel shopModel, ICardsModel cardsModel,
            IAdsService adsService, IBoostersManager boostersManager)
        {
            _windowService = windowService;
            _playerTimesService = playerTimesService;
            _levelModel = levelModel;
            _regionManager = regionManager;
            _shopModel = shopModel;
            _cardsModel = cardsModel;
            _adsService = adsService;
            _boostersManager = boostersManager;
        }

        public void Initialize(IGameplayModel gameplayModel)
        {
            _gameplayModel = gameplayModel;

            _windowService.OnOpen += OpenWindow;
            _levelModel.OnLevelUp += OnLevelUp;
            _shopModel.Purchased += OnPurchase;
            _cardsModel.UpgradeSucced += OnUpgradeCard;
            _cardsModel.StartUpgrade += OnStartUpgrade;
            _adsService.StartShow += OnStartAds;
            _adsService.Showed += OnAdsShowed;
            _boostersManager.Activate += OnBoosterActivate;
            _gameplayModel.OnStartWave += WaveStarted;
            _gameplayModel.OnWaveCompleted += WaveCompleted;
            _gameplayModel.OnWaveLoose += WaveLoosed;

            //GameLoaded
            AppMetrica.Instance.ReportEvent("Game Loaded",
                $"{{\"Project version\":\"{Application.version}\\, " +
                $"Day\":\"{_playerTimesService.GetDaysInPlay()}\"}}");
        }

        private void WaveLoosed(ERegionType regionType, int index)
        {
            var regionProgress = _regionManager.Progress;
            var parameters = $"{{\"level_number\":\"{regionProgress.WaveIndex + 1}\", " +
                             $"\"level_name\":\"{regionType}\"," +
                             $"\"level_count\":\"{regionProgress.LevelStartCount}\"}}";
            SendEvent("level_failed", parameters);
        }

        private void WaveCompleted(ERegionType regionType, int index)
        {
            var regionProgress = _regionManager.Progress;
            var parameters = $"{{\"level_number\":\"{regionProgress.WaveIndex + 1}\", " +
                             $"\"level_name\":\"{regionType}\"," +
                             $"\"level_count\":\"{regionProgress.LevelStartCount}\", " +                       
                             $"\"time\":\"{_gameplayModel.GameplayTimeDuration}\"}}";
            SendEvent("level_finish", parameters);
        }

        private void WaveStarted(ERegionType regionType, int index)
        {
            var regionProgress = _regionManager.Progress;

            var parameters = $"{{\"level_number\":\"{regionProgress.WaveIndex + 1}\", " +
                             $"\"level_name\":\"{regionType}\"," +
                             $"\"level_count\":\"{regionProgress.LevelStartCount}\"}}";
            SendEvent("level_start", parameters);
        }

        private void OnStartAds(EAdsType adsType)
        {
            var parameters =
                $"{{\"ad_type\":\"{adsType}\", " +
                $"\"result\":\"{"start"}\", " +
                $"\"connection\":\"{Application.internetReachability != NetworkReachability.NotReachable}\"}}";

            SendEvent("video_ads_started", parameters);
        }

        private void OnAdsShowed(EAdsType adsType)
        {
            var parameters =
                $"{{\"ad_type\":\"{adsType}\", " +
                $"\"result\":\"{"watched"}\", " +
                $"\"connection\":\"{Application.internetReachability != NetworkReachability.NotReachable}\"}}";

            var regionProgress = _regionManager.ProgressData;
            parameters += $"{{\"level_number\":\"{regionProgress.CurrentWaweIndex + 1}\", " +
                          $"\"level_name\":\"{regionProgress.ERegionType},{regionProgress.CurrentWaweIndex + 1}\"," +
                          $"\"level_count\":\"{_regionManager.Progress.LevelStartCount}\"}}";

            SendEvent("video_ads_watch", parameters);
        }


        public void SendEvent(string eventName, string parameters = "")
        {
            AppMetrica.Instance.ReportEvent(eventName, parameters);
            Debug.Log($"Send appmetrica event {eventName}");
        }

        public void SendEventDay(string eventName, string parameters)
        {
            parameters +=
                $"{{\"Day\":\"{_playerTimesService.GetDaysInPlay()}\"}}";

            AppMetrica.Instance.ReportEvent(eventName, parameters);
            Debug.Log($"Send appmetrica event {eventName}");
        }

        private void OnStartUpgrade(EZombieNames unitClass)
        {
            var regionProgress = _regionManager.ProgressData;
            var parameters =
                $"{{\"UnitType\":\"{unitClass}\", " +
                $"\"RegionType\":\"{regionProgress.ERegionType}\", " +
                $"\"WaveIndex\":\"{regionProgress.CurrentWaweIndex + 1}\"}}";

            SendEvent("Button unit upgrade", parameters);
        }

        private void OnUpgradeCard(EZombieNames unitClass)
        {
            var regionProgress = _regionManager.ProgressData;
            var parameters =
                $"{{\"UnitType\":\"{unitClass}\", " +
                $"\"RegionType\":\"{regionProgress.ERegionType}\", " +
                $"\"WaveIndex\":\"{regionProgress.CurrentWaweIndex + 1}\"}}";
            SendEvent("Unit upgrade", parameters);
        }

        private void OnLevelUp(int level)
        {
            var parameters = $"{{\"Level\":\"{_levelModel.CurrentLevel + 1}\"}}";
            SendEvent("Player levelup", parameters);
        }

        private void OpenWindow(WindowType windowType)
        {
            switch (windowType)
            {
                case WindowType.Shop:
                    SendEvent("Tab Shop");
                    break;
                case WindowType.Region:
                    SendEvent("Tab Map");
                    break;
                case WindowType.Lobby:
                    SendEvent("Tab Waves");
                    break;
                case WindowType.Cards:
                    SendEvent("Tab Collection");
                    break;
            }
        }

        private void OnPurchase(EShopProductType productType)
        {
            SendEvent(productType.ToString(), $"{{\"ProductType\":\"{productType}\"}}");
        }

        private void OnBoosterActivate(EBoosterType boosterType)
        {
            var parameters =
                $"{{\"BoosterActivated\":\"{boosterType}\"}}";
            SendEvent(parameters);
        }
    }
}