namespace Fanex.Data.Repository.Tests
{
    using SqlServer;
    using System;

#if NET5_0_OR_GREATER
    using Microsoft.Extensions.Configuration;
#endif

    public sealed class FanexDataConfigurationManager : IDisposable
    {
        public FanexDataConfigurationManager()
        {
#if NETFRAMEWORK
            DbSettingProviderManager
                .StartNewSession()
                .WithSqlServer("~/App_Data")
                .Run();
#elif NET5_0_OR_GREATER
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false);
            var configuration = builder.Build();

            DbSettingProviderManager
              .StartNewSession()
              .WithSqlServer(configuration)
              .Run();
#endif
        }

        public void Dispose()
            => DbSettingProviderManager.ClearSession();
    }
}