namespace Fanex.Data.Vault
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using VaultSharp;
    using VaultSharp.V1.AuthMethods.AppRole;

    public interface VaultProvider
    {
        Task<List<DatabaseCredential>> LoadAsync(List<string> connectionAliases);
    }

    public class VaultProviderImp : VaultProvider
    {
        private readonly VaultOptions vaultOptions;

        public VaultProviderImp(VaultOptions vaultOptions)
        {
            this.vaultOptions = vaultOptions;
        }

        public async Task<List<DatabaseCredential>> LoadAsync(List<string> connectionAliases)
        {
            var client = CreateVaultClient();
            var result = new List<DatabaseCredential>();
            const string dbAliasPath = "database/alias";
            var aliasSecrets = await client.V1.Secrets.KeyValue.V2
                .ReadSecretAsync(dbAliasPath, null, vaultOptions.MountPath)
                .ConfigureAwait(false);

            foreach (var alias in connectionAliases)
            {
                var parts = alias.Split('.');
                if (parts.Length != 3)
                {
                    continue;
                }

                var dbAlias = parts[0];

                var dbLogin = parts[2];
                var credentialPath = $"database/credentials/{dbLogin}";
                var credentialSecrets = await client.V1.Secrets.KeyValue.V2
                    .ReadSecretAsync(credentialPath, null, vaultOptions.MountPath)
                    .ConfigureAwait(false);
                var username = credentialSecrets.Data.Data["username"].ToString();
                var password = credentialSecrets.Data.Data["password"].ToString();

                var ip = aliasSecrets.Data.Data.ContainsKey(dbAlias) ? aliasSecrets.Data.Data[dbAlias].ToString() : string.Empty;

                var item = new DatabaseCredential(alias)
                {
                    IP = ip,
                    Password = password,
                    Username = username
                };

                result.Add(item);
            }

            return result.OrderBy(v => v.Key).ToList();
        }

        private VaultClient CreateVaultClient()
        {
            var vaultClientSettings = new VaultClientSettings(
                vaultOptions.Address,
                new AppRoleAuthMethodInfo(vaultOptions.Role, vaultOptions.SecretId)
            );

            return new VaultClient(vaultClientSettings);
        }
    }
}
