using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Configs.Rewards
{
    [CreateAssetMenu(fileName = "CardRewardConfig", menuName = "Configs/Rewards/CardRewardConfig")]
    public class CardRewardConfig : ScriptableObject
    {
        [SerializeField] private List<CardRewardConfigData> _cardRewardConfig = new();

        public List<CardRewardConfigData> RewardConfig => _cardRewardConfig;
    }
}