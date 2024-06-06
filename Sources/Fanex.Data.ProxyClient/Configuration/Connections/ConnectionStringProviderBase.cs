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
        {
            if (connectionMaps == null)
            {
                throw new ArgumentNullException(nameof(connectionMaps));
            }

            if (connectionMaps.Count == 0)
            {
                throw new ArgumentException($"The passed {nameof(connectionMaps)} contains no connection configuration.", nameof(connectionMaps));
            }

            ConnectionMaps = connectionMaps;
            connectionCache.Clear();
        }

        public virtual string GetConnectionString(string connectionAlias, bool isEncrypted = true)
        {
            if (string.IsNullOrWhiteSpace(connectionAlias))
            {
                throw new ArgumentException(
                    $"{nameof(connectionAlias)} must not be null or empty or only contain whitespaces.",
                    nameof(connectionAlias));
            }

            return connectionCache.GetOrAdd(connectionAlias, PickConnectionStringFromConnectionMaps);

            string PickConnectionStringFromConnectionMaps(string alias)
            {
                var connectionString = GetConnectionConfiguration(alias).ConnectionString;

                if (isEncrypted)
                {
                    connectionString = PasswordDecryptor(connectionString);
                }

                return connectionString;
            }
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