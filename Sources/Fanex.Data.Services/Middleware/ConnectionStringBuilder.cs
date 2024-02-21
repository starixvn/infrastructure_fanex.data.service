namespace Fanex.Data.Services.Middleware;

internal class ConnectionStringBuilder(string connectionString)
{
    private const int SplitIndex = 2;
    private const string DefaultServerKeyword = "data source";
    private const string DefaultUserIdKeyword = "user id";

    private static readonly string[] ServerAliases = { DefaultServerKeyword, "server", "host", "datasource", "address", "addr", "network address" };
    private static readonly string[] UserIdAliases = { DefaultUserIdKeyword, "uid", "username", "user name", "user", "userid" };

    private readonly IDictionary<string, string> connectionStringKeyValuePairs = BuildConnectionStringKeyValues(connectionString);

    private static IDictionary<string, string> BuildConnectionStringKeyValues(string connectionString)
        => connectionString.Split(';')
            .Where(kvp => kvp.Contains('='))
            .Select(kvp => kvp.Split(new[] { '=' }, SplitIndex))
            .ToDictionary(kvp => kvp[0].Trim(),
                kvp => kvp[1].Trim(),
                StringComparer.InvariantCultureIgnoreCase);

    public string GetServer()
        => GetValue(ServerAliases);

    public string GetUserId()
        => GetValue(UserIdAliases);

    private string GetValue(IEnumerable<string> keywordAliases)
    {
        foreach (var keyword in keywordAliases)
        {
            if (connectionStringKeyValuePairs.TryGetValue(keyword, out var value))
            {
                return value;
            }
        }

        return "N/A";
    }
}