using ONEbook.UM.Configurations;
using ONEbook.UM.Helpers;
using ONEbook.UM.Services;

namespace Fanex.Data.Services.Extensions;

using Logging;
using Logging.Sentry.RavenClient;
using Microsoft.Extensions.Caching.Memory;
using ONEbook.UM.AspNetCore;
using ONEbook.UM.Transport;
using SharpRaven;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddOneBookUm(this IServiceCollection services, IConfiguration configuration)
    {
        var umConfig = new UMClientConfiguration().Initialize(configuration);
        services.AddSingleton(umConfig);
        services.AddSingleton<IUMCacheService, MemoryUmCacheService>();
        services.AddSingleton<IUMRestClient, UMRestClient>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddSingleton<IHttpContextHelper, HttpContextHelper>();
        services.AddSingleton<IMaintenanceService, MaintenanceService>();

        return services;
    }

    public static IServiceCollection AddFanexLog(this IServiceCollection services, IConfiguration configuration)
    {
        LogManager
            .SetDefaultLogCategory(configuration.GetSection("LogCategory").Value)
            .Use(new List<ILogEngine>
            {
                new SentryLogging(new SentryEngineOptions
                {
                    Dsn = new Dsn(configuration.GetSection("SentryEngineOptions:Dsn").Value)
                })
            });
        return services;
    }
}


public class MemoryUmCacheService(IMemoryCache memoryCache) : IUMCacheService
{
    public T Get<T>(string key) => memoryCache.Get<T>(key);

    public void Set<T>(string key, T value, TimeSpan absoluteExpiration)
        => memoryCache.Set(key, value, new MemoryCacheEntryOptions().SetAbsoluteExpiration(absoluteExpiration));

    public void Remove(string key) => memoryCache.Remove(key);
}