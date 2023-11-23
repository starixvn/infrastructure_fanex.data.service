namespace Fanex.Data
{
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class DbSetting
    {
        public DbSetting(
            string key,
            string provider,
            string dbName,
            string connectionString,
            string spName,
            int? commandTimeout,
            IDictionary<string, string> columnMaps,
            IDictionary<string, object> firmParameters)
        {
            Key = key;
            Provider = provider;
            DbName = dbName;
            ConnectionString = connectionString;
            SpName = spName;
            CommandTimeout = commandTimeout;
            ColumnMaps = columnMaps;
            FirmParameters = firmParameters;
        }

        public string Key { get; }

        public string Provider { get; set; }

        public string DbName { get; }

        public string ConnectionString { get; private set; }

        public string SpName { get; }

        public int? CommandTimeout { get; }

        public IDictionary<string, string> ColumnMaps { get; set; }

        public IDictionary<string, object> FirmParameters { get; }

        public bool IsValid()
            => IsValidSpNameConfiguration() && IsValidDbNameConfiguration() && IsValidColumnMaps();

        protected bool IsValidSpNameConfiguration()
            => !string.IsNullOrWhiteSpace(Key) && !string.IsNullOrWhiteSpace(SpName);

        protected bool IsValidDbNameConfiguration()
            => !string.IsNullOrWhiteSpace(DbName) && !string.IsNullOrWhiteSpace(ConnectionString);

        protected bool IsValidColumnMaps()
        {
            var numberOfMappingsWithType = ColumnMaps.Keys.Count(IsMappedWithType);

            return numberOfMappingsWithType == 0 || numberOfMappingsWithType == ColumnMaps.Count;
        }

        private static bool IsMappedWithType(string mappedKey)
        {
            var typeNameSeparatorIndex = mappedKey.IndexOf(';');

            if (typeNameSeparatorIndex >= 0)
            {
                var mappedTypeName = mappedKey.Substring(typeNameSeparatorIndex + 1);

                return !string.IsNullOrEmpty(mappedTypeName);
            }

            return false;
        }

        public DbSetting Clone()
        {
            return new DbSetting
                (Key,
                Provider,
                DbName,
                ConnectionString,
                SpName,
                CommandTimeout,
                new Dictionary<string, string>(ColumnMaps),
                FirmParameters);
        }

        public void ReplaceServerName(string serverName)
        {
            if (string.IsNullOrWhiteSpace(serverName))
            {
                return;
            }

            var builder = new DbConnectionStringBuilder
            {
                ConnectionString = ConnectionString
            };
            builder.SetServer(serverName);

            ConnectionString = builder.ToString();
        }
    }
}