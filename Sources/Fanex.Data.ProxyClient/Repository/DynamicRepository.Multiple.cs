namespace Fanex.Data.Repository
{
    using System;
    using Newtonsoft.Json.Linq;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public partial class DynamicRepository : IDynamicRepository
    {
        public (IEnumerable<T1>, IEnumerable<T2>) FetchMultiple<T1, T2>(ICriteria criteria, string serverName = "") => FetchMultipleAsync<T1, T2>(criteria, serverName).Result;

        public (IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>) FetchMultiple<T1, T2, T3>(ICriteria criteria, string serverName = "") => FetchMultipleAsync<T1, T2, T3>(criteria, serverName).Result;

        public (IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>) FetchMultiple<T1, T2, T3, T4>(ICriteria criteria, string serverName = "")
            => FetchMultipleAsync<T1, T2, T3, T4>(criteria, serverName).Result;

        public (IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>) FetchMultiple<T1, T2, T3, T4, T5>(ICriteria criteria, string serverName = "") => FetchMultipleAsync<T1, T2, T3, T4, T5>(criteria, serverName).Result;

        public (IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>, IEnumerable<T6>) FetchMultiple<T1, T2, T3, T4, T5, T6>(ICriteria criteria, string serverName = "") => FetchMultipleAsync<T1, T2, T3, T4, T5, T6>(criteria, serverName).Result;

        public async Task<(IEnumerable<T1>, IEnumerable<T2>)> FetchMultipleAsync<T1, T2>(ICriteria criteria, string serverName = "")
        {
            const string endpoint = "/api/dynamic-repository/fetch-multiple?collectionCount=2";

            var typeNames = new List<Type>
            {
                typeof(T1),
                typeof(T2)
            };
            var resultInJArray = await ExecuteApiService<JArray>(endpoint, criteria, typeNames, serverName).ConfigureAwait(false);

            return (
                resultInJArray[0].ToObject<IEnumerable<T1>>(JsonSerializer),
                resultInJArray[1].ToObject<IEnumerable<T2>>(JsonSerializer));
        }

        public async Task<(IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>)> FetchMultipleAsync<T1, T2, T3>(ICriteria criteria, string serverName = "")
        {
            const string endpoint = "/api/dynamic-repository/fetch-multiple?collectionCount=3";

            var typeNames = new List<Type>
            {
                typeof(T1),
                typeof(T2),
                typeof(T3)
            };
            var resultInJArray = await ExecuteApiService<JArray>(endpoint, criteria, typeNames, serverName).ConfigureAwait(false);

            return (
                resultInJArray[0].ToObject<IEnumerable<T1>>(JsonSerializer),
                resultInJArray[1].ToObject<IEnumerable<T2>>(JsonSerializer),
                resultInJArray[2].ToObject<IEnumerable<T3>>(JsonSerializer));
        }

        public async Task<(IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>)> FetchMultipleAsync<T1, T2, T3, T4>(ICriteria criteria, string serverName = "")
        {
            const string endpoint = "/api/dynamic-repository/fetch-multiple?collectionCount=4";

            var typeNames = new List<Type>
            {
                typeof(T1),
                typeof(T2),
                typeof(T3),
                typeof(T4)
            };
            var resultInJArray = await ExecuteApiService<JArray>(endpoint, criteria, typeNames, serverName).ConfigureAwait(false);

            return (resultInJArray[0].ToObject<IEnumerable<T1>>(JsonSerializer)
                , resultInJArray[1].ToObject<IEnumerable<T2>>(JsonSerializer)
                , resultInJArray[2].ToObject<IEnumerable<T3>>(JsonSerializer)
                , resultInJArray[3].ToObject<IEnumerable<T4>>(JsonSerializer));
        }

        public async Task<(IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>)> FetchMultipleAsync<T1, T2, T3, T4, T5>(ICriteria criteria, string serverName = "")
        {
            const string endpoint = "/api/dynamic-repository/fetch-multiple?collectionCount=5";

            var typeNames = new List<Type>
            {
                typeof(T1),
                typeof(T2),
                typeof(T3),
                typeof(T4),
                typeof(T5)
            };
            var resultInJArray = await ExecuteApiService<JArray>(endpoint, criteria, typeNames, serverName).ConfigureAwait(false);

            return (resultInJArray[0].ToObject<IEnumerable<T1>>(JsonSerializer)
                , resultInJArray[1].ToObject<IEnumerable<T2>>(JsonSerializer)
                , resultInJArray[2].ToObject<IEnumerable<T3>>(JsonSerializer)
                , resultInJArray[3].ToObject<IEnumerable<T4>>(JsonSerializer)
                , resultInJArray[4].ToObject<IEnumerable<T5>>(JsonSerializer));
        }

        public async Task<(IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>, IEnumerable<T6>)> FetchMultipleAsync<T1, T2, T3, T4, T5, T6>(ICriteria criteria, string serverName = "")
        {
            const string endpoint = "/api/dynamic-repository/fetch-multiple?collectionCount=6";

            var typeNames = new List<Type>
            {
                typeof(T1),
                typeof(T2),
                typeof(T3),
                typeof(T4),
                typeof(T5),
                typeof(T6)
            };
            var resultInJArray = await ExecuteApiService<JArray>(endpoint, criteria, typeNames, serverName).ConfigureAwait(false);

            return (resultInJArray[0].ToObject<IEnumerable<T1>>(JsonSerializer)
                , resultInJArray[1].ToObject<IEnumerable<T2>>(JsonSerializer)
                , resultInJArray[2].ToObject<IEnumerable<T3>>(JsonSerializer)
                , resultInJArray[3].ToObject<IEnumerable<T4>>(JsonSerializer)
                , resultInJArray[4].ToObject<IEnumerable<T5>>(JsonSerializer)
                , resultInJArray[5].ToObject<IEnumerable<T6>>(JsonSerializer));
        }
    }
}