namespace Fanex.Data.Services.Proxy;

using SqlServer;

public class SqlObjectDb : ObjectDb
{
    public SqlObjectDb(IDbSetting dbSetting, bool ignoreRedundantParameters)
    {
        IgnoreRedundantParameters = ignoreRedundantParameters;
        SettingKey = dbSetting.Key;
        DbSetting = dbSetting;
        CommandTimeout = dbSetting.CommandTimeout ?? DefaultCommandTimeout;
        Runner = new SqlStoredProcedureRunner(dbSetting.DbName, dbSetting.ConnectionString);
    }
}