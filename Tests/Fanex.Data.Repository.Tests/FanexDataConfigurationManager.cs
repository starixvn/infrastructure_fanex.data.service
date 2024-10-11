namespace Fanex.Data.Repository.Tests
{
    using SqlServer;
    using System;

    using Microsoft.Extensions.Configuration;

    public sealed class FanexDataConfigurationManager : IDisposable
    {
        public FanexDataConfigurationManager()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false);
            var configuration = builder.Build();

            DbSettingProviderManager
              .StartNewSession()
              .WithSqlServer(configuration)
              .UseDefaultDbSettingProvider("App_Data")
              .Run();
        }

        public void Dispose()
            => DbSettingProviderManager.ClearSession();
    }
}