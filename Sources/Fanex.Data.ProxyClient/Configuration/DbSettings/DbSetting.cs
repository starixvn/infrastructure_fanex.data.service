namespace Fanex.Data;

using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
public class DbSetting(
    string key,
    string provider,
    string dbName,
    string connectionString,
    string spName,
    int? commandTimeout,
    IDictionary<string, string> columnMaps,
    IDictionary<string, object> firmParameters)
{
    public const int DefaultCommandTimeout = 120;

    public string Key { get; } = key;

    public string Provider { get; set; } = provider;

    public string DbName { get; } = dbName;

    public string ConnectionString { get; private set; } = connectionString;

    public string SpName { get; } = spName;

    public int CommandTimeout { get; } = commandTimeout ?? DefaultCommandTimeout;

    public IDictionary<string, string> ColumnMaps { get; set; } = columnMaps;

    public IDictionary<string, object> FirmParameters { get; } = firmParameters;

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

    public void ReplaceServerIp(string serverIp)
    {
        if (string.IsNullOrWhiteSpace(serverIp))
        {
            return;
        }

        var builder = new DbConnectionStringBuilder
        {
            ConnectionString = ConnectionString
        };
        builder.SetServer(serverIp);

        ConnectionString = builder.ToString();
    }
}