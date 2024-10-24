using System.Data.Common;
using Xunit;

namespace Fanex.Data.ProxyClient.Tests.Extensions;

public class DbConnectionStringBuilderExtensionsTests
{
    private readonly DbConnectionStringBuilder _connectionStringBuilder = new();

    [Theory]
    [InlineData("data source=10.19.10.47")]
    [InlineData("server=10.19.10.47")]
    [InlineData("host=10.19.10.47")]
    [InlineData("datasource=10.19.10.47")]
    [InlineData("address=10.19.10.47")]
    [InlineData("addr=10.19.10.47")]
    [InlineData("network address=10.19.10.47")]
    public void GetServer_KeywordIsInDefinedServerAliases_ReturnConfiguredServer(string connectionString)
    {
        // Arrange.
        _connectionStringBuilder.ConnectionString = connectionString;

        // Act.
        var actualServerName = _connectionStringBuilder.GetServer();

        // Assert.
        const string expectedServerName = "10.19.10.47";

        Assert.Equal(expectedServerName, actualServerName);
    }

    [Theory]
    [InlineData("data source=10.19.10.47", "data source=localhost")]
    [InlineData("server=10.19.10.47", "server=localhost")]
    [InlineData("host=10.19.10.47", "host=localhost")]
    [InlineData("datasource=10.19.10.47", "datasource=localhost")]
    [InlineData("address=10.19.10.47", "address=localhost")]
    [InlineData("addr=10.19.10.47", "addr=localhost")]
    [InlineData("network address=10.19.10.47", "network address=localhost")]
    [InlineData("packet size=4096;Data Source=10.40.40.61;Initial Catalog=bodb_Notification;integrated security=false; user id=bodbAdminNet; password=password;", "packet size=4096;data source=localhost;initial catalog=bodb_Notification;integrated security=false;user id=bodbAdminNet;password=password")]
    [InlineData(@"Data Source=10.18.200.61\\Devdb053;Initial Catalog=FanexDataTests;Persist Security Info=True;User ID=FanexUser;Password=password;TrustServerCertificate=True;","data source=localhost;initial catalog=FanexDataTests;persist security info=True;user id=FanexUser;password=password;trustservercertificate=True")]
    public void SetServer_ConnectionStringAlreadyContainedServerName_Overwrite(string connectionString, string expectedNewConnectionString)
    {
        // Arrange.
        const string newServerName = "localhost";

        _connectionStringBuilder.ConnectionString = connectionString;

        // Act.
        _connectionStringBuilder.SetServer(newServerName);

        // Assert.
        Assert.Equal(expectedNewConnectionString, _connectionStringBuilder.ToString());
    }

    [Fact]
    public void SetServer_ConnectionStringDoesNotContainServerName_AddServerByDefaultKeyword()
    {
        // Arrange.
        var oldConnectionString = string.Empty;
        const string newServerName = "FakeNewServerName";

        _connectionStringBuilder.ConnectionString = oldConnectionString;

        // Act.
        _connectionStringBuilder.SetServer(newServerName);

        // Assert.
        const string expectedConnectionString = "data source=FakeNewServerName";

        Assert.Equal(expectedConnectionString, _connectionStringBuilder.ToString());
    }

    [Theory]
    [InlineData("initial catalog=test")]
    [InlineData("database=test")]
    public void GetDatabase_KeywordIsInDefinedDatabaseAliases_ReturnConfiguredDatabase(string connectionString)
    {
        // Arrange.
        _connectionStringBuilder.ConnectionString = connectionString;

        // Act.
        var actualDatabaseName = _connectionStringBuilder.GetDatabase();

        // Assert.
        const string expectedDatabaseName = "test";

        Assert.Equal(expectedDatabaseName, actualDatabaseName);
    }

    [Theory]
    [InlineData("initial catalog=test", "initial catalog=xtest")]
    [InlineData("database=test", "database=xtest")]
    public void SetDatabase_ConnectionStringAlreadyContainedDatabaseName_Overwrite(string connectionString, string expectedNewConnectionString)
    {
        // Arrange.
        const string newDatabaseName = "xtest";

        _connectionStringBuilder.ConnectionString = connectionString;

        // Act.
        _connectionStringBuilder.SetDatabase(newDatabaseName);

        // Assert.
        Assert.Equal(expectedNewConnectionString, _connectionStringBuilder.ToString());
    }

