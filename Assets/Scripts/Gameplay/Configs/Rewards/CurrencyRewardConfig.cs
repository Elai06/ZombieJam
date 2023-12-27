using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Configs.Rewards
{
    [CreateAssetMenu(fileName = "CurrencyRewardConfig", menuName = "Configs/Rewards/CurrencyRewardConfig")]
    public class CurrencyRewardConfig : ScriptableObject
    {
        [SerializeField]private List<CurrencyRewardConfigData> _currencyRewards = new();

        public List<CurrencyRewardConfigData> CurrencyRewards => _currencyRewards;
    }
}