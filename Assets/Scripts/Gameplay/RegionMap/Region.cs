using Gameplay.Configs.Region;
using Gameplay.Enums;
using TMPro;
using UnityEngine;

namespace Gameplay.RegionMap
{
    public class Region : MonoBehaviour
    {
        [SerializeField] private ERegionType _regionType;
        [SerializeField] private TextMeshProUGUI _regionName;

        private RegionConfigData _regionData;
        private RegionProgressData _regionProgressData;

        private bool _isSelected;
        private Material _material;

        public ERegionType RegionType => _regionType;

        private void Awake()
        {
            _material = transform.GetChild(transform.childCount - 1).GetComponent<Renderer>().material;
        }

        public void Initialize(RegionProgressData regionProgressData, bool IsSelected)
        {
            _regionProgressData = regionProgressData;
            _isSelected = IsSelected;
            _regionName.text = regionProgressData.ERegionType.ToString();

            if (_regionProgressData.IsCompleted)
            {
                _material.color = Color.green;
            }
            else if (!IsSelected)
            {
                _material.color = Color.blue;
            }

            if (_isSelected)
            {
                _material.color = Color.yellow;
            }
        }
    }
}