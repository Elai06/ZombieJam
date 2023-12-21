using System;
using System.Collections.Generic;
using Gameplay.Enums;
using UnityEngine;

namespace Gameplay.Configs.Region
{
    [Serializable]
    public struct RegionConfigData
    {
        public ERegionType RegionType;
        public List<GameObject> WavePrefabs;
    }
}