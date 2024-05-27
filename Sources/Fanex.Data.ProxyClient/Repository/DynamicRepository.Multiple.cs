namespace Fanex.Data.Repository
{
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    public partial class DynamicRepository : IDynamicRepository
    {
        public (IEnumerable<T1>, IEnumerable<T2>) FetchMultiple<T1, T2>(ICriteria criteria, string serverIp = "") => FetchMultipleAsync<T1, T2>(criteria, serverIp).GetAwaiter().GetResult();

        public (IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>) FetchMultiple<T1, T2, T3>(ICriteria criteria, string serverIp = "") => FetchMultipleAsync<T1, T2, T3>(criteria, serverIp).GetAwaiter().GetResult();

        public (IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>) FetchMultiple<T1, T2, T3, T4>(ICriteria criteria, string serverIp = "")
            => FetchMultipleAsync<T1, T2, T3, T4>(criteria, serverIp).GetAwaiter().GetResult();

        public (IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>) FetchMultiple<T1, T2, T3, T4, T5>(ICriteria criteria, string serverIp = "") => FetchMultipleAsync<T1, T2, T3, T4, T5>(criteria, serverIp).GetAwaiter().GetResult();

        public (IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>, IEnumerable<T6>) FetchMultiple<T1, T2, T3, T4, T5, T6>(ICriteria criteria, string serverIp = "") => FetchMultipleAsync<T1, T2, T3, T4, T5, T6>(criteria, serverIp).GetAwaiter().GetResult();

        public async Task<(IEnumerable<T1>, IEnumerable<T2>)> FetchMultipleAsync<T1, T2>(ICriteria criteria, string serverIp = "")
        {
            const string endpoint = "/api/dynamic-repository/fetch-multiple?collectionCount=2";

            if (!criteria.IsValid())
            {
                return (
                    Enumerable.Empty<T1>(),
                    Enumerable.Empty<T2>()
                );
            }
            var typeNames = new List<Type>
            {
                typeof(T1),
                typeof(T2)
            };
            var resultInJArray = await ExecuteApiService<JArray>(endpoint, criteria, typeNames, serverIp).ConfigureAwait(false);

            return (
                resultInJArray[0].ToList<T1>(typeof(T1), JsonSerializer),
                resultInJArray[1].ToList<T2>(typeof(T2), JsonSerializer));
        }

        public async Task<(IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>)> FetchMultipleAsync<T1, T2, T3>(ICriteria criteria, string serverIp = "")
        {
            const string endpoint = "/api/dynamic-repository/fetch-multiple?collectionCount=3";

            if (!criteria.IsValid())
            {
                return (
                    Enumerable.Empty<T1>(),
                    Enumerable.Empty<T2>(),
                    Enumerable.Empty<T3>()
                );
            }

            var typeNames = new List<Type>
            {
                typeof(T1),
                typeof(T2),
                typeof(T3)
            };
            var resultInJArray = await ExecuteApiService<JArray>(endpoint, criteria, typeNames, serverIp).ConfigureAwait(false);

            return (
                resultInJArray[0].ToList<T1>(typeof(T1), JsonSerializer),
                resultInJArray[1].ToList<T2>(typeof(T2), JsonSerializer),
                resultInJArray[2].ToList<T3>(typeof(T3), JsonSerializer));
        }

        public async Task<(IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>)> FetchMultipleAsync<T1, T2, T3, T4>(ICriteria criteria, string serverIp = "")
        {
            const string endpoint = "/api/dynamic-repository/fetch-multiple?collectionCount=4";

            if (!criteria.IsValid())
            {
                return (
                    Enumerable.Empty<T1>(),
                    Enumerable.Empty<T2>(),
                    Enumerable.Empty<T3>(),
                    Enumerable.Empty<T4>()
                );
            }

            var typeNames = new List<Type>
            {
                typeof(T1),
                typeof(T2),
                typeof(T3),
                typeof(T4)
            };
            var resultInJArray = await ExecuteApiService<JArray>(endpoint, criteria, typeNames, serverIp).ConfigureAwait(false);

            return (resultInJArray[0].ToList<T1>(typeof(T1), JsonSerializer)
                , resultInJArray[1].ToList<T2>(typeof(T2), JsonSerializer)
                , resultInJArray[2].ToList<T3>(typeof(T3), JsonSerializer)
                , resultInJArray[3].ToList<T4>(typeof(T4), JsonSerializer));
        }

        public async Task<(IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>)> FetchMultipleAsync<T1, T2, T3, T4, T5>(ICriteria criteria, string serverIp = "")
        {
            const string endpoint = "/api/dynamic-repository/fetch-multiple?collectionCount=5";

            if (!criteria.IsValid())
            {
                return (
                    Enumerable.Empty<T1>(),
                    Enumerable.Empty<T2>(),
                    Enumerable.Empty<T3>(),
                    Enumerable.Empty<T4>(),
                    Enumerable.Empty<T5>()
                );
            }

            var typeNames = new List<Type>
            {
                typeof(T1),
                typeof(T2),
                typeof(T3),
                typeof(T4),
                typeof(T5)
            };
            var resultInJArray = await ExecuteApiService<JArray>(endpoint, criteria, typeNames, serverIp).ConfigureAwait(false);

            return (resultInJArray[0].ToList<T1>(typeof(T1), JsonSerializer)
                , resultInJArray[1].ToList<T2>(typeof(T2), JsonSerializer)
                , resultInJArray[2].ToList<T3>(typeof(T3), JsonSerializer)
                , resultInJArray[3].ToList<T4>(typeof(T4), JsonSerializer)
                , resultInJArray[4].ToList<T5>(typeof(T5), JsonSerializer));
        }

        public async Task<(IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>, IEnumerable<T6>)> FetchMultipleAsync<T1, T2, T3, T4, T5, T6>(ICriteria criteria, string serverIp = "")
        {
            const string endpoint = "/api/dynamic-repository/fetch-multiple?collectionCount=6";

            if (!criteria.IsValid())
            {
                return (
                    Enumerable.Empty<T1>(),
                    Enumerable.Empty<T2>(),
                    Enumerable.Empty<T3>(),
                    Enumerable.Empty<T4>(),
                    Enumerable.Empty<T5>(),
                    Enumerable.Empty<T6>()
                    );
            }

            var typeNames = new List<Type>
            {
                typeof(T1),
                typeof(T2),
                typeof(T3),
                typeof(T4),
                typeof(T5),
                typeof(T6)
            };
            var resultInJArray = await ExecuteApiService<JArray>(endpoint, criteria, typeNames, serverIp).ConfigureAwait(false);

            return (resultInJArray[0].ToList<T1>(typeof(T1), JsonSerializer)
                , resultInJArray[1].ToList<T2>(typeof(T2), JsonSerializer)
                , resultInJArray[2].ToList<T3>(typeof(T3), JsonSerializer)
                , resultInJArray[3].ToList<T4>(typeof(T4), JsonSerializer)
                , resultInJArray[4].ToList<T5>(typeof(T5), JsonSerializer)
                , resultInJArray[5].ToList<T6>(typeof(T6), JsonSerializer));
        }
    }
}