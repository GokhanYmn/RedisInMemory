using RedisExchangeAPI.Web.Services;
using Microsoft.Extensions.Caching.StackExchangeRedis;

var builder = WebApplication.CreateBuilder(args);

// ?? Redis Service'i Singleton olarak ekle (Baðlantýyý constructor içinde yapýyoruz)
builder.Services.AddSingleton<RedisService>();

// IDistributedCache için StackExchangeRedisCache yapýlandýrmasý
var redisHost = builder.Configuration["Redis:Host"] ?? "localhost";
var redisPort = builder.Configuration["Redis:Port"] ?? "6379";
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = $"{redisHost}:{redisPort}";
    options.InstanceName = "master";
});

builder.Services.AddControllersWithViews();

var app = builder.Build();
var redisService = app.Services.GetRequiredService<RedisService>();
redisService.Connect();

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();