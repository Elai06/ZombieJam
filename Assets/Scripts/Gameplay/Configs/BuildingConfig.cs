using System.Collections.Generic;
using Gameplay.Enums;
using UnityEngine;

namespace Gameplay.Configs
{
    [CreateAssetMenu(fileName = "BuildingConfig", menuName = "Configs/BuildingConfig")]
    public class BuildingConfig : ScriptableObject
    {
        [SerializeField] private List<BuldingConfigData> _config;

        public BuldingConfigData GetBuildingConfig(EBuildingType type)
        {
            return _config.Find(x => x.Type == type);
        }
    }
}