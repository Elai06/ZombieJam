using System;
using Gameplay.Enums;

namespace Gameplay.Parameters
{
    [Serializable]
    public struct ParameterData
    {
        public EParameter Type;
        public float Value;
        public float MultiplierForUpgrade;
    }
}