using Mango.Service.RewardApi.Data;
using Mango.Service.RewardApi.Message;
using Mango.Service.RewardApi.Model;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Mango.Service.RewardApi.Model.Services
{
    public class RewardService : IRewardService
    {
        private DbContextOptions<AppDbContext> options;

        public RewardService(DbContextOptions<AppDbContext> options)
        {
            this.options = options;
        }

        public async Task UpDateReward(RewardMessage rewards)
        {
            try
            {
                Rewards _rewards = new()
                {
                    OrderId = rewards.OrderId,
                    RewardActivitiy = rewards.RewardActivity,
                    UserId = rewards.UserId,
                    RewardDate= DateTime.Now,
                };

                await using var db = new AppDbContext(options);
                await db.Rewards.AddAsync(_rewards);
                await db.SaveChangesAsync();

            }
            catch (Exception ex)
            {

            }
        }
    }
}
