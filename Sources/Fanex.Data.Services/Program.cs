using Fanex.Data.Services.Middleware;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Add services to the container.
services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
});
services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseResponseCompression();

app.UseRouting();

app.UseMiddleware<ErrorHandler>();

app.MapControllers();

app.Run();