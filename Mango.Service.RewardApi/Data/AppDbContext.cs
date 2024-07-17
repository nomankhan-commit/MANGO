

using Mango.Service.RewardApi.Model;
using Mango.Service.RewardApi.Model;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace Mango.Service.RewardApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Rewards> Rewards { get; set; }

    
    }
}
