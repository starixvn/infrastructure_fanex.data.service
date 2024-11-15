using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Fanex.Data.ProxyClient.Tests")]
namespace Fanex.Data.Vault;

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

internal interface VaultService
{
    Task<bool> UpdateConnectionMaps();

    Task<List<DatabaseCredential>> TryLoadAsync(List<string> connectionAliases, int tryCount);

    bool IsCredentialChanged(List<DatabaseCredential> credentials);

    void UpdateConnectionMapsByVaultCredentials(Dictionary<string, ConnectionConfiguration> connectionMaps, IEnumerable<DatabaseCredential> credentials);
}

internal class VaultServiceImp(VaultProvider vaultProvider) : VaultService
{
    private const int TryCount = 3;

    private List<DatabaseCredential> currentCredentials;

    public async Task<bool> UpdateConnectionMaps()
    {
        var connectionMaps = DbSettingProviderManager.CurrentSession.ConnectionMaps;
        var connectionAliases = connectionMaps.Keys.Select(k => k).ToList();
        var credentials = await TryLoadAsync(connectionAliases, TryCount).ConfigureAwait(false);

        var isCredentialChanged = IsCredentialChanged(credentials);
        if (isCredentialChanged)
        {
            UpdateConnectionMapsByVaultCredentials(connectionMaps, credentials);
            DbSettingProviderManager.CurrentSession.Use(connectionMaps, true);
        }

        currentCredentials = credentials;

        return isCredentialChanged;
    }

    public async Task<List<DatabaseCredential>> TryLoadAsync(List<string> connectionAliases, int tryCount)
    {
        Exception latestException = null;
        for (var i = 0; i < tryCount; i++)
        {
            try
            {
                await vaultProvider.EnsureVaultTokenIsValid().ConfigureAwait(false);
                return await vaultProvider.LoadAsync(connectionAliases).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                latestException = e;
                vaultProvider.ResetVaultToken();
            }

            await Task.Delay(3000).ConfigureAwait(false);
        }

        if (latestException != null)
        {
            throw latestException;
        }

        return null;
    }

    public bool IsCredentialChanged(List<DatabaseCredential> credentials)
    {
        if (credentials == null)
        {
            return false;
        }

        return currentCredentials == null || !currentCredentials.SequenceEqual(credentials);
    }

    public void UpdateConnectionMapsByVaultCredentials(Dictionary<string, ConnectionConfiguration> connectionMaps, IEnumerable<DatabaseCredential> credentials)
    {
        foreach (var item in credentials)
        {
            if (!connectionMaps.TryGetValue(item.Key, out var connection))
            {
                continue;
            }

            var builder = new DbConnectionStringBuilder
            {
                ConnectionString = connection.ConnectionString
            };

            if (!string.IsNullOrWhiteSpace(item.IP))
            {
                builder.SetServer(item.IP);
            }

            if (!string.IsNullOrWhiteSpace(item.Username))
            {
                builder.SetUserId(item.Username);
            }

            if (!string.IsNullOrWhiteSpace(item.Password))
            {
                builder.SetPassword(item.Password);
            }

            var newConnection = new ConnectionConfiguration(
                connection.ConnectionAlias,
                builder.ToString(),
                connection.ProviderName);

            connectionMaps[item.Key] = newConnection;
        }
    }
}