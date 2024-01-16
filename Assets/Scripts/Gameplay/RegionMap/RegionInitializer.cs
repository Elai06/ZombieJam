using System;
using Infrastructure.PersistenceProgress;
using Infrastructure.StaticData;
using UnityEngine;
using Zenject;

namespace Gameplay.RegionMap
{
    public class RegionInitializer : MonoBehaviour
    {
        private IProgressService _progressService;
        private GameStaticData _gameStaticData;

        [Inject]
        private void Construct(IProgressService progressService, GameStaticData gameStaticData)
        {
            _progressService = progressService;
            _gameStaticData = gameStaticData;
        }
        
        public void Initialize()
        {
            var regionProgress = _progressService.PlayerProgress.RegionProgress;
            foreach (var data in _gameStaticData.RegionConfig.ConfigData)
            {
                regionProgress.GetOrCreate(data.RegionType);
            }
        }
    }
}