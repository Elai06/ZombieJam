using System.Collections.Generic;
using Gameplay.Enums;
using UnityEngine;

namespace Gameplay.Configs.Region
{
    [CreateAssetMenu(fileName = "RegionConfig", menuName = "Configs/Region/RegionConfig")]
    public class RegionConfig : ScriptableObject
    {
        [SerializeField] private List<RegionConfigData> _regionConfigData;

        public List<RegionConfigData> GetConfig()
        {
            return _regionConfigData;
        }

        public RegionConfigData GetRegionConfig(ERegionType regionType)
        {
            return _regionConfigData.Find(x => x.RegionType == regionType);
        }
    }
}