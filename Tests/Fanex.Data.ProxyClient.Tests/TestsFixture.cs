using Microsoft.Extensions.Configuration;

namespace Fanex.Data.ProxyClient.Tests;

using System;
using Xunit;

public sealed class TestsFixture : IDisposable
{
    public TestsFixture()
    {
        var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false);
        var configuration = builder.Build();
        DbSettingProviderManager
            .StartNewSession()
            .Register(configuration)
            .Run();
    }

    public void Dispose()
    {
        DbSettingProviderManager.ClearSession();
    }
}

[CollectionDefinition("Fanex Data Client collection")]
public class DatabaseCollection : ICollectionFixture<TestsFixture>;