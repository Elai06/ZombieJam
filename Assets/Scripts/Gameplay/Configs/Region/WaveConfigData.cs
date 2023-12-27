using System;
using Gameplay.Configs.Rewards;
using UnityEngine;

namespace Gameplay.Configs.Region
{
    [Serializable]
    public struct WaveConfigData
    {
        public GameObject Prefab;
        public CurrencyRewardConfig currencyRewardConfig;
    }
}