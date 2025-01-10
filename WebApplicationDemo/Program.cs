using Fanex.Data;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

// Add services to the container.
services.AddFanexData(configuration);
services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapControllers();

await app.RunAsync();
