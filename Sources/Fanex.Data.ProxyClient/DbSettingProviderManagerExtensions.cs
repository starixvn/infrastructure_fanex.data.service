#if NETFRAMEWORK
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
#endif

#if NET5_0_OR_GREATER
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
#endif

namespace Fanex.Data
{
    using Vault;

    public static class DbSettingProviderManagerExtensions
    {

#if NETFRAMEWORK

        public static DbSettingProviderManager Register(
                this DbSettingProviderManager currentSession,
                bool ignoreRedundantParameters = false)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                var connectionStrings = ConfigurationManager.ConnectionStrings;
                var connections = ReadConnectionStrings(connectionStrings);

                var resourcePath = appSettings["StoredProcedurePath"];
                var serviceApiUrl = appSettings["FanexDataServiceApi"];
                var vaultOptions = ReadVaultOptions(appSettings);
                return currentSession
                        .Use(connections)
                        .Use(vaultOptions)
                        .Use(serviceApiUrl)
                        .UseDefaultDbSettingProvider(resourcePath, ignoreRedundantParameters);
            }
            catch (Exception exception)
            {
                const string message = "Invalid configuration in appsettings.json. "
                                       + "Please check ConnectionStrings and StoredProcedurePath";
                throw new ArgumentException(message, exception);
            }
        }

        private static Dictionary<string, ConnectionConfiguration> ReadConnectionStrings(ConnectionStringSettingsCollection connectionStrings)
        {
            var connectionStringSettings = connectionStrings.Cast<ConnectionStringSettings>();

            return connectionStringSettings
                .Where(connectionStringSetting => !string.IsNullOrEmpty(connectionStringSetting.ConnectionString))
                .ToDictionary(
                    connectionStringSetting => connectionStringSetting.Name,
                    connectionStringSetting => new ConnectionConfiguration(
                        connectionStringSetting.Name,
                        connectionStringSetting.ConnectionString,
                        connectionStringSetting.ProviderName),
                    StringComparer.OrdinalIgnoreCase);
        }

        private static VaultOptions ReadVaultOptions(NameValueCollection appSettings)
        {
            var enable = appSettings["VaultOptions:Enable"] == "1" || appSettings["VaultOptions:Enable"] == "True" || appSettings["VaultOptions:Enable"] == "true";
            var interval = 0;
            if (!int.TryParse(appSettings["VaultOptions:MinutesPolling"], out interval))
            {
                interval = 5;
            }
            return new VaultOptions
            {
                Address = appSettings["VaultOptions:Address"],
                Enable = enable,
                MountPath = appSettings["VaultOptions:MountPath"],
                MinutesPolling = interval,
                Role = appSettings["VaultOptions:Role"],
                SecretId = appSettings["VaultOptions:SecretId"]
            };

        }
#endif

#if NET5_0_OR_GREATER

        public static IServiceCollection AddFanexData(this IServiceCollection services, IConfiguration configuration, bool ignoreRedundantParameters = false)
        {
            DbSettingProviderManager
                .StartNewSession()
                .Register(configuration, ignoreRedundantParameters)
                .Run();
            
            return services;
        }

        public static DbSettingProviderManager Register(
            this DbSettingProviderManager currentSession,
            IConfiguration configuration,
            bool ignoreRedundantParameters = false)
        {
            try
            {
                var connections = ReadConnectionStrings(configuration);

                var resourcePath = configuration.GetSection("StoredProcedurePath").Value;
                var serviceApiUrl = configuration.GetSection("FanexDataServiceApi").Value;
                var vaultOptions = configuration.GetSection("VaultOptions").Get<VaultOptions>();
                return currentSession
                        .Use(connections)
                        .Use(serviceApiUrl)
                        .Use(vaultOptions)
                        .UseDefaultDbSettingProvider(resourcePath, ignoreRedundantParameters);
            }
            catch (Exception exception)
            {
                const string message = "Invalid configuration in appsettings.json. "
                                       + "Please check ConnectionStrings and StoredProcedurePath";
                throw new ArgumentException(message, exception);
            }
        }

        private static Dictionary<string, ConnectionConfiguration> ReadConnectionStrings(IConfiguration configuration)
        {
            var connections = configuration.GetSection("ConnectionStrings")
                .GetChildren()
                .AsEnumerable()
                .ToDictionary(
                    x => x.GetValue<string>("Name"),
                    y => new ConnectionConfiguration(y.GetValue<string>("Name"), y.GetValue<string>("ConnectionString"), y.GetValue<string>("ProviderName"))
                );
            
            return connections;
        }
#endif
    }
}