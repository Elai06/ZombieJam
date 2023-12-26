using System.Collections.Generic;
using Gameplay.Units;
using Infrastructure.PersistenceProgress;
using UnityEngine;
using Utils.ZenjectInstantiateUtil;
using Zenject;

namespace Gameplay.RegionMap
{
    public class RegionMapManager : MonoBehaviour
    {
        private const int REGION_LAYER = 9;

        [SerializeField] private RegionTooltipView _regionTooltipView;
        [SerializeField] private List<Region> _regions = new();

        [Inject] private IProgressService _progressService;
        private readonly RaycastDetector _raycastDetector = new();

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

        private void SetTouch()
        {
            var contactInfo = _raycastDetector.RayCast(REGION_LAYER);
            if (contactInfo.Collider == null) return;

            var region = contactInfo.Collider.transform.parent.GetComponent<Region>();
            var progressData = _progressService.PlayerProgress.RegionProgress.GetOrCreate(region.RegionType);
            _regionTooltipView.Initialize(progressData);
        }

        private void InitializeRegions()
        {
            var progress = _progressService.PlayerProgress.RegionProgress;
            foreach (var region in _regions)
            {
                var regionProgress = progress.RegionProgressData
                    .Find(x => x.ERegionType == region.RegionType);

                var isSelected = regionProgress.ERegionType == progress.CurrentRegionType;

                region.Initialize(regionProgress, isSelected);
            }
        }
    }
}