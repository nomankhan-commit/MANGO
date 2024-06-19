

using Mango.Service.AuthAPI.Model;
using Mango.Services.AuthApi;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;


namespace Mango.Services.AuthApi.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUsers>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {

        }
        

        public DbSet<ApplicationUsers> ApplicationUsers { get; set; }   


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
           
            
            
        }
    }
}
