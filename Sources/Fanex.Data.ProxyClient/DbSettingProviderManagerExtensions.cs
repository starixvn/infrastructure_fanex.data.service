namespace Fanex.Data
{
    using System.Diagnostics.CodeAnalysis;

#if NET5_0_OR_GREATER
    using System;
    using System.Linq;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
#endif

    [SuppressMessage("ReSharper", "UnusedType.Global")]
    public static class DbSettingProviderManagerExtensions
    {
#if NET5_0_OR_GREATER
        public static DbSettingProviderManager Register(
            this DbSettingProviderManager currentSession,
            IConfiguration configuration,
            bool ignoreRedundantParameters = false)
        {
            try
            {
                var connections = configuration.GetSection("ConnectionStrings")
                    .GetChildren()
                    .AsEnumerable()
                    .ToDictionary(
                        x => x.GetValue<string>("Name"),
                        y => new ConnectionConfiguration(y.GetValue<string>("Name"), y.GetValue<string>("ConnectionString"), y.GetValue<string>("ProviderName"))
                    );

                var resourcePath = configuration.GetSection("StoredProcedurePath").Value;
                var serviceApiUrl = configuration.GetSection("FanexDataServiceApi").Value;
                return currentSession
                        .Use(connections)
                        .Use(serviceApiUrl)
                        .Register(resourcePath, ignoreRedundantParameters);
            }
            catch (Exception exception)
            {
                const string message = "Invalid configuration in appsettings.json. "
                                       + "Please check ConnectionStrings and StoredProcedurePath";
                throw new ArgumentException(message, exception);
            }
        }

        public static IServiceCollection AddFanexData(this IServiceCollection services, IConfiguration configuration)
        {
            DbSettingProviderManager
                .StartNewSession()
                .Register(configuration)
                .Run();
            
            return services;
        }
#endif
    }
}