namespace Fanex.Data
{
    using System.Net.Http;
    using System.Text;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Models;
    using Newtonsoft.Json;

    public static class CacheManager
    {
        public static async Task<string> ClearCache(int maxRetries = 100)
        {
            var serverStatus = new Dictionary<string, DateTimeOffset>();
            var clearedServers = new List<string>();
            var counter = 0;
            while (counter < maxRetries)
            {
                var removeCacheStatus = await RemoveStoredProcedureCache().ConfigureAwait(false);
                if (removeCacheStatus != null)
                {
                    serverStatus = removeCacheStatus.ServerStatus;
                    var clearedServer = removeCacheStatus.ServerName;
                    TryToAddToCollection(clearedServers, clearedServer);
                }

                if (AreTwoCollectionsEqual(clearedServers, serverStatus.Keys))
                {
                    break;
                }

                await Task.Delay(100);
                counter++;
            }

            return ToString(serverStatus, clearedServers, counter + 1);
        }

        public static async Task<string> ClearAllCache(int maxRetries = 100)
        {
            var serverStatus = new Dictionary<string, DateTimeOffset>();
            var clearedServers = new List<string>();
            var counter = 0;
            while (counter < maxRetries)
            {
                var removeCacheStatus = await RemoveAllStoredProcedureCache().ConfigureAwait(false);
                if (removeCacheStatus != null)
                {
                    serverStatus = removeCacheStatus.ServerStatus;
                    var clearedServer = removeCacheStatus.ServerName;
                    TryToAddToCollection(clearedServers, clearedServer);
                }

                if (AreTwoCollectionsEqual(clearedServers, serverStatus.Keys))
                {
                    break;
                }

                await Task.Delay(100);
                counter++;
            }

            return ToString(serverStatus, clearedServers, counter + 1);
        }

        private static bool AreTwoCollectionsEqual(IEnumerable<string> first, IEnumerable<string> second)
        {
            var sortedFirst = first.OrderBy(x => x).ToList();
            var sortedSecond = second.OrderBy(x => x).ToList();

            return sortedFirst.SequenceEqual(sortedSecond);
        }

        private static void TryToAddToCollection(ICollection<string> collection, string item)
        {
            if (!collection.Contains(item))
            {
                collection.Add(item);
            }
        }

        private static string ToString(Dictionary<string, DateTimeOffset> serverStatus, List<string> clearedServers, int counter)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("Server\tLast Active Time");
            foreach (var pair in serverStatus)
            {
                stringBuilder.AppendLine($"{pair.Key}\t{pair.Value}");
            }

            stringBuilder.AppendLine();
            stringBuilder.AppendLine($"After {counter} loop. Stored procedure cache was cleared in following servers: {string.Join(",", clearedServers)}");

            return stringBuilder.ToString();
        }

        private static async Task<RemoveCacheResult> RemoveStoredProcedureCache()
        {
            const string endpoint = "api/cache/remove";
            try
            {
                var storedProcedures = DbSettingProviderManager.GetAllStoredProcedures();
                var requestJson = JsonConvert.SerializeObject(storedProcedures);
                var requestJsonContent = new StringContent(requestJson, Encoding.UTF8, "application/json");
                var response = await HttpClientFactory
                    .Client
                    .PostAsync(endpoint, requestJsonContent).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return JsonConvert.DeserializeObject<RemoveCacheResult>(json);
            }
            catch
            {
                return null;
            }
        }

        private static async Task<RemoveCacheResult> RemoveAllStoredProcedureCache()
        {
            const string endpoint = "api/cache/clean";
            try
            {
                var requestJsonContent = new StringContent(string.Empty, Encoding.UTF8, "application/json");
                var response = await HttpClientFactory
                    .Client
                    .PostAsync(endpoint, requestJsonContent).ConfigureAwait(false);
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return JsonConvert.DeserializeObject<RemoveCacheResult>(json);
            }
            catch
            {
                return null;
            }
        }
    }
}
