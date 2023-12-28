using Gameplay.Reward;
using Infrastructure.Windows.MVVM;

namespace Gameplay.Windows.Rewards
{
    public class RewardViewModelFactory : IViewModelFactory<RewardsViewModel, RewardsView, IRewardModel>
    {
        public RewardsViewModel Create(IRewardModel model, RewardsView view)
        {
            return new RewardsViewModel(model, view);
        }
    }
}