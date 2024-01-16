using System;
using System.Collections.Generic;
using Gameplay.Ad;
using Gameplay.Cards;
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
        private readonly IGameplayModel _gameplayModel;
        private readonly IShopModel _shopModel;
        private readonly ICardsModel _cardsModel;
        private readonly IAdsService _adsService;

        public EventsManager(IWindowService windowService, IPlayerTimesService playerTimesService,
            ILevelModel levelModel, IGameplayModel gameplayModel, IShopModel shopModel, ICardsModel cardsModel,
            IAdsService adsService)
        {
            _windowService = windowService;
            _playerTimesService = playerTimesService;
            _levelModel = levelModel;
            _gameplayModel = gameplayModel;
            _shopModel = shopModel;
            _cardsModel = cardsModel;
            _adsService = adsService;
        }

        public void Initialize()
        {
            _windowService.OnOpen += OpenWindow;
            _levelModel.UpdateExperience += OnLevelUp;
            _shopModel.Purchased += OnPurchase;
            _cardsModel.UpgradeSucced += OnUpgradeCard;
            _cardsModel.StartUpgrade += OnStartUpgrade;
            _adsService.StartShow += OnStartAds;
            _adsService.Showed += OnAdsShowed;

            //GameLoaded
            AppMetrica.Instance.ReportEvent("Game Loaded", $"{{\"day\":\"{_playerTimesService.GetDaysInPlay()}\"}}");
        }
        
        private void OnStartAds(EAdsType adsType)
        {
            var regionProgress = _gameplayModel.GetCurrentRegionProgress().GetCurrentRegion();
            var parameters =
                $"{{\"Ads\":\"{adsType}\", " +
                $"\"RegionType\":\"{regionProgress.ERegionType}\", " +
                $"\"WaveIndex\":\"{regionProgress.CurrentWaweIndex}\"}}";

            SendEventWithLevelDay("AdsStarted", parameters);
        }

        private void OnAdsShowed()
        {
            var regionProgress = _gameplayModel.GetCurrentRegionProgress().GetCurrentRegion();
            var parameters =
                $"{{\"Ads\":\"{_adsService.AdsType}\", " +
                $"\"RegionType\":\"{regionProgress.ERegionType}\", " +
                $"\"WaveIndex\":\"{regionProgress.CurrentWaweIndex}\"}}";

            SendEventWithLevelDay("AdsCompleted", parameters);
        }

        
        public void SendEventWithLevelDay(string eventName, string parameters = "")
        {
            parameters +=
                $"{{\"Level\":\"{_levelModel.CurrentLevel}\", \"Day\":\"{_playerTimesService.GetDaysInPlay()}\"}}";

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

        private void OnStartUpgrade(EZombieType zombieType)
        {
            var regionProgress = _gameplayModel.GetCurrentRegionProgress().GetCurrentRegion();
            var parameters =
                $"{{\"UnitType\":\"{zombieType}\", " +
                $"\"RegionType\":\"{regionProgress.ERegionType}\", " +
                $"\"WaveIndex\":\"{regionProgress.CurrentWaweIndex}\"}}";

            SendEventWithLevelDay("Button unit upgrade", parameters);
        }

        private void OnUpgradeCard(EZombieType zombieType)
        {
            var regionProgress = _gameplayModel.GetCurrentRegionProgress().GetCurrentRegion();
            var parameters =
                $"{{\"UnitType\":\"{zombieType}\", " +
                $"\"RegionType\":\"{regionProgress.ERegionType}\", " +
                $"\"WaveIndex\":\"{regionProgress.CurrentWaweIndex}\"}}";
            SendEventWithLevelDay("Unit upgrade", parameters);
        }

        private void OnLevelUp(LevelProgress levelProgress)
        {
            var regionProgress = _gameplayModel.GetCurrentRegionProgress().GetCurrentRegion();
            var parametrs =
                $"{{\"RegionType\":\"{regionProgress.ERegionType}\", " +
                $"\"WaveIndex\":\"{regionProgress.CurrentWaweIndex}\"}}";

            SendEventWithLevelDay("Player levelup", parametrs);
        }

        private void OpenWindow(WindowType windowType)
        {
            switch (windowType)
            {
                case WindowType.Shop:
                    SendEventWithLevelDay("Tab Shop");
                    break;
                case WindowType.Region:
                    SendEventWithLevelDay("Tab Map");
                    break;
                case WindowType.Lobby:
                    SendEventWithLevelDay("Tab Waves");
                    break;
                case WindowType.Cards:
                    SendEventWithLevelDay("Tab Collection");
                    break;
            }
        }

        private void OnPurchase(EShopProductType productType)
        {
            SendEventWithLevelDay(productType.ToString(), $"{{\"ProductType\":\"{productType}\"}}");
        }
    }
}