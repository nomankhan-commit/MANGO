using AutoMapper;
using Mango.Service.ProductApi.Data;
using Microsoft.EntityFrameworkCore;

namespace Mango.Service.ProductApi
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
