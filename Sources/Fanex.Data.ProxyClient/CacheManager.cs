using System;

namespace Fanex.Data;

using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Extensions;
using Newtonsoft.Json;

public static class CacheManager
{
    public static async Task<string> ClearCacheAsync()
    {
        const string endpoint = "api/cache/remove";
        var storedProcedures = DbSettingProviderManager.GetAllStoredProcedures();
        var requestJson = JsonConvert.SerializeObject(storedProcedures);
        var requestJsonContent = new StringContent(requestJson, Encoding.UTF8, "application/json");
        var response = await HttpClientFactory
            .Client
            .PostAsync(endpoint, requestJsonContent).ConfigureAwait(false);
        var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        return json;
    }

    public static void ClearCache()
    {
        SafeClearCacheAsync();
    }

    private static async Task SafeClearCacheAsync()
    {
        for (var i = 0; i < 60; i++)
        {
            try
            {
                await ClearCacheAsync().ConfigureAwait(false);
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error clearing cache: {ex.Message}");
            }

            await Task.Delay(TimeSpan.FromSeconds(5));
        }
    }
}