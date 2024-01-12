using System.Collections.Generic;
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
        private IWindowService _windowService;
        private IPlayerTimesService _playerTimesService;
        private ILevelModel _levelModel;
        private IGameplayModel _gameplayModel;
        private IShopModel _shopModel;

        public EventsManager(IWindowService windowService, IPlayerTimesService playerTimesService,
            ILevelModel levelModel, IGameplayModel gameplayModel, IShopModel shopModel)
        {
            _windowService = windowService;
            _playerTimesService = playerTimesService;
            _levelModel = levelModel;
            _gameplayModel = gameplayModel;
            _shopModel = shopModel;
        }

        public void Initialize()
        {
            _windowService.OnOpen += OpenWindow;
            _levelModel.UpdateExperience += OnLevelUp;
            _shopModel.Purchased += OnPurchase;

            //GameLoaded
            AppMetrica.Instance.ReportEvent("Game Loaded", $"{_playerTimesService.GetDaysInPlay()}");
        }

        private void OnLevelUp(LevelProgress levelProgress)
        {
            var regionProgress = _gameplayModel.GetCurrentRegionProgress().GetCurrentRegion();
            var parametrs = $"RegionType {regionProgress.ERegionType} / WaveIndex {regionProgress.CurrentWaweIndex}";


            SendEvent("Player levelup", parametrs);
        }

        private void SendEvent(string eventName, string parameters = "")
        {
            parameters += $"/ Level {_levelModel.CurrentLevel} / Day {_playerTimesService.GetDaysInPlay()}";

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
            SendEvent(productType.ToString(), $"{productType}");
        }
    }
}