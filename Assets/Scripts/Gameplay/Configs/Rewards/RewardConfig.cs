using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Configs.Rewards
{
    [CreateAssetMenu(fileName = "RewardConfig", menuName = "Configs/RewardConfig")]
    public class RewardConfig : ScriptableObject
    {
        [SerializeField]private List<RewardConfigData> _rewards = new();

        public List<RewardConfigData> Rewards => _rewards;
    }
}