

using Mango.Services.CouponAPI.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.CouponAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {

        }
        public DbSet<Coupons> coupons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Coupons>().HasData(new Coupons
            {
                CouponID = 1,
                CouponCode = "100FF",
                DiscountAmount = 10,
                MinAmount = 20,
            });
            modelBuilder.Entity<Coupons>().HasData(new Coupons
            {
                CouponID = 2,
                CouponCode = "200FF",
                DiscountAmount = 20,
                MinAmount = 40,
            });
            
            
        }
    }
}
