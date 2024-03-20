using Gameplay.Enums;
using Gameplay.Parameters;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Configs.Zombies
{
    [CreateAssetMenu(menuName = "Configs/ZombieConfig/ZombieData", fileName = "ZombieData", order = 0)]
    public class ZombieData : ScriptableObject
    {
        public EZombieNames Name;
        public EUnitClass Type;
        public Unit Prefab;
        public ParametersConfig Parameters;
        public EZombieSize ZombieSize;
    }
}