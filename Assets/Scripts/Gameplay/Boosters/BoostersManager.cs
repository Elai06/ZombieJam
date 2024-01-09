using System;
using Gameplay.Ad;
using Infrastructure.PersistenceProgress;
using UnityEngine;
using Zenject;

namespace Gameplay.Boosters
{
    public class BoostersManager : IBoostersManager
    {
        public event Action<EBoosterType> Activate;

        private readonly BoostersProgress _boostersProgress;
        private readonly IAdsService _adsService;

        [Inject]
        public BoostersManager(IProgressService progressService, IAdsService adsService)
        {
            _adsService = adsService;
            _boostersProgress = progressService.PlayerProgress.BoostersProgress;
        }

        public void ActivateBooster(EBoosterType boosterType)
        {
            var progress = _boostersProgress.GetOrCreate(boosterType);

            if (progress.Value <= 0)
            {
                if (_adsService.ShowAds(EAdsType.Reward))
                {
                    _adsService.Showed += () => OnShowedAds(boosterType);
                    return;
                }

                Debug.Log($"{boosterType} not available");
                return;
            }

            Activate?.Invoke(boosterType);
        }

        private void OnShowedAds(EBoosterType type)
        {
            _adsService.Showed -= () => OnShowedAds(type);

            AddBooster(type, 1);
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
    }
}