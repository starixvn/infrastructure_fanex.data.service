namespace Fanex.Data.Repository
{
    using Extensions;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    public partial class DynamicRepository
    {
        private static readonly HttpClient Client;

        private static readonly JsonSerializerSettings JsonSerializerSettings;

        private static readonly JsonSerializer JsonSerializer;

        static DynamicRepository()
        {
#if NET5_0_OR_GREATER
            var handler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Brotli
            };
#else
            var handler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip
            };
#endif

            Client = new HttpClient(handler)
            {
                BaseAddress = DbSettingProviderManager.ServiceApiUri
            };
#if NET5_0_OR_GREATER
            Client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("br"));
#else
            Client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
#endif

            JsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = ShouldSerializeContractResolver.Instance
            };

            JsonSerializer = JsonSerializer.Create(JsonSerializerSettings);
        }

        #region sync

        public IEnumerable<T> Fetch<T>(ICriteria criteria, string serverName = "") => FetchAsync<T>(criteria, serverName).Result;

        public DataSet FetchDataSet(ICriteria criteria, string serverName = "") => FetchDataSetAsync(criteria, serverName).Result;

        public T Get<T>(ICriteria criteria, string serverName = "") => GetAsync<T>(criteria, serverName).Result;

        public int Execute(INonQueryCommand command, string serverName = "") => ExecuteAsync(command, serverName).Result;

        #endregion

        #region async

        public async Task<IEnumerable<T>> FetchAsync<T>(ICriteria criteria, string serverName = "")
        {
            var endpoint = IsSimpleType(typeof(T))
                ? "api/dynamic-repository/fetch-simple-type"
                : "api/dynamic-repository/fetch";

            if (!criteria.IsValid())
            {
                return Enumerable.Empty<T>();
            }

            var typeNames = new List<string> { typeof(T).Name };
            return await ExecuteApiService<IEnumerable<T>>(endpoint, criteria, typeNames, serverName).ConfigureAwait(false);
        }

        public async Task<DataSet> FetchDataSetAsync(ICriteria criteria, string serverName = "")
        {
            const string endpoint = "api/dynamic-repository/fetch-dataset";

            if (!criteria.IsValid())
            {
                return new DataSet();
            }

            return await ExecuteApiService<DataSet>(endpoint, criteria, new List<string>(), serverName).ConfigureAwait(false);
        }

        public async Task<T> GetAsync<T>(ICriteria criteria, string serverName = "")
        {
            var endpoint = IsSimpleType(typeof(T))
                ? "api/dynamic-repository/get-simple-type"
                : "api/dynamic-repository/get";

            if (!criteria.IsValid())
            {
                return default;
            }

            var typeNames = new List<string> { typeof(T).Name };
            return await ExecuteApiService<T>(endpoint, criteria, typeNames, serverName).ConfigureAwait(false);
        }

        public async Task<int> ExecuteAsync(INonQueryCommand command, string serverName = "")
        {
            const string endpoint = "api/dynamic-repository/execute";
            if (!command.IsValid())
            {
                return default;
            }

            return await ExecuteApiService<int>(endpoint, command, new List<string>(), serverName).ConfigureAwait(false);
        }

        #endregion

        #region utility

        private static async Task<T> ExecuteApiService<T>(string endpoint, ICriteria criteria, List<string> typeNames, string serverName)
        {
            var proxyParameter = DbSettingProviderManager.GetProxyParameter(criteria, serverName, typeNames);
            return await ExecuteApiService<T>(endpoint, criteria, proxyParameter).ConfigureAwait(false);
        }

        private static async Task<T> ExecuteApiService<T>(string endpoint, ICriteria criteria, ProxyParameter proxyParameter)
        {
            var requestJson = JsonConvert.SerializeObject(proxyParameter, JsonSerializerSettings);
            var response = await PostAsync(endpoint, requestJson).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                throw DbProxyNotSuccessHttpException.Create(endpoint, requestJson, json);
            }

            var responseJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            try
            {
                return ExtractResultFromJson<T>(criteria, responseJson);
            }
            catch (Exception e)
            {
                throw DbProxyJsonDeserializeException.Create(
                    endpoint,
                    requestJson,
                    responseJson,
                    e);
            }
        }

        private static T ExtractResultFromJson<T>(ICriteria criteria, string responseJson)
        {
            var proxyResult = JsonConvert.DeserializeObject<ProxyResult<T>>(responseJson, JsonSerializerSettings);
            var criteriaResult = JsonConvert.DeserializeObject(proxyResult.ParameterSet, criteria.GetType(), JsonSerializerSettings);
            criteria.CopyPropertiesFrom(criteriaResult);

            return proxyResult.Result;
        }

        private static async Task<HttpResponseMessage> PostAsync(string endpoint, string requestJson)
        {
            var requestJsonContent = new StringContent(requestJson, Encoding.UTF8, "application/json");
            try
            {
                return await Client.PostAsync(endpoint, requestJsonContent).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                throw DbProxyHttpRequestException.Create(endpoint, requestJson, e);
            }
        }

        private static bool IsSimpleType(Type type)
        {
            return type.IsPrimitive || type.IsEnum || type == typeof(decimal)
                   || type == typeof(string)
                   || type == typeof(DateTime)
                   || type == typeof(TimeSpan) || type == typeof(DateTimeOffset);
        }

        #endregion
    }
}