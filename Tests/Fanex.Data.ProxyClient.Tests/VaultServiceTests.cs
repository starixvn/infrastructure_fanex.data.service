using Xunit;
using System.Threading.Tasks;
using NSubstitute;
using Fanex.Data.Vault;
using System.Collections.Generic;

namespace Fanex.Data.ProxyClient.Tests
{
    using System;

    [Collection("Fanex Data Client collection")]
    public class VaultServiceTests
    {
        public readonly List<DatabaseCredential> Credentials = new()
        {
            new DatabaseCredential("DBACC.bodb02.wsv_nap")
            {
                IP = "10.18.200.61\\Devdb053",
                Password = "h9Sky5eJOAeB0EOPr1IhVw==",
                Username = "FanexUser"
            }
        };

        [Fact]
        public async Task TryLoadAsyncTests_HappyCase()
        {
            var vaultProvider = Substitute.For<VaultProvider>();
            vaultProvider.LoadAsync(Arg.Any<List<string>>()).ReturnsForAnyArgs(new List<DatabaseCredential>());

            var vaultService = new VaultServiceImp(vaultProvider);
            var actual = await vaultService.TryLoadAsync(new List<string>(), 3);

            Assert.Empty(actual);
        }

        [Fact]
        public async Task TryLoadAsyncTests_SuccessAtSecondTime()
        {
            var vaultProvider = Substitute.For<VaultProvider>();
            var callCount = 0;
            vaultProvider.LoadAsync(Arg.Any<List<string>>())
                .Returns(async _ =>
                    callCount++ == 0
                        ? throw new Exception("Exception at function LoadAsync")
                        : await Task.FromResult(new List<DatabaseCredential>()));
            var vaultService = new VaultServiceImp(vaultProvider);
            var actual = await vaultService.TryLoadAsync(new List<string>(), 3);

            Assert.Empty(actual);
        }

        [Fact]
        public async Task TryLoadAsyncTests_AlwaysFail()
        {
            var vaultProvider = Substitute.For<VaultProvider>();
            vaultProvider.LoadAsync(Arg.Any<List<string>>())
                .Returns(async _ =>
                {
                    await Task.CompletedTask;
                    throw new Exception("Exception at function LoadAsync");
                });
            var vaultService = new VaultServiceImp(vaultProvider);

            await Assert.ThrowsAsync<InvalidCastException>(async () => await vaultService.TryLoadAsync(new List<string>(), 2));
        }

        [Fact]
        public async Task UpdateConnectionMaps_HappyCase()
        {
            var vaultProvider = Substitute.For<VaultProvider>();
            vaultProvider.LoadAsync(Arg.Any<List<string>>()).ReturnsForAnyArgs(Credentials);

            var vaultService = new VaultServiceImp(vaultProvider);
            await vaultService.UpdateConnectionMaps();

            var actualConnectionString = DbSettingProviderManager.CurrentSession.ConnectionMaps["DBACC.bodb02.wsv_nap"].ConnectionString;
            const string expected = "packet size=4096;data source=10.18.200.61\\Devdb053;initial catalog=bodb02;connect timeout=600;integrated security=false;user id=FanexUser;password=\"h9Sky5eJOAeB0EOPr1IhVw==\";trustservercertificate=True";
            Assert.Equal(expected, actualConnectionString);
        }

        [Fact]
        public void UpdateConnectionMapsByVaultCredentials_UpdatesCorrectly()
        {
            // Arrange
            var vaultProvider = Substitute.For<VaultProvider>();
            var vaultService = new VaultServiceImp(vaultProvider);
            var connectionMaps = new Dictionary<string, ConnectionConfiguration>
            {
                { "testKey", new ConnectionConfiguration("alias", "server=test;user id=test;password=test", "provider") }
            };
            var credentials = new List<DatabaseCredential>
            {
                new DatabaseCredential("testKey") {  IP = "newIP", Username = "newUser", Password = "newPass" }
            };

            // Act
            vaultService.UpdateConnectionMapsByVaultCredentials(connectionMaps, credentials);

            // Assert
            var updatedConnection = connectionMaps["testKey"];
            Assert.Contains("server=newIP", updatedConnection.ConnectionString);
            Assert.Contains("user id=newUser", updatedConnection.ConnectionString);
            Assert.Contains("password=newPass", updatedConnection.ConnectionString);
        }

        [Fact]
        public async Task TryLoadAsync_LoadsCredentialsSuccessfully()
        {
            // Arrange
            var vaultProvider = Substitute.For<VaultProvider>();
            var connectionAliases = new List<string> { "alias1", "alias2" };
            var expectedCredentials = new List<DatabaseCredential>
            {
                new DatabaseCredential("alias1") { IP = "ip1", Username = "user1", Password = "pass1" },
                new DatabaseCredential("alias2") { IP = "ip2", Username = "user2", Password = "pass2" }
            };
            vaultProvider.LoadAsync(connectionAliases).Returns(expectedCredentials);

            var vaultService = new VaultServiceImp(vaultProvider);

            // Act
            var actualCredentials = await vaultService.TryLoadAsync(connectionAliases, 1);

            // Assert
            Assert.Equal(expectedCredentials, actualCredentials);
        }

        [Fact]
        public async Task TryLoadAsync_RetriesOnFailure()
        {
            // Arrange
            var vaultProvider = Substitute.For<VaultProvider>();
            var connectionAliases = new List<string> { "alias1", "alias2" };
            vaultProvider.When(x => x.LoadAsync(connectionAliases)).Do(x => { throw new Exception(); });

            var vaultService = new VaultServiceImp(vaultProvider);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => vaultService.TryLoadAsync(connectionAliases, 2));
            await vaultProvider.Received(2).LoadAsync(connectionAliases);
        }
    }
}