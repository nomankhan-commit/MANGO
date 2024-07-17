using AutoMapper;
using Mango.Service.OrderApi.Data;
using Microsoft.EntityFrameworkCore;

namespace Mango.Service.OrderApi
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
