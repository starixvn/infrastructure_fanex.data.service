using Fanex.Data.Extensions;
using System.Diagnostics.CodeAnalysis;

namespace Fanex.Data
{
    using Models;
    using Repository;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Vault;

    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class DbSettingProviderManager
    {
        private DbSettingProviderBase settingProvider;

        private ConnectionStringProviderBase connectionStringProvider;

        private Uri serviceApiUri;

        private VaultProvider vaultProvider;

        private VaultService vaultService;

        private VaultPollingService vaultPollingService;

        protected bool IgnoreRedundantParameters { get; set; }

        public static DbSettingProviderManager CurrentSession { get; private set; }

        public Dictionary<string, ConnectionConfiguration> ConnectionMaps { get; private set; }

        public VaultOptions VaultOptions { get; private set; }

        public static DbSettingProviderManager StartNewSession()
        {
            CurrentSession = new DbSettingProviderManager();
            return CurrentSession;
        }

        public static void ClearSession()
        {
            CurrentSession = null;
        }

        internal static ProxyParameter GetProxyParameter(ICriteria criteria, string serverIp, List<string> typeNames)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria));
            }

            var key = criteria.GetSettingKey();
            var dbSetting = CurrentSession.settingProvider.GetSetting(key);
            dbSetting.ReplaceServerIp(serverIp);

            return new ProxyParameter
            {
                IgnoreRedundantParameters = CurrentSession.IgnoreRedundantParameters,
                ParameterSet = criteria,
                DbSetting = dbSetting,
                TypeNames = typeNames,
                SpecialTypes = criteria.GetSpecialTypes()
            };
        }

        internal static List<StoredProcedureDefinition> GetAllStoredProcedures()
        {
            var settingDataSources = CurrentSession
                .settingProvider
                .SettingDataSource
                .Values;

            return settingDataSources
                .Select(v => new StoredProcedureDefinition
                {
                    ConnectionString = v.ConnectionString,
                    StoreProcedureName = v.SpName
                }).ToList();
        }

        internal static int GetMaxSpTimeout()
        {
            var settingDataSources = CurrentSession
                .settingProvider
                .SettingDataSource
                .Values;

            return settingDataSources.Max(s => s.CommandTimeout);
        }

        internal static Uri ServiceApiUri => CurrentSession.serviceApiUri;

        // ReSharper disable once UnusedMethodReturnValue.Global
        public DbSettingProviderManager UseDefaultDbSettingProvider(string resourcePath, bool ignoreRedundantParameters = false)
        {
            if (string.IsNullOrWhiteSpace(resourcePath))
            {
                throw new ArgumentException("Resource path is null or empty.", nameof(resourcePath));
            }
            IgnoreRedundantParameters = ignoreRedundantParameters;
            settingProvider = new XmlDbSettingProvider(resourcePath);

            return this;
        }

        public DbSettingProviderManager Use(string uri)
        {
            CurrentSession.serviceApiUri = new Uri(uri);
            return this;
        }

        public DbSettingProviderManager Use(Dictionary<string, ConnectionConfiguration> connectionMaps)
        {
            ConnectionMaps = connectionMaps ?? throw new ArgumentNullException(nameof(connectionMaps));
            connectionStringProvider = new DefaultConnectionStringProvider(connectionMaps);
            connectionStringProvider.Initialize(connectionMaps);

            return this;
        }

        public DbSettingProviderManager Use(VaultOptions vaultOptions, VaultProvider provider = null)
        {
            VaultOptions = vaultOptions;

            vaultProvider = provider ?? new VaultProviderImp(vaultOptions);

            vaultPollingService?.Stop();

            vaultService = new VaultServiceImp(vaultProvider);
            vaultPollingService = new VaultPollingServiceImp(vaultProvider);

            return this;
        }

        public void RestartSettingProvider()
        {
            settingProvider.ConnectionStringProvider = connectionStringProvider;
            settingProvider.SettingDataSource.Clear();
            settingProvider.Restart();
        }


        public void Run()
        {
            if (settingProvider == null)
            {
                throw new InvalidOperationException($"No {nameof(DbSettingProviderBase)} was configured.");
            }

            if (connectionStringProvider == null)
            {
                throw new InvalidOperationException($"No {nameof(ConnectionStringProviderBase)} was configured. Please call Use(Dictionary<string, ConnectionConfiguration> connectionConfiguration) first.");
            }

            if (VaultOptions?.Enable == true)
            {
                vaultService.UpdateConnectionMaps().Wait();
                vaultPollingService.Start(VaultOptions.MinutesPolling * 60);
            }

            settingProvider.ConnectionStringProvider = connectionStringProvider;
            settingProvider.Start();

#if (DEBUG)
            CacheManager.ClearCache().Wait();
#else
            CacheManager.ClearCache();
#endif
        }
    }
}