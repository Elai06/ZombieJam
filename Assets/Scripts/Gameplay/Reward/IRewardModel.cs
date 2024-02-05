using System.Collections.Generic;
using Gameplay.Enums;

namespace Gameplay.Reward
{
    public interface IRewardModel
    {
        void CreateRewards(string description, ERewardType rewardType);
        void AdditionalRewards(EResourceType resourceType, string id, int value);
        string Description { get; set; }
        List<RewardData> RewardDatas { get; set; }
        ERewardType RewardType { get; }
        void ShowRewardWindow();
        void GetRewards();
    }
}