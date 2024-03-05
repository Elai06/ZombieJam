using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Ad;
using Gameplay.Configs.Boosters;
using Gameplay.Curencies;
using Gameplay.Enums;
using Gameplay.Windows.Gameplay;
using Infrastructure.PersistenceProgress;
using Infrastructure.StaticData;
using UnityEngine;

namespace Gameplay.Boosters
{
    public class BoostersManager : IBoostersManager
    {
        private const int PRICE_BOOSTER = 5;

        public event Action<EBoosterType> Activate;

        private readonly IProgressService _progressService;
        private IGameplayModel _gameplayModel;
        private ICurrenciesModel _currenciesModel;

        private BoostersProgress _boostersProgress;
        private BoostersConfig _boostersConfig;

        private Dictionary<EBoosterType, bool> _boosters = new Dictionary<EBoosterType, bool>();

        public BoostersManager(IProgressService progressService, GameStaticData gameStaticData,
            ICurrenciesModel currenciesModel)
        {
            _progressService = progressService;
            _boostersConfig = gameStaticData.BoostersConfig;
            _currenciesModel = currenciesModel;

            progressService.OnLoaded += Loaded;
        }

        public void Initialize(IGameplayModel gameplayModel)
        {
            _gameplayModel = gameplayModel;

            _gameplayModel.OnStartWave += ResetBoosters;
        }

        private void Loaded()
        {
            _progressService.OnLoaded -= Loaded;

            _boostersProgress = _progressService.PlayerProgress.BoostersProgress;

            foreach (var boosterProgressData in _boostersConfig.Boosters)
            {
                _boosters.Add(boosterProgressData.BoosterType, false);
            }

            _boosters.Add(EBoosterType.Relocation, false);
        }

        public void ActivateBooster(EBoosterType boosterType)
        {
            if (_boosters[boosterType] && boosterType != EBoosterType.Relocation) return;

            var progress = _boostersProgress.GetOrCreate(boosterType);
            if (progress.Value <= 0)
            {
                var hardCurrencyProgress =
                    _currenciesModel.CurrenciesProgress.GetOrCreate(ECurrencyType.HardCurrency);
                /*if (_adsService.ShowAds(EAdsType.Reward))
                {
                    _adsService.Showed += () => OnShowedAds(boosterType);
                    return;
                }*/


                if (hardCurrencyProgress.Value >= PRICE_BOOSTER)
                {
                    _currenciesModel.Consume(ECurrencyType.HardCurrency, PRICE_BOOSTER);
                    AddBooster(boosterType, 1);
                    _boosters[boosterType] = true;
                    Activate?.Invoke(boosterType);
                    return;
                }

                Debug.Log($"{boosterType} not available");
                return;
            }

            _boosters[boosterType] = true;

            Activate?.Invoke(boosterType);
        }

        private void OnShowedAds(EBoosterType type)
        {
            // _adsService.Showed -= () => OnShowedAds(type);

            AddBooster(type, 1);

            _boosters[type] = true;

            Activate?.Invoke(type);
        }

        public void ConsumeBooster(EBoosterType boosterType, int value)
        {
            var progress = _boostersProgress.GetOrCreate(boosterType);
            progress.Value -= value;
        }

        public void AddBooster(EBoosterType boosterType, int value)
        {
            var progress = _boostersProgress.GetOrCreate(boosterType);
            progress.Value += value;
        }

        public BoosterProgressData GetBoosterProgressData(EBoosterType eBoosterType)
        {
            return _boostersProgress.GetOrCreate(eBoosterType);
        }

        private void ResetBoosters(ERegionType regionType ,int waveIndex)
        {
            for (int i = 0; i < _boosters.Count; i++)
            {
                var boosterType = _boosters.ToList()[i].Key;
                _boosters[boosterType] = false;
            }
        }
    }
}