using System.Collections.Generic;
using Gameplay.Configs.Rewards;
using UnityEngine;

namespace Gameplay.Configs.Level
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "Configs/LevelConfig")]
    public class LevelConfig : ScriptableObject
    {
        [SerializeField] private int _reqiredExperienceForUp;
        [SerializeField] private int _experienceForWin;
        [SerializeField] private int _experienceForLoose;

        [SerializeField] private float multiplierExperience;

        [SerializeField] private List<RewardConfig> _levelRewards;

        public List<RewardConfig> LevelRewards => _levelRewards;

        public float MultiplierExperience => multiplierExperience;

        public int ReqiredExperienceForUp => _reqiredExperienceForUp;

        public int ExperienceForLoose => _experienceForLoose;

        public int ExperienceForWin => _experienceForWin;
    }
}