using Gameplay.Reward;
using Infrastructure.Windows;
using UnityEngine;
using Zenject;

namespace Gameplay.Windows.Rewards
{
    public class RewardsWindow : Window
    {
        [SerializeField] private RewardViewInitializer _rewardViewInitializer;
        [Inject] private IRewardModel _rewardModel;

        private void OnEnable()
        {
            _rewardViewInitializer.Initialize(_rewardModel);
        }
    }
}