using Mango.web.Service;
using Mango.web.Service.IService;
using Mango.web.Utility;
using Microsoft.AspNetCore.Authentication.Cookies;


// mango.web
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
    builder.Services.AddControllersWithViews();
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddHttpClient();

    builder.Services.AddHttpClient<ICouponService,CouponService>();
    builder.Services.AddHttpClient<IProductService,ProductService>();
    builder.Services.AddHttpClient<IAuthService,AuthService>();
    builder.Services.AddHttpClient<ICartService,CartService>();
    builder.Services.AddHttpClient<IOrderService,OrderService>();

    SD.CouponApiBase = builder.Configuration["ServiceUrls:CouPonApi"];
    SD.ProductApiBase = builder.Configuration["ServiceUrls:ProductApi"];
    SD.AuthApiBase = builder.Configuration["ServiceUrls:AuthApi"];
    SD.ShoppingCartApiBase = builder.Configuration["ServiceUrls:ShoppingCartApi"];
    SD.OrderApiBase = builder.Configuration["ServiceUrls:OrderApi"];

    builder.Services.AddScoped<ITokenProvider,TokenProvider>();
    builder.Services.AddScoped<IBaseService,BaseService>();
    builder.Services.AddScoped<ICouponService, CouponService>();    
    builder.Services.AddScoped<IProductService, ProductService>();    
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<ICartService, CartService>();
    builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(op => {
        op.ExpireTimeSpan = TimeSpan.FromHours(10);
        op.LoginPath = "/Auth/login";
        op.AccessDeniedPath = "/Auth/AccessDenied";
        });
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
