using System.Collections.Generic;
using Gameplay.Enums;
using UnityEngine;

namespace Gameplay.Configs.Region
{
    [CreateAssetMenu(fileName = "RegionConfig", menuName = "Configs/Region/RegionConfig")]
    public class RegionConfig : ScriptableObject
    {
        [SerializeField] private List<RegionConfigData> _regionConfigData;

        public List<RegionConfigData> ConfigData => _regionConfigData;

        public RegionConfigData GetRegionConfig(ERegionType regionType)
        {
            return ConfigData.Find(x => x.RegionType == regionType);
        }
    }
}