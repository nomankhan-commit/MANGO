using Mango.Service.RewardApi.Message;
using Mango.Service.RewardApi.Model.Services;

namespace Mango.Service.RewardApi.Model.Services
{
    public interface IRewardService
    {
        Task UpDateReward(RewardMessage rewardMessage);
    }
}
