using System;
using System.Collections.Generic;
using Gameplay.Reward;
using Infrastructure.Windows.MVVM;

namespace Gameplay.Windows.Rewards
{
    public class RewardsViewModel : ViewModelBase<IRewardModel, RewardsView>
    {
        public RewardsViewModel(IRewardModel model, RewardsView view) : base(model, view)
        {
        }

        public override void Show()
        {
            InitializeRewards();
        }

        public override void Subscribe()
        {
            base.Subscribe();

            View.RewardsClick += Model.GetRewards;
        }

        public override void Unsubscribe()
        {
            base.Unsubscribe();

            View.RewardsClick -= Model.GetRewards;
        }

        private void InitializeRewards()
        {
            var rewardsViewData = new List<RewardSubViewData>();
            foreach (var reward in Model.RewardDatas)
            {
                var viewData = new RewardSubViewData
                {
                    ID = reward.ID,
                    Value = reward.Value,
                    //   Sprite = 
                };

                rewardsViewData.Add(viewData);
            }

            View.InititializeReward(rewardsViewData, Model.Description);
        }
    }
}