
using Xunit;
using System.Threading.Tasks;
using Fanex.Data.Vault;
using System.Collections.Generic;

namespace Fanex.Data.ProxyClient.Tests
{
    [Collection("Fanex Data Client collection")]
    public class VaultProviderIntegrationTests
    {
        [Fact]
        public async Task LoadAsyncTests_HappyCase()
        {
            var vaultOptions = new VaultOptions
            {
                Address = "https://vault.nexdev.net",
                Enable = true,
                MountPath = "pontus",
                MinutesPolling = 1,
                Role = "88e53c60-a6a0-8876-0de8-ff035343c0fc",
                SecretId = "19918eba-d3b4-35e4-70dc-bf2c7b41cd93"
            };
            var aliases = new List<string>
            {
                "DBACC.bodb02.wsv_nap"
            };
            var vaultProvider = new VaultProviderImp(vaultOptions);
            
            var credentials = await vaultProvider.LoadAsync(aliases);

            Assert.NotEmpty(credentials);
        }
    }
}