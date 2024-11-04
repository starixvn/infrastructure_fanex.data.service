namespace Fanex.Data
{
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
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

        public static string ClearCache()
        {
	        return ClearCacheAsync().GetAwaiter().GetResult();
        }
	}
}
