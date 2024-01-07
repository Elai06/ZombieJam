using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Configs.Rewards
{
    [CreateAssetMenu(fileName = "RewardConfig", menuName = "Configs/Rewards/RewardConfig")]
    public class RewardConfig : ScriptableObject
    {
        [SerializeField] private List<RewardConfigData> _rewardConfig;

        public List<RewardConfigData> Rewards => _rewardConfig;
    }
}