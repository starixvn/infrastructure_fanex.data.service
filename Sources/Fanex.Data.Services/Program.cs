using Fanex.Caching.Redis;
using Fanex.Data.Services.Cache;
using Fanex.Data.Services.Extensions;
using Fanex.Data.Services.Middleware;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

// Add services to the container.
services.AddRedisCache(configuration.GetSection("Fanex.Caching.Redis"));
services.AddOneBookUm(configuration);
services.AddFanexLog(configuration);

services.AddControllers().AddNewtonsoftJson(options =>
{
    var dateConverter = new IsoDateTimeConverter
    {
        DateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fff"
    };
    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
    options.SerializerSettings.Converters.Add(dateConverter);
});
services.Configure<HostOptions>(options =>
{
    options.ShutdownTimeout = TimeSpan.FromMinutes(5);
});
services.AddHostedService<ServerAliveReportService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseRouting();

app.UseMiddleware<ErrorHandlerMiddleware>();

app.MapControllers();

var serviceProvider = app.Services;
var life = serviceProvider.GetService<IHostApplicationLifetime>();

life.ApplicationStopped.Register(() =>
{
    Console.WriteLine($"Application is stopped at ${DateTimeOffset.Now}");
});
life.ApplicationStopping.Register(() =>
{
    Console.WriteLine($"Application is being stopped at {DateTimeOffset.Now}");
});
Monitoring.StartTime = DateTimeOffset.UtcNow;

await app.RunAsync();