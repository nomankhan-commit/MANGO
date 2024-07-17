using AutoMapper;
using Mango.Services.CouponAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.CouponAPI
{
    public class ServiceRegistrationHelper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            IMapper mapper = MappingConfig.RegisterMapper().CreateMapper();
            services.AddSingleton(mapper);
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddAuthorization();
            services.AddControllers();
          
        }
    }
}
