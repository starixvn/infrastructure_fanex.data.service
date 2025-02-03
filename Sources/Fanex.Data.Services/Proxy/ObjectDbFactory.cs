namespace Fanex.Data.Services.Proxy;

public static class ObjectDbFactory
{
    public static ObjectDb CreateInstance(ProxyParameter proxyParameter)
    {
        ParamsProcessing.Process(proxyParameter);
        var dbSetting = proxyParameter.DbSetting;
        var ignoreRedundantParameters = proxyParameter.IgnoreRedundantParameters;

        return dbSetting.Provider == "MySql.Data.MySqlClient"
            ? new MySqlObjectDb(dbSetting, ignoreRedundantParameters)
            : new SqlObjectDb(dbSetting, ignoreRedundantParameters);
    }
}