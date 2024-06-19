using AutoMapper;
using Mango.Service.AuthAPI.Service.IService;
using Mango.Service.AuthAPI.Service;
using Mango.Services.AuthApi.Data;
using Microsoft.EntityFrameworkCore;
using Mango.Service.AuthAPI.Model;
using Microsoft.AspNetCore.Identity;

namespace Mango.Services.AuthApi
{
    public class ServiceRegistrationHelper
    {
        public static void RegisterServices(IServiceCollection services, ConfigurationManager configuration)
        {

			services.Configure<JwtOptions>(configuration.GetSection("ApiSettings:JwtOptions"));
			services.AddIdentity<ApplicationUsers, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();
			IMapper mapper = MappingConfig.RegisterMapper().CreateMapper();
            services.AddSingleton(mapper);
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
	;
			services.AddAuthorization();
            services.AddControllers();
            services.AddScoped<IJWTokenGenerator, JWTokenGenerator>();
            services.AddScoped<IAuthService, AuthService>();


		}
    }
}
