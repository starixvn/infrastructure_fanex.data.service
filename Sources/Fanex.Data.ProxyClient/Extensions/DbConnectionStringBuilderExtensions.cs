namespace Fanex.Data;

using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

public static class DbConnectionStringBuilderExtensions
{
    public const string DefaultServerKeyword = "data source";
    public const string DefaultDatabaseKeyword = "initial catalog";
    public const string DefaultUserIdKeyword = "user id";
    public const string DefaultPasswordKeyword = "password";

    internal static readonly string[] ServerAliases = [DefaultServerKeyword, "server", "host", "datasource", "address", "addr", "network address"
    ];
    internal static readonly string[] DatabaseAliases = [DefaultDatabaseKeyword, "database"];
    internal static readonly string[] UserIdAliases = [DefaultUserIdKeyword, "uid", "username", "user name", "user", "userid"
    ];
    internal static readonly string[] PasswordAliases = [DefaultPasswordKeyword, "pwd"];

    public static string GetServer(this DbConnectionStringBuilder connectionStringBuilder)
        => GetValue(connectionStringBuilder, ServerAliases);

    public static void SetServer(this DbConnectionStringBuilder connectionStringBuilder, string serverIp)
    {
        if (!ChangeValue(connectionStringBuilder, ServerAliases, serverIp))
        {
            connectionStringBuilder[DefaultServerKeyword] = serverIp;
        }
    }

    public static string GetDatabase(this DbConnectionStringBuilder connectionStringBuilder)
        => GetValue(connectionStringBuilder, DatabaseAliases);

    public static void SetDatabase(this DbConnectionStringBuilder connectionStringBuilder, string databaseName)
    {
        if (!ChangeValue(connectionStringBuilder, DatabaseAliases, databaseName))
        {
            connectionStringBuilder[DefaultDatabaseKeyword] = databaseName;
        }
    }

    public static string GetUserId(this DbConnectionStringBuilder connectionStringBuilder)
        => GetValue(connectionStringBuilder, UserIdAliases);

    public static void SetUserId(this DbConnectionStringBuilder connectionStringBuilder, string userId)
    {
        if (!ChangeValue(connectionStringBuilder, UserIdAliases, userId))
        {
            connectionStringBuilder[DefaultUserIdKeyword] = userId;
        }
    }

    public static string GetPassword(this DbConnectionStringBuilder connectionStringBuilder)
        => GetValue(connectionStringBuilder, PasswordAliases);

    public static void SetPassword(this DbConnectionStringBuilder connectionStringBuilder, string password)
    {
        if (!ChangeValue(connectionStringBuilder, PasswordAliases, password))
        {
            connectionStringBuilder[DefaultPasswordKeyword] = password;
        }
    }

    private static string GetValue(DbConnectionStringBuilder connectionStringBuilder, IEnumerable<string> keywordAliases)
    {
        return keywordAliases.Where(connectionStringBuilder.ContainsKey)
            .Select(keyword => connectionStringBuilder[keyword] as string)
            .FirstOrDefault();
    }

    private static bool ChangeValue(DbConnectionStringBuilder connectionStringBuilder, IEnumerable<string> keywordAliases, string value)
    {
        foreach (var keyword in keywordAliases)
        {
            if (connectionStringBuilder.ContainsKey(keyword))
            {
                connectionStringBuilder[keyword] = value;

                return true;
            }
        }

        return false;
    }
}