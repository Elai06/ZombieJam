using System.Collections.Generic;
using Gameplay.Enums;

namespace Gameplay.Reward
{
    public interface IRewardModel
    {
        void CreateRewards();
        void AdditionalRewards(EResourceType resourceType, string id, int value);
        void GetRewards();
        string Description { get; set; }
        List<RewardData> RewardDatas { get; set; }
    }
}