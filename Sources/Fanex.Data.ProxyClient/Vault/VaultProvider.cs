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

        void ResetVaultToken();

        Task EnsureVaultTokenIsValid();

        string GetIp(string dbAlias);
    }

    public class VaultProviderImp : VaultProvider
    {
        private const int MinTimeToLive = 15;

        private static IDictionary<string, string> DbAlias = new Dictionary<string, string>();

        private readonly VaultOptions vaultOptions;

        private readonly VaultClient vaultClient;

        public VaultProviderImp(VaultOptions vaultOptions)
        {
            this.vaultOptions = vaultOptions;
            vaultClient = CreateVaultClient(vaultOptions);
        }

        public async Task<List<DatabaseCredential>> LoadAsync(List<string> connectionAliases)
        {
            await LoadDbAlias();

            var result = await QueryCredentials(connectionAliases);

            return result.OrderBy(v => v.Key).ToList();
        }

        private async Task LoadDbAlias()
        {
            const string dbAliasPath = "database/alias";
            var aliasSecrets = await vaultClient.V1.Secrets.KeyValue.V2
                .ReadSecretAsync(dbAliasPath, null, vaultOptions.MountPath)
                .ConfigureAwait(false);
            var dbAlias = aliasSecrets.Data.Data.ToDictionary(v => v.Key,
                v => v.Value == null ? string.Empty : v.Value.ToString().Trim());

            DbAlias = dbAlias;
        }

        private async Task<List<DatabaseCredential>> QueryCredentials(List<string> connectionAliases)
        {
            var result = new List<DatabaseCredential>();

            foreach (var alias in connectionAliases)
            {
                var parts = alias.Split('.');
                if (parts.Length != 3)
                {
                    continue;
                }

                var dbAlias = parts[0];
                var dbLogin = parts[2];

                var (username, password) = await QueryCredential(dbLogin);

                var ip = GetIp(dbAlias);

                var item = new DatabaseCredential(alias)
                {
                    IP = ip,
                    Password = password,
                    Username = username
                };

                result.Add(item);
            }

            return result;
        }

        private async Task<(string username, string password)> QueryCredential(string dbLogin)
        {
            var credentialPath = $"database/credentials/{dbLogin}";
            var credentialSecrets = await vaultClient.V1.Secrets.KeyValue.V2
                .ReadSecretAsync(credentialPath, null, vaultOptions.MountPath)
                .ConfigureAwait(false);
            var username = credentialSecrets.Data.Data["username"].ToString();
            var password = credentialSecrets.Data.Data["password"].ToString();
            return (username, password);
        }

        public async Task EnsureVaultTokenIsValid()
        {
            var tokenData = await vaultClient.V1.Auth.Token.LookupSelfAsync();
            var timeToLive = tokenData.Data.TimeToLive;
            if (timeToLive < MinTimeToLive)
            {
                vaultClient.V1.Auth.ResetVaultToken();
            }
        }

        private static VaultClient CreateVaultClient(VaultOptions vaultOptions)
        {
            var vaultClientSettings = new VaultClientSettings(
                vaultOptions.Address,
                new AppRoleAuthMethodInfo(vaultOptions.Role, vaultOptions.SecretId)
            );

            return new VaultClient(vaultClientSettings);
        }

        public void ResetVaultToken()
        {
            vaultClient.V1.Auth.ResetVaultToken();
        }

        public string GetIp(string dbAlias)
        {
            if (DbAlias.TryGetValue(dbAlias, out var ip))
            {
                return ip;
            }

            return string.Empty;
        }
    }
}
