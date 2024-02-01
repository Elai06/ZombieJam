using System.Collections.Generic;
using Gameplay.Units;
using Infrastructure.PersistenceProgress;
using Infrastructure.Windows;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils.ZenjectInstantiateUtil;
using Zenject;

namespace Gameplay.RegionMap
{
    public class RegionMapManager : MonoBehaviour
    {
        private const int REGION_LAYER = 9;

        [SerializeField] private RegionTooltipView _regionTooltipView;
        [SerializeField] private RegionCamera _regionCamera;
        [SerializeField] private List<Region> _regions = new();

        [Inject] private IProgressService _progressService;
        [Inject] private IWindowService _windowService;
        private readonly RaycastDetector _raycastDetector = new();

        private int _currentRegion;

        private void Start()
        {
            InjectService.Instance.Inject(this);

            InitializeRegions();
        }

        private void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                SetTouch();
            }
        }

        [Button("MoveCamera")]
        public void MoveCameraNextRegion()
        {
            if (_currentRegion >= _regions.Count - 1) return;
            _currentRegion++;
            var region = _regions[_currentRegion];
        //    _regionCamera.MoveCamera(region.transform);
        }

        private void SetTouch()
        {
            var contactInfo = _raycastDetector.RayCast(REGION_LAYER);
            if (contactInfo.Collider == null) return;

            var region = contactInfo.Collider.transform.parent.GetComponent<Region>();
            var progressData = _progressService.PlayerProgress.RegionProgress.GetOrCreate(region.RegionType);
            /*if (progressData.ERegionType == _progressService.PlayerProgress.RegionProgress.CurrentRegionType)
            {
                SceneManager.LoadScene($"Gameplay");
                _windowService.Open(WindowType.Lobby);
                return;
            }*/

            _regionTooltipView.Initialize(progressData);
        }

        private void InitializeRegions()
        {
            var progress = _progressService.PlayerProgress.RegionProgress;
            for (var index = 0; index < _regions.Count; index++)
            {
                var region = _regions[index];
                var regionProgress = progress.RegionProgressData
                    .Find(x => x.ERegionType == region.RegionType);

                var isSelected = regionProgress.ERegionType == progress.CurrentRegionType;

                region.Initialize(regionProgress, isSelected);

                if (isSelected)
                {
                    var prevRegion = index > 0 ? index - 1 : 0;
                    _regionCamera.MoveCamera(_regions[prevRegion].transform, region.transform);
                }
            }
        }
    }
}