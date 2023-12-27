using System;
using Gameplay.Configs.Rewards;
using UnityEngine;

namespace Gameplay.Configs.Level
{
    [Serializable]
    public struct LevelRewards
    {
        [SerializeField] private BoosterRewardConfig _boosterRewardConfig;
        [SerializeField] private CurrencyRewardConfig _currencyRewardConfig;
    }
}