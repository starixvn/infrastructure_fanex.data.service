namespace Fanex.Data.Repository
{
    using Extensions;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    public partial class DynamicRepository
    {
        private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = ShouldSerializeContractResolver.Instance,
            DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fff",
            Converters = new List<JsonConverter> { new BytePropertyConverter() }
        };

        private static readonly JsonSerializer JsonSerializer = JsonSerializer.Create(JsonSerializerSettings);

        public string DebugInformation { get; set; }

        #region sync

        public IEnumerable<T> Fetch<T>(ICriteria criteria, string serverIp = "") => FetchAsync<T>(criteria, serverIp).GetAwaiter().GetResult();

        public DataSet FetchDataSet(ICriteria criteria, string serverIp = "") => FetchDataSetAsync(criteria, serverIp).GetAwaiter().GetResult();

        public T Get<T>(ICriteria criteria, string serverIp = "") => GetAsync<T>(criteria, serverIp).GetAwaiter().GetResult();

        public int Execute(INonQueryCommand command, string serverIp = "") => ExecuteAsync(command, serverIp).GetAwaiter().GetResult();

        #endregion

        #region async

        public async Task<IEnumerable<T>> FetchAsync<T>(ICriteria criteria, string serverIp = "")
        {
            const string endpoint = "api/dynamic-repository/fetch";

            if (!criteria.IsValid())
            {
                return Enumerable.Empty<T>();
            }

            var typeNames = new List<string> { typeof(T).Name };
            var result = await ExecuteApiService<JArray>(endpoint, criteria, typeNames, serverIp).ConfigureAwait(false);
            return result.ToList<T>(typeof(T), JsonSerializer);
        }

        public async Task<DataSet> FetchDataSetAsync(ICriteria criteria, string serverIp = "")
        {
            const string endpoint = "api/dynamic-repository/fetch-dataset";

            if (!criteria.IsValid())
            {
                return new DataSet();
            }

            return await ExecuteApiService<DataSet>(endpoint, criteria, new List<string>(), serverIp).ConfigureAwait(false);
        }

        public async Task<T> GetAsync<T>(ICriteria criteria, string serverIp = "")
        {
            const string endpoint = "api/dynamic-repository/get";

            if (!criteria.IsValid())
            {
                return default;
            }

            var typeNames = new List<string> { typeof(T).Name };
            var result = await ExecuteApiService<JObject>(endpoint, criteria, typeNames, serverIp).ConfigureAwait(false);
            return result.ToSingleData<T>(typeof(T), JsonSerializer);
        }

        public async Task<int> ExecuteAsync(INonQueryCommand command, string serverIp = "")
        {
            const string endpoint = "api/dynamic-repository/execute";
            if (!command.IsValid())
            {
                return default;
            }

            return await ExecuteApiService<int>(endpoint, command, new List<string>(), serverIp).ConfigureAwait(false);
        }

        #endregion

        #region utility

        private async Task<T> ExecuteApiService<T>(string endpoint, ICriteria criteria, List<string> typeNames, string serverIp)
        {
            var proxyParameter = DbSettingProviderManager.GetProxyParameter(criteria, serverIp, typeNames);
            return await ExecuteApiService<T>(endpoint, criteria, proxyParameter).ConfigureAwait(false);
        }

        private async Task<T> ExecuteApiService<T>(string endpoint, ICriteria criteria, List<Type> types, string serverIp)
        {
            var typeNames = types.Select(t => t.Name).ToList();
            var proxyParameter = DbSettingProviderManager.GetProxyParameter(criteria, serverIp, typeNames);

            return await ExecuteApiService<T>(endpoint, criteria, proxyParameter).ConfigureAwait(false);
        }

        private async Task<T> ExecuteApiService<T>(string endpoint, ICriteria criteria, ProxyParameter proxyParameter)
        {
            var requestJson = JsonConvert.SerializeObject(proxyParameter, JsonSerializerSettings);
            var response = await PostAsync(endpoint, requestJson).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false) + $". Status code: {response.StatusCode}";
                throw DbProxyNotSuccessHttpException.Create(endpoint, requestJson, json);
            }

            var responseJson = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            try
            {
                var result = ExtractResultFromJson<T>(criteria, responseJson);
                DebugInformation = $"Endpoint: {DbSettingProviderManager.ServiceApiUri}{endpoint}" + Environment.NewLine
                    + $"RequestJson: {requestJson}" + Environment.NewLine
                    + $"ResponseJson: {responseJson}";
                return result;
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
            criteria.CopyValueFrom(criteriaResult);
            return proxyResult.Result;
        }

        private static async Task<HttpResponseMessage> PostAsync(string endpoint, string requestJson)
        {
            var requestJsonContent = new StringContent(requestJson, Encoding.UTF8, "application/json");
            try
            {
                return await HttpClientFactory
                    .Client
                    .PostAsync(endpoint, requestJsonContent).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                throw DbProxyHttpRequestException.Create(endpoint, requestJson, e);
            }
        }

        #endregion
    }
}