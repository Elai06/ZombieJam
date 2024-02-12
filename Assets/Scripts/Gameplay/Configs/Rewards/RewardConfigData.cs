using System;
using Gameplay.Boosters;
using Gameplay.Configs.Zombies;
using Gameplay.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gameplay.Configs.Rewards
{
    [Serializable]
    public struct RewardConfigData
    {
        public EResourceType RewardType;

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
                _ => ""
            };
        }
    }
}