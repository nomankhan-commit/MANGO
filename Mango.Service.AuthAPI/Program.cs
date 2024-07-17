using Mango.Service.AuthAPI.Model;
using Mango.Service.AuthAPI.Service;
using Mango.Service.AuthAPI.Service.IService;
using Mango.Services.AuthApi;
using Mango.Services.AuthApi.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

//mango.auth

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(op =>
{
	  op.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
//builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("ApiSettings:JwtOptions"));
//builder.Services.AddIdentity<IdentityUser,IdentityRole>().AddEntityFrameworkStores<AppDbContext>();


ServiceRegistrationHelper.RegisterServices(builder.Services, builder.Configuration);

// Add services to the container.

//builder.Services.AddControllers();
//builder.Services.AddScoped<IAuthService, AuthService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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