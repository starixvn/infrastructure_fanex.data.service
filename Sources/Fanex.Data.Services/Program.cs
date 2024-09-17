using Fanex.Caching.Redis;
using Fanex.Data.Services;
using Fanex.Data.Services.Cache;
using Fanex.Data.Services.Middleware;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

// Add services to the container.
services.AddRedisCache(configuration.GetSection("Fanex.Caching.Redis"));

services.AddControllers().AddNewtonsoftJson(options =>
{
    var dateConverter = new IsoDateTimeConverter
    {
        DateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fff"
    };
    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
    options.SerializerSettings.Converters.Add(dateConverter);
});

services.AddHostedService<ServerAliveReportService>();
services.AddHostedService<GarbageCollectorService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseRouting();
app.UseMiddleware<ErrorHandlerMiddleware>();
app.MapControllers();

await app.RunAsync();