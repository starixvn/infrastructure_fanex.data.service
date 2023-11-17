namespace Fanex.Data.Services.Proxy;

public static class ObjectDbFactory
{
    public static ObjectDb CreateInstance(ProxyParameter proxyParameter)
    {
        var dbSetting = proxyParameter.DbSetting;
        var ignoreRedundantParameters = proxyParameter.IgnoreRedundantParameters;

        return dbSetting.Provider switch
        {
            "MySql.Data.MySqlClient" => new MySqlObjectDb(dbSetting, ignoreRedundantParameters),
            _ => new SqlObjectDb(dbSetting, ignoreRedundantParameters)
        };
    }
}