using Microsoft.Extensions.Caching.Distributed;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "CorsPolicy",
        policy =>
        {
            policy.AllowAnyMethod()
                .AllowCredentials()
                .SetIsOriginAllowed((host) => true)
                .AllowAnyHeader(); 
        });
});

builder.Services.AddSingleton(typeof(IList<byte[]>), new List<byte[]>());

builder.Services.AddStackExchangeRedisCache(option =>
{
    option.Configuration = builder.Configuration.GetConnectionString("redis");
});

builder.Services.AddSingleton<ICache>(builder =>
{
    return QuickCache.GetInstance();
});


builder.Services.AddSingleton<IRedisTestService>((o) => {
    var cache = o.GetRequiredService<IDistributedCache>();
    return RedisTestService.Instance(cache);
});

builder.Services.AddSingleton<IQuickCacheTestService>((o) => {
    var cache = o.GetRequiredService<ICache>();
    return QuickCacheTestService.Instance(cache);
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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Enable the /metrics page to export Prometheus metrics.
// Open http://localhost:5099/metrics to see the metrics.
//
// Metrics published in this sample:
// * built-in process metrics giving basic information about the .NET runtime (enabled by default)
// * metrics from .NET Event Counters (enabled by default, updated every 10 seconds)
// * metrics from .NET Meters (enabled by default)
// * metrics about requests made by registered HTTP clients used in SampleService (configured above)
// * metrics about requests handled by the web app (configured above)
// * ASP.NET health check statuses (configured above)
// * custom business logic metrics published by the SampleService class
app.MapMetrics();

app.Run();
