using System;
using Gameplay.Enums;
using Gameplay.Parameters;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Configs
{
    [Serializable]
    public struct ZombieData
    {
        public EUnitClass Type;
        public Unit Prefab;
        public ParametersConfig Parameters;
    }
}