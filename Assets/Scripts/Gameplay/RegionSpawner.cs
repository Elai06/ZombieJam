using Gameplay.Configs.Region;
using Infrastructure.PersistenceProgress;
using Infrastructure.Windows;
using UnityEngine;
using Utils.ZenjectInstantiateUtil;
using Zenject;

namespace Gameplay
{
    public class RegionSpawner : MonoBehaviour
    {
        private IRegionManager _regionManager;
        private IWindowService _windowService;

        private GameObject _currentLevel;

        [Inject]
        public void Construct(IRegionManager regionManager, IWindowService windowService)
        {
            _regionManager = regionManager;
            _windowService = windowService;
        }

        private void Start()
        {
            InjectService.Instance.Inject(this);

            InstantiateLevel();
        }

        private void InstantiateLevel()
        {
            if (_currentLevel != null)
            {
                Destroy(_currentLevel);
            }

            var progress = _regionManager.ProgressData;
            var regionConfig = _regionManager.GetActualRegion(progress.ERegionType);
            var waveIndex = progress.CurrentWaweIndex;
            _currentLevel = Instantiate(regionConfig.Waves[waveIndex].Prefab, Vector3.zero, Quaternion.identity);

            if (_windowService.IsOpen(WindowType.Vignette))
            {
                _windowService.Close(WindowType.Vignette);
            }
        }
    }
}