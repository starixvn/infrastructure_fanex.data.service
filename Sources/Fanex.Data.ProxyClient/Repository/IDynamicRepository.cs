using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Fanex.Data.Repository
{
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
    public interface IDynamicRepository
    {
        (IEnumerable<T1>, IEnumerable<T2>) FetchMultiple<T1, T2>(ICriteria criteria, string serverName = "");
        
        (IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>) FetchMultiple<T1, T2, T3>(ICriteria criteria, string serverName = "");
        
        (IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>) FetchMultiple<T1, T2, T3, T4>(ICriteria criteria, string serverName = "");
        
        (IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>) FetchMultiple<T1, T2, T3, T4, T5>(ICriteria criteria, string serverName = "");
        
        (IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>, IEnumerable<T6>) FetchMultiple<T1, T2, T3, T4, T5, T6>(ICriteria criteria, string serverName = "");
        
        Task<(IEnumerable<T1>, IEnumerable<T2>)> FetchMultipleAsync<T1, T2>(ICriteria criteria, string serverName = "");
        
        Task<(IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>)> FetchMultipleAsync<T1, T2, T3>(ICriteria criteria, string serverName = "");
        
        Task<(IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>)> FetchMultipleAsync<T1, T2, T3, T4>(ICriteria criteria, string serverName = "");
        
        Task<(IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>)> FetchMultipleAsync<T1, T2, T3, T4, T5>(ICriteria criteria, string serverName = "");
        
        Task<(IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>, IEnumerable<T6>)> FetchMultipleAsync<T1, T2, T3, T4, T5, T6>(ICriteria criteria, string serverName = "");
        
        IEnumerable<T> Fetch<T>(ICriteria criteria, string serverName = "");
        
        DataSet FetchDataSet(ICriteria criteria, string serverName = "");
        
        T Get<T>(ICriteria criteria, string serverName = "");
        
        int Execute(INonQueryCommand command, string serverName = "");
        
        Task<IEnumerable<T>> FetchAsync<T>(ICriteria criteria, string serverName = "");
        
        Task<DataSet> FetchDataSetAsync(ICriteria criteria, string serverName = "");
        
        Task<T> GetAsync<T>(ICriteria criteria, string serverName = "");
        
        Task<int> ExecuteAsync(INonQueryCommand command, string serverName = "");
    }
}