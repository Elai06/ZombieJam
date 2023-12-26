using System;
using Infrastructure.PersistenceProgress;
using UnityEngine;
using Zenject;

namespace Gameplay.Boosters
{
    public class BoostersManager : IBoostersManager
    {
        public event Action<EBoosterType> Activate;

        private BoostersProgress _boostersProgress;

        [Inject]
        public void Construct(IProgressService progressService)
        {
            _boostersProgress = progressService.PlayerProgress.BoostersProgress;
        }

        public void ActivateBooster(EBoosterType boosterType)
        {
            var progress = _boostersProgress.GetOrCreate(boosterType);

            if (progress.Value <= 0)
            {
                Debug.Log($"{boosterType} not available");
                return;
            }
            Activate?.Invoke(boosterType);
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
          return  _boostersProgress.GetOrCreate(eBoosterType);
        }
    }
}