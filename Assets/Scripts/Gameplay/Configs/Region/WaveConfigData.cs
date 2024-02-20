using System;
using Gameplay.Configs.Rewards;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gameplay.Configs.Region
{
    [Serializable]
    public struct WaveConfigData
    {
        public GameObject Prefab;
        public EWaveTimerType WaveTimerType;
       [ShowIf("IsAvailableTimerDuration")] public int TimerDuration;
        public RewardConfig RewardConfig;

        private bool IsAvailableTimerDuration()
        {
            return WaveTimerType != EWaveTimerType.None;
        }
    }
}