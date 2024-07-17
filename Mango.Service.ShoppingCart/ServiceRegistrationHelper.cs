using AutoMapper;
using Mango.Service.ShoppingCart.Data;
using Microsoft.EntityFrameworkCore;

namespace Mango.Service.ShoppingCart
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
