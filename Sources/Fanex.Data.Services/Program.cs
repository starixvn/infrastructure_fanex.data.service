using Fanex.Caching.Redis;
using Fanex.Data.Services.Cache;
using Fanex.Data.Services.Middleware;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

// Add services to the container.
services.AddRedisCache(configuration.GetSection("Fanex.Caching.Redis"));

services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
});
services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
});

services.AddHostedService<ServerAliveReportService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseResponseCompression();

app.UseRouting();

app.UseMiddleware<ErrorHandler>();

app.MapControllers();



app.Run();