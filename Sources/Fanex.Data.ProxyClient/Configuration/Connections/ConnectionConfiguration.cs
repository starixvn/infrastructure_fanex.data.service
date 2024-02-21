namespace Fanex.Data
{
    using System;

    public class ConnectionConfiguration
    {
        private ConnectionConfiguration(string connectionAlias, string connectionString)
        {
            if (string.IsNullOrEmpty(connectionAlias))
            {
                throw new ArgumentException(
                    $"{nameof(connectionAlias)} is required to map connection string and provider.",
                    nameof(connectionAlias));
            }

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException(
                    $"{nameof(connectionString)} must not be null or empty or only contain whitespaces.",
                    nameof(connectionString));
            }

            ConnectionAlias = connectionAlias;
            ConnectionString = connectionString;
        }

        public ConnectionConfiguration(string connectionAlias, string connectionString, string providerName)
            : this(connectionAlias, connectionString)
        {
            ProviderName = providerName;
        }

        public string ConnectionAlias { get; }

        public string ConnectionString { get; }

        public string ProviderName { get; }
    }
}