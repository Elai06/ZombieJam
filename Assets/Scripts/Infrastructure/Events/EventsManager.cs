using System;
using System.Collections.Generic;
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

        public EventsManager(IWindowService windowService, IPlayerTimesService playerTimesService,
            ILevelModel levelModel, IGameplayModel gameplayModel, IShopModel shopModel, ICardsModel cardsModel)
        {
            _windowService = windowService;
            _playerTimesService = playerTimesService;
            _levelModel = levelModel;
            _gameplayModel = gameplayModel;
            _shopModel = shopModel;
            _cardsModel = cardsModel;
        }

        public void Initialize()
        {
            _windowService.OnOpen += OpenWindow;
            _levelModel.UpdateExperience += OnLevelUp;
            _shopModel.Purchased += OnPurchase;
            _cardsModel.UpgradeSucced += OnUpgradeCard;
            _cardsModel.StartUpgrade += OnStartUpgrade;

            //GameLoaded
            AppMetrica.Instance.ReportEvent("Game Loaded", $"{{\"day\":\"{_playerTimesService.GetDaysInPlay()}\"}}");
        }

        private void OnStartUpgrade(EZombieType zombieType)
        {
            var regionProgress = _gameplayModel.GetCurrentRegionProgress().GetCurrentRegion();
            var parameters =
                $"{{\"UnitType\":\"{zombieType}\", "+
                $"\"RegionType\":\"{regionProgress.ERegionType}\", " +
                $"\"WaveIndex\":\"{regionProgress.CurrentWaweIndex}\"}}";
            
            SendEvent("Button unit upgrade", parameters);
        }

        private void OnUpgradeCard(EZombieType zombieType)
        {
            var regionProgress = _gameplayModel.GetCurrentRegionProgress().GetCurrentRegion();
            var parameters =
                $"{{\"UnitType\":\"{zombieType}\", "+
                $"\"RegionType\":\"{regionProgress.ERegionType}\", " +
                $"\"WaveIndex\":\"{regionProgress.CurrentWaweIndex}\"}}";
            SendEvent("Unit upgrade", parameters);
        }

        private void OnLevelUp(LevelProgress levelProgress)
        {
            var regionProgress = _gameplayModel.GetCurrentRegionProgress().GetCurrentRegion();
            var parametrs =
                $"{{\"RegionType\":\"{regionProgress.ERegionType}\", " +
                $"\"WaveIndex\":\"{regionProgress.CurrentWaweIndex}\"}}";

            SendEvent("Player levelup", parametrs);
        }

        private void SendEvent(string eventName, string parameters = "")
        {
            parameters += $"{{\"Level\":\"{_levelModel.CurrentLevel}\", \"Day\":\"{_playerTimesService.GetDaysInPlay()}\"}}";

            AppMetrica.Instance.ReportEvent(eventName, parameters);
            Debug.Log($"Send appmetrica event {eventName}");
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
    }
}