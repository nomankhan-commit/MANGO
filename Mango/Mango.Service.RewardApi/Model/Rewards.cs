using System.ComponentModel.DataAnnotations;

namespace Mango.Service.RewardApi.Model
{
    public class Rewards
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime RewardDate { get; set; }
        public int RewardActivitiy { get; set; }
        public int OrderId { get; set; }
    }
}
