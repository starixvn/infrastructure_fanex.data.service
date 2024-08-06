#if NET5_0_OR_GREATER

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
                Address = "https://vault-dev.nexdev.net",
                Enable = true,
                MountPath = "pontus",
                MinutesPolling = 1,
                Role = "4b985f89-4ec5-abf9-ebaa-f49bdb743d43",
                SecretId = "caaa9926-4a84-5253-a01d-e8698440dd10"
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
#endif