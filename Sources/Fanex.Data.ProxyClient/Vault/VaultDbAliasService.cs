namespace Fanex.Data.Vault;

using System.Collections.Concurrent;
using System.Collections.Generic;

public interface VaultDbAliasService
{
    string GetIp(string dbAlias);

    void UpdateDbAlias(IDictionary<string, string> dbAlias);
}

public class VaultDbAliasServiceImp: VaultDbAliasService
{
    private static readonly ConcurrentDictionary<string, string> DbAlias = new();

    public string GetIp(string dbAlias)
    {
        return DbAlias.TryGetValue(dbAlias, out var ip) ? ip : string.Empty;
    }

    public void UpdateDbAlias(IDictionary<string, string> dbAlias)
    {
        foreach (var item in dbAlias)
        {
            DbAlias[item.Key] = item.Value;
        }
    }
}