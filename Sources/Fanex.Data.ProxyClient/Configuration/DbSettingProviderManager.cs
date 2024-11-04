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
        private DbSettingProvider settingProvider;

        private ConnectionStringProvider connectionStringProvider;

        private Uri serviceApiUri;

        private static VaultPollingService vaultPollingService;

        protected bool IgnoreRedundantParameters { get; set; }

        public static DbSettingProviderManager CurrentSession { get; private set; }

        public Dictionary<string, ConnectionConfiguration> ConnectionMaps { get; private set; }

        public VaultOptions VaultOptions { get; private set; }

        public static DbSettingProviderManager StartNewSession()
        {
            CurrentSession = new DbSettingProviderManager();
            vaultPollingService?.Stop();
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
            settingProvider = new DbSettingProvider(resourcePath);

            return this;
        }

        public DbSettingProviderManager Use(string uri)
        {
            CurrentSession.serviceApiUri = new Uri(uri.TrimEnd('/'));
            return this;
        }

        public DbSettingProviderManager Use(Dictionary<string, ConnectionConfiguration> connectionMaps, bool isPlainPassword = false)
        {
            ConnectionMaps = connectionMaps ?? throw new ArgumentNullException(nameof(connectionMaps));
            connectionStringProvider = new ConnectionStringProvider(connectionMaps);
            connectionStringProvider.Initialize(connectionMaps, isPlainPassword);

            return this;
        }

        public DbSettingProviderManager Use(VaultOptions vaultOptions)
        {
            VaultOptions = vaultOptions;
            return this;
        }

        public void ReloadConnectionStrings()
        {
            settingProvider.UpdateConnectionString(ConnectionMaps);
        }


        public void Run()
        {
            if (settingProvider == null)
            {
                throw new InvalidOperationException($"No {nameof(DbSettingProvider)} was configured.");
            }

            if (connectionStringProvider == null)
            {
                throw new InvalidOperationException($"No {nameof(ConnectionStringProvider)} was configured. Please call Use(Dictionary<string, ConnectionConfiguration> connectionConfiguration) first.");
            }

            if (VaultOptions?.Enable == true)
            {
                var vaultProvider = new VaultProviderImp(VaultOptions);
                var vaultService = new VaultServiceImp(vaultProvider);
                vaultService.UpdateConnectionMaps().Wait();

                vaultPollingService?.Stop();
                vaultPollingService = new VaultPollingService(vaultProvider);
                vaultPollingService.Start(VaultOptions.MinutesPolling * 60);
            }

            settingProvider.ConnectionStringProvider = connectionStringProvider;
            settingProvider.Start();

            CacheManager.ClearCache();
        }
    }
}