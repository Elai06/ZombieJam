using System;
using System.Collections.Generic;
using Gameplay.Boosters;
using Gameplay.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gameplay.Configs.Rewards
{
    [CreateAssetMenu(fileName = "RewardConfig", menuName = "Configs/Rewards/RewardConfig")]
    public class RewardConfig : ScriptableObject
    {
        [SerializeField] private List<RewardConfigData> _rewardConfig;

        public List<RewardConfigData> Rewards => _rewardConfig;
    }

    [Serializable]
    public struct RewardConfigData
    {
        public EResourceType RewardType;
        [SerializeField, ShowIf("ShowZombie")] private EZombieType _zombieType;

        [SerializeField, ShowIf("ShowCurrency")]
        private ECurrencyType _currencyType;

        [SerializeField, ShowIf("ShowBooster")]
        private EBoosterType _boosterType;

        public int Value;

        private bool ShowZombie()
        {
            return RewardType == EResourceType.Card;
        }

        private bool ShowCurrency()
        {
            return RewardType == EResourceType.Currency;
        }

        private bool ShowBooster()
        {
            return RewardType == EResourceType.Booster;
        }

        public string GetId()
        {
            return RewardType switch
            {
                EResourceType.Booster => _boosterType.ToString(),
                EResourceType.Currency => _currencyType.ToString(),
                EResourceType.Card => _zombieType.ToString(),
                _ => ""
            };
        }
    }
}