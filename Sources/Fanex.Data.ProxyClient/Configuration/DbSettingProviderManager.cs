using Fanex.Data.Extensions;
using System.Diagnostics.CodeAnalysis;

namespace Fanex.Data
{
    using Models;
    using Repository;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class DbSettingProviderManager
    {
        private static DbSettingProviderManager currentSession;

        private DbSettingProviderBase settingProvider;

        private XmlConnectionStringProvider connectionStringProvider;

        private Uri serviceApiUri;

        protected bool IgnoreRedundantParameters { get; set; }

        public static DbSettingProviderManager StartNewSession()
        {
            currentSession = new DbSettingProviderManager();

            return currentSession;
        }

        public static void ClearSession()
        {
            currentSession = null;
        }

        internal static ProxyParameter GetProxyParameter(ICriteria criteria, string serverName, List<string> typeNames)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria));
            }

            var key = criteria.GetSettingKey();
            var dbSetting = currentSession.settingProvider.GetSetting(key);
            dbSetting.ReplaceServerName(serverName);

            return new ProxyParameter
            {
                IgnoreRedundantParameters = currentSession.IgnoreRedundantParameters,
                ParameterSet = criteria,
                DbSetting = dbSetting,
                TypeNames = typeNames,
                SpecialTypes = criteria.GetSpecialTypes()
            };
        }

        internal static List<StoredProcedureDefinition> GetAllStoredProcedures()
        {
            var settingDataSources = currentSession
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
            var settingDataSources = currentSession
                .settingProvider
                .SettingDataSource
                .Values;

            return settingDataSources.Max(s => s.CommandTimeout);
        }

        internal static Uri ServiceApiUri => currentSession.serviceApiUri;

        public DbSettingProviderManager Register(
            string resourcePath,
            bool ignoreRedundantParameters = false)
        {
            if (settingProvider == null)
            {
                UseDefaultDbSettingProvider(resourcePath);
            }

            IgnoreRedundantParameters = ignoreRedundantParameters;

            return this;
        }

        // ReSharper disable once UnusedMethodReturnValue.Global
        public DbSettingProviderManager UseDefaultDbSettingProvider(string resourcePath)
        {
            if (string.IsNullOrWhiteSpace(resourcePath))
            {
                throw new ArgumentException("Resource path is null or empty.", nameof(resourcePath));
            }

            settingProvider = new XmlDbSettingProvider(resourcePath);

            return this;
        }

        public DbSettingProviderManager Use(string uri)
        {
            currentSession.serviceApiUri = new Uri(uri);
            return this;
        }

        public DbSettingProviderManager Use(Dictionary<string, ConnectionConfiguration> connectionConfiguration)
        {
            if (connectionConfiguration == null)
            {
                throw new ArgumentNullException(nameof(connectionConfiguration));
            }

            connectionStringProvider = new XmlConnectionStringProvider(connectionConfiguration);
            connectionStringProvider.Initialize(connectionConfiguration);

            return this;
        }

        public void Run()
        {
            if (settingProvider == null)
            {
                throw new InvalidOperationException($"No {nameof(DbSettingProviderBase)} was configured.");
            }

            if (connectionStringProvider == null)
            {
#if NETFRAMEWORK
                connectionStringProvider = new XmlConnectionStringProvider();
#else
                throw new InvalidOperationException($"No {nameof(ConnectionStringProviderBase)} was configured. Please call Use(Dictionary<string, ConnectionConfiguration> connectionConfiguration) first.");
#endif
            }

            settingProvider.ConnectionStringProvider = connectionStringProvider;
            settingProvider.Start();

            CacheManager.ClearCache();
        }
    }
}