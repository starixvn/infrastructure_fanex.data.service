#if NET5_0_OR_GREATER
using Microsoft.Extensions.Configuration;
#endif

namespace Fanex.Data.ProxyClient.Tests
{
    using System;
    using Xunit;

    public sealed class TestsFixture : IDisposable
    {
        public TestsFixture()
        {
#if NETFRAMEWORK
            DbSettingProviderManager
                .StartNewSession()
                .Use(System.Configuration.ConfigurationManager.AppSettings["FanexDataServiceApi"])
                .Register("~/App_Data")
                .Run();
#else
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false);
            var configuration = builder.Build();
            DbSettingProviderManager
                .StartNewSession()
                .Register(configuration)
                .Run();
#endif
        }

        public void Dispose()
        {
            DbSettingProviderManager.ClearSession();
        }
    }

    [CollectionDefinition("Fanex Data Client collection")]
    public class DatabaseCollection : ICollectionFixture<TestsFixture>
    {
    }
}