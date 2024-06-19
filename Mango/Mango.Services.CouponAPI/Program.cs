using AutoMapper;
using Mango.Services.CouponAPI;
using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Security.Cryptography.Xml;
using System.Text;

//mango.couponapi

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(op =>
{
    op.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
ServiceRegistrationHelper.RegisterServices(builder.Services);
//IMapper mapper = MappingConfig.RegisterMapper().CreateMapper();
//builder.Services.AddSingleton(mapper);
//builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(op =>
{
    op.AddSecurityDefinition(name: "Bearer", securityScheme: new OpenApiSecurityScheme { 
    
        Name="Authorization",
        Description="Bearer Token",
        In = ParameterLocation.Header,
        Type=SecuritySchemeType.ApiKey,
        Scheme="Bearer"
    
    });
    op.AddSecurityRequirement(new OpenApiSecurityRequirement() 
    {
        {
             new OpenApiSecurityScheme
             {
                Reference  =  new OpenApiReference
                 {
                     Type=ReferenceType.SecurityScheme,
                     Id = JwtBearerDefaults.AuthenticationScheme
                 }
             },new string[]{ }
         }

    });
});


builder.AddAppAuthentication();

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
ApplyMigration();
app.Run();

void ApplyMigration()
{

    using (var scope = app.Services.CreateScope())
    {

        var _db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        if (_db.Database.GetPendingMigrations().Count() > 0)
        {
            _db.Database.Migrate();
        }

    }


}




//public static class AutoMapperServiceExtension
//{
//    public static void ConfigureAutoMappers(IServiceCollection services)
//    {
//        services.AddAutoMapper(Assembly.GetEntryAssembly());
//    }
//}
