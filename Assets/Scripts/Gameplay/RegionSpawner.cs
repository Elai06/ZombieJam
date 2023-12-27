using Gameplay.Configs.Region;
using Infrastructure.PersistenceProgress;
using UnityEngine;
using Utils.ZenjectInstantiateUtil;
using Zenject;

namespace Gameplay
{
    public class RegionSpawner : MonoBehaviour
    {
        private IProgressService _progressService;
        private IRegionManager _regionManager;
        
        private GameObject _currentLevel;

        [Inject]
        public void Construct(IProgressService progressService, IRegionManager regionManager)
        {
            _progressService = progressService;
            _regionManager = regionManager;
        }

        private void Start()
        {
            InjectService.Instance.Inject(this);

            InstatiateLevel();
        }

        private void InstatiateLevel()
        {
            if (_currentLevel != null)
            {
                Destroy(_currentLevel);
            }

            var progress = _progressService.PlayerProgress.RegionProgress.GetCurrentRegion();
            var regionConfig = _regionManager.GetActualRegion(progress.ERegionType);
            var waveIndex = progress.CurrentWaweIndex;
            _currentLevel = Instantiate(regionConfig.Waves[waveIndex].Prefab, Vector3.zero, Quaternion.identity);
        }
    }
}