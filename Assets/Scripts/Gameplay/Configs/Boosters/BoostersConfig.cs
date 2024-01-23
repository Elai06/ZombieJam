using System.Collections.Generic;
using Gameplay.Boosters;
using UnityEngine;

namespace Gameplay.Configs.Boosters
{
    [CreateAssetMenu(menuName = "Configs/BoostersConfig", fileName = "BoostersConfig", order = 0)]
    public class BoostersConfig : ScriptableObject
    {
        public List<BoosterConfigData> Boosters;

        public BoosterConfigData GetBoosterConfig(EBoosterType type)
        {
            return Boosters.Find(x => x.BoosterType == type);
        }
    }
}