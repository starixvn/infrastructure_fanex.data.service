#if NET7_0_OR_GREATER

using Xunit;
using System.Threading.Tasks;
using NSubstitute;
using Fanex.Data.Vault;
using System.Collections.Generic;

namespace Fanex.Data.ProxyClient.Tests
{
    using Repository;

    /*
    CREATE PROCEDURE FanexDataIpById
    AS
    SELECT 1 AS ID, '10.18.200.58' AS [IP], SUSER_NAME() AS [Username]

    GO
    GRANT EXECUTE ON FanexDataIpById TO wsv_nap
    GRANT EXECUTE ON FanexDataIpById TO wsv_lap
    GO

     */
    public class IpByIdCriteria : CriteriaBase
    {
        public override string GetSettingKey()
            => "FanexDataIpById";

        public override bool IsValid() => true;
    }

    public class IpObject
    {
        public int Id { get; set; }
        
        public string Ip { get; set; }

        public string Username { get; set; }
    }

    [Collection("Fanex Data Client collection")]
    public class VaultServiceIntegrationTests
    {
        public readonly List<DatabaseCredential> Credential58 = new()
        {
            new DatabaseCredential("DBACC.bodb02.wsv_nap")
            {
                IP = "10.18.200.58",
                Password = "wsN3v3rG1v3Up3ncrypt!0n!v",
                Username = "wsv_nap"
            }
        };

        public readonly List<DatabaseCredential> Credential56 = new()
        {
            new DatabaseCredential("DBACC.bodb02.wsv_nap")
            {
                IP = "10.18.200.56",
                Password = "wsN3v3rG1v3Up3ncrypt!0n!v",
                Username = "wsv_nap"
            }
        };

        public readonly List<DatabaseCredential> Credential_wsv_lap = new()
        {
            new DatabaseCredential("DBACC.bodb02.wsv_nap")
            {
                IP = "10.18.200.56",
                Password = "wsF0rt!fyP@ssw0rd&Securityv",
                Username = "wsv_lap"
            }
        };

        [Fact]
        public async Task CallSp_AfterUpdateConnectionMaps_HappyCase()
        {
            var vaultProvider = Substitute.For<VaultProvider>();
            vaultProvider.LoadAsync(Arg.Any<List<string>>()).ReturnsForAnyArgs(Credential58);

            var vaultService = new VaultServiceImp(vaultProvider);
            await vaultService.UpdateConnectionMaps();

            DbSettingProviderManager.CurrentSession.RestartSettingProvider();

            var criteria = new IpByIdCriteria();

            var dynamicRepository = new DynamicRepository();
            var obj = await dynamicRepository.GetAsync<IpObject>(criteria);
            Assert.Equal("10.18.200.58", obj.Ip);
        }

        [Fact]
        public async Task CallSp_AfterUpdateConnectionMaps_ChangeAliasIp()
        {
            var vaultProvider = Substitute.For<VaultProvider>();
            var pollingService = new VaultPollingService(vaultProvider);
            try
            {
                vaultProvider.LoadAsync(Arg.Any<List<string>>()).ReturnsForAnyArgs(Credential58);
                pollingService.Start(1);

                await Task.Delay(5000);
                var criteria = new IpByIdCriteria();

                var dynamicRepository = new DynamicRepository();
                var obj58 = await dynamicRepository.GetAsync<IpObject>(criteria);
                Assert.Equal("10.18.200.58", obj58.Ip);

                vaultProvider.LoadAsync(Arg.Any<List<string>>()).ReturnsForAnyArgs(Credential56);

                await Task.Delay(5000);
                var obj56 = await dynamicRepository.GetAsync<IpObject>(criteria);
                Assert.Equal("10.18.200.56", obj56.Ip);
            }
            finally
            {
                pollingService.Stop();
            }
        }

        [Fact]
        public async Task CallSp_AfterUpdateConnectionMaps_ChangeCredential()
        {
            var vaultProvider = Substitute.For<VaultProvider>();
            var pollingService = new VaultPollingService(vaultProvider);
            try
            {
                vaultProvider.LoadAsync(Arg.Any<List<string>>()).ReturnsForAnyArgs(Credential58);
                pollingService.Start(1);

                await Task.Delay(5000);
                var criteria = new IpByIdCriteria();

                var dynamicRepository = new DynamicRepository();
                var obj58 = await dynamicRepository.GetAsync<IpObject>(criteria);
                Assert.Equal("wsv_nap", obj58.Username);

                vaultProvider.LoadAsync(Arg.Any<List<string>>()).ReturnsForAnyArgs(Credential_wsv_lap);

                await Task.Delay(5000);
                var obj56 = await dynamicRepository.GetAsync<IpObject>(criteria);
                Assert.Equal("wsv_lap", obj56.Username);
            }
            finally
            {
                pollingService.Stop();
            }
        }
    }
}
#endif