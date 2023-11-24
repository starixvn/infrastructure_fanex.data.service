using System.Diagnostics.CodeAnalysis;

namespace Fanex.Data
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global"),
     SuppressMessage("ReSharper", "VirtualMemberNeverOverridden.Global")]
    public class ConnectionStringProviderBase
    {
        private readonly ConcurrentDictionary<string, string> connectionCache = new ConcurrentDictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        protected Func<string, string> PasswordDecryptor { get; set; }
            = Cryptography.DecryptConnectionString;

        protected Dictionary<string, ConnectionConfiguration> ConnectionMaps { get; set; }

        internal void Initialize(Dictionary<string, ConnectionConfiguration> connectionMaps)
            => ConnectionMaps = connectionMaps;

        public virtual string GetConnectionString(string connectionAlias, bool isEncrypted = true)
        {
            string PickConnectionStringFromConnectionMaps(string alias)
            {
                var connectionString = GetConnectionConfiguration(alias).ConnectionString;

                if (isEncrypted)
                {
                    connectionString = PasswordDecryptor(connectionString);
                }

                return connectionString;
            }

            if (string.IsNullOrWhiteSpace(connectionAlias))
            {
                throw new ArgumentException(
                    $"{nameof(connectionAlias)} must not be null or empty or only contain whitespaces.",
                    nameof(connectionAlias));
            }

            return connectionCache.GetOrAdd(connectionAlias, PickConnectionStringFromConnectionMaps);
        }

        public virtual string GetProviderName(string connectionAlias)
        {
            var providerName = GetConnectionConfiguration(connectionAlias).ProviderName;
            return string.IsNullOrEmpty(providerName)
                ? string.Empty
                : providerName;
        }

        protected ConnectionConfiguration GetConnectionConfiguration(string connectionAlias)
        {
            if (ConnectionMaps == null || ConnectionMaps.Count == 0)
            {
                throw new InvalidOperationException(
                    "The ConnectionStringProvider has not been initialized correctly or it did not return any connection string.");
            }

            if (!ConnectionMaps.TryGetValue(connectionAlias, out var connectionConfiguration))
            {
                throw new KeyNotFoundException($"There is no connection alias named '{connectionAlias}' in the list of configured connection strings.");
            }

            return connectionConfiguration;
        }
    }
}