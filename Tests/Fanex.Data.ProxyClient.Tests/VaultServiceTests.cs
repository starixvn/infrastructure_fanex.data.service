#if NET5_0_OR_GREATER

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
                        : new List<DatabaseCredential>());
            var vaultService = new VaultServiceImp(vaultProvider);
            var actual = await vaultService.TryLoadAsync(new List<string>(), 3);

            Assert.Empty(actual);
        }

        [Fact]
        public async Task TryLoadAsyncTests_AlwaysFail()
        {
            var vaultProvider = Substitute.For<VaultProvider>();
            vaultProvider.LoadAsync(Arg.Any<List<string>>())
                .Returns(async _ => throw new Exception("Exception at function LoadAsync"));
            var vaultService = new VaultServiceImp(vaultProvider);

            await Assert.ThrowsAsync<Exception>(async () => await vaultService.TryLoadAsync(new List<string>(), 2));
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
    }
}
#endif