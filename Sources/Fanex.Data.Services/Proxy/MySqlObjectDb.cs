namespace Fanex.Data.Services.Proxy;

using MySql;

public class MySqlObjectDb : ObjectDb
{
    public MySqlObjectDb(IDbSetting dbSetting, bool ignoreRedundantParameters)
    {
        IgnoreRedundantParameters = ignoreRedundantParameters;
        SettingKey = dbSetting.Key;
        DbSetting = dbSetting;
        CommandTimeout = dbSetting.CommandTimeout ?? DefaultCommandTimeout;
        Runner = new MySqlStoredProcedureRunner(dbSetting.DbName, dbSetting.ConnectionString);
    }
}