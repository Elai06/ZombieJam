using System;
using Gameplay.Parameters;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Configs
{
    [Serializable]
    public struct ZombieData
    {
        public EZombieType Type;
        public Unit Prefab;
        public ParametersConfig Parameters;
    }
}