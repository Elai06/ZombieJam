using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Configs.Rewards
{
    [CreateAssetMenu(fileName = "BoosterRewardConfig", menuName = "Configs/Rewards/BoosterRewardConfig")]
    public class BoosterRewardConfig : ScriptableObject
    {
        [SerializeField]private List<BoosterRewardConfigData> _boostersRewardConfig = new();

        public List<BoosterRewardConfigData> BoosterRewards => _boostersRewardConfig;
    }
}