    [Fact]
    public void SetDatabase_ConnectionStringDoesNotContainSetDatabaseName_AddDatabaseByDefaultKeyword()
    {
        // Arrange.
        var oldConnectionString = string.Empty;
        const string newDatabaseName = "FakeNewDatabaseName";

        _connectionStringBuilder.ConnectionString = oldConnectionString;

        // Act.
        _connectionStringBuilder.SetDatabase(newDatabaseName);

        // Assert.
        const string expectedConnectionString = "initial catalog=FakeNewDatabaseName";

        Assert.Equal(expectedConnectionString, _connectionStringBuilder.ToString());
    }

    [Theory]
    [InlineData("user id=sa")]
    [InlineData("uid=sa")]
    [InlineData("username=sa")]
    [InlineData("user name=sa")]
    [InlineData("user=sa")]
    [InlineData("userid=sa")]
    public void GetUserId_KeywordIsInDefinedUserIdAliases_ReturnConfiguredUserId(string connectionString)
    {
        // Arrange.
        _connectionStringBuilder.ConnectionString = connectionString;

        // Act.
        var actualUserId = _connectionStringBuilder.GetUserId();

        // Assert.
        const string expectedUserId = "sa";

        Assert.Equal(expectedUserId, actualUserId);
    }

    [Theory]
    [InlineData("user id=js", "user id=nhl")]
    [InlineData("uid=js", "uid=nhl")]
    [InlineData("username=js", "username=nhl")]
    [InlineData("user name=js", "user name=nhl")]
    [InlineData("user=js", "user=nhl")]
    [InlineData("userid=js", "userid=nhl")]
    public void SetUserId_ConnectionStringAlreadyContainedUserId_Overwrite(string connectionString, string expectedNewConnectionString)
    {
        // Arrange.
        const string newUserId = "nhl";

        _connectionStringBuilder.ConnectionString = connectionString;

        // Act.
        _connectionStringBuilder.SetUserId(newUserId);

        // Assert.
        Assert.Equal(expectedNewConnectionString, _connectionStringBuilder.ToString());
    }

    [Fact]
    public void SetUserId_ConnectionStringDoesNotContainUserId_AddUserIdByDefaultKeyword()
    {
        // Arrange.
        var oldConnectionString = string.Empty;
        const string newUserId = "FakeUserId";

        _connectionStringBuilder.ConnectionString = oldConnectionString;

        // Act.
        _connectionStringBuilder.SetUserId(newUserId);

        // Assert.
        const string expectedConnectionString = "user id=FakeUserId";

        Assert.Equal(expectedConnectionString, _connectionStringBuilder.ToString());
    }

    [Theory]
    [InlineData("password=1234aa")]
    [InlineData("pwd=1234aa")]
    public void GetPassword_KeywordIsInDefinedPasswordAliases_ReturnConfiguredPassword(string connectionString)
    {
        // Arrange.
        _connectionStringBuilder.ConnectionString = connectionString;

        // Act.
        var actualPassword = _connectionStringBuilder.GetPassword();

        // Assert.
        const string expectedPassword = "1234aa";

        Assert.Equal(expectedPassword, actualPassword);
    }

    [Theory]
    [InlineData("password=existed-password", "password=password")]
    [InlineData("pwd=existed-password", "pwd=password")]
    public void SetPassword_ConnectionStringAlreadyContainedPassword_Overwrite(string connectionString, string expectedNewConnectionString)
    {
        // Arrange.
        const string newPassword = "password";

        _connectionStringBuilder.ConnectionString = connectionString;

        // Act.
        _connectionStringBuilder.SetPassword(newPassword);

        // Assert.
        Assert.Equal(expectedNewConnectionString, _connectionStringBuilder.ToString());
    }

    [Fact]
    public void SetPassword_ConnectionStringDoesNotContainPassword_AddPasswordByDefaultKeyword()
    {
        // Arrange.
        var oldConnectionString = string.Empty;
        const string newPassword = "FakeNewPassword";

        _connectionStringBuilder.ConnectionString = oldConnectionString;

        // Act.
        _connectionStringBuilder.SetPassword(newPassword);

        // Assert.
        const string expectedConnectionString = "password=FakeNewPassword";

        Assert.Equal(expectedConnectionString, _connectionStringBuilder.ToString());
    }
}