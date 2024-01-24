using System;
using Gameplay.Enums;
using Gameplay.Parameters;
using Gameplay.Units;

namespace Gameplay.Configs.Zombies
{
    [Serializable]
    public struct ZombieData
    {
        public EUnitClass Type;
        public Unit Prefab;
        public ParametersConfig Parameters;
        public EZombieSize ZombieSize;
    }

    public enum EZombieSize
    {
        SingleCell,
        TwoCells
    }
}