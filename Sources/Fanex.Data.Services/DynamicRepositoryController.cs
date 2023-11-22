using Fanex.Data.Services.Proxy;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Data.Services;

using Extensions;
using Newtonsoft.Json;
using System.Data;

[Route("api/dynamic-repository")]
[ApiController]
public class DynamicRepositoryController : ControllerBase
{
    [HttpPost("fetch-simple-type")]
    public async Task<IActionResult> FetchSimpleType(ProxyParameter proxyParameter)
    {
        using var objectDb = Proxy.ObjectDbFactory.CreateInstance(proxyParameter);

        var queriedModels = await objectDb.QueryAsync<dynamic>(proxyParameter.ParameterSet);

        var proxyResult = new ProxyResult<IEnumerable<object>>
        {
            Result = DictionaryExtensions.GetFirstColumn(queriedModels),
            ParameterSet = JsonConvert.SerializeObject(proxyParameter.ParameterSet)
        };

        return Ok(proxyResult);
    }

    [HttpPost("fetch")]
    public async Task<IActionResult> Fetch(ProxyParameter proxyParameter)
    {
        using var objectDb = Proxy.ObjectDbFactory.CreateInstance(proxyParameter);

        var models = await objectDb.QueryAsync<dynamic>(proxyParameter.ParameterSet);
        var columnMaps = proxyParameter.DbSetting.ColumnMaps ?? new Dictionary<string, string>();
        var typeNames = proxyParameter.TypeNames ?? new List<string>();
        columnMaps.MapCollection(typeNames.Any() ? typeNames[0] : string.Empty, models);

        var proxyResult = new ProxyResult<IEnumerable<dynamic>>
        {
            Result = models,
            ParameterSet = JsonConvert.SerializeObject(proxyParameter.ParameterSet)
        };

        return Ok(proxyResult);
    }

    [HttpPost("fetch-dataset")]
    public async Task<IActionResult> FetchDataSet(ProxyParameter proxyParameter)
    {
        using var objectDb = Proxy.ObjectDbFactory.CreateInstance(proxyParameter);
        var model = await objectDb.ExecuteDataSetAsync(proxyParameter.ParameterSet);

        var proxyResult = new ProxyResult<DataSet>
        {
            Result = model,
            ParameterSet = JsonConvert.SerializeObject(proxyParameter.ParameterSet)
        };

        return Ok(proxyResult);
    }

    [HttpPost("fetch-multiple")]
    public async Task<IActionResult> FetchMultiple([FromQuery] int collectionCount, ProxyParameter proxyParameter)
    {
        var fetchedData = await FetchMultipleAsync(proxyParameter, collectionCount).ConfigureAwait(false);
        var typeNames = proxyParameter.TypeNames ?? new List<string>();
        var columnMaps = proxyParameter.DbSetting.ColumnMaps ?? new Dictionary<string, string>();
        for (var i = 0; i < typeNames.Count; i++)
        {
            columnMaps.MapCollection(typeNames[i], fetchedData[i]);
        }

        var proxyResult = new ProxyResult<List<dynamic>[]>
        {
            Result = fetchedData,
            ParameterSet = JsonConvert.SerializeObject(proxyParameter.ParameterSet)
        };

        return Ok(proxyResult);
    }

    private static async Task<List<dynamic>[]> FetchMultipleAsync(ProxyParameter proxyParameter, int collectionCount)
    {
        var result = InitializeFetchMultipleResultSet(collectionCount);

        using var objectDb = Proxy.ObjectDbFactory.CreateInstance(proxyParameter);
        using var multiResultReader = await objectDb.QueryMultipleAsync(proxyParameter.ParameterSet);

        var index = 0;

        while (!multiResultReader.IsEmptyOrClosed && index < collectionCount)
        {
            result[index] = (await multiResultReader.ReadAsync<dynamic>()).ToList();
            index++;
        }

        return result;
    }

    private static List<dynamic>[] InitializeFetchMultipleResultSet(int collectionCount)
    {
        var result = new List<object>[collectionCount];

        for (var index = 0; index < collectionCount; index++)
        {
            result[index] = new List<dynamic>();
        }

        return result;
    }

    [HttpPost("get-simple-type")]
    public async Task<IActionResult> GetSimpleType(ProxyParameter proxyParameter)
    {
        using var objectDb = Proxy.ObjectDbFactory.CreateInstance(proxyParameter);
        var queriedModel = await objectDb.QueryEntityAsync<dynamic>(proxyParameter.ParameterSet);

        var columnMaps = proxyParameter.DbSetting.ColumnMaps ?? new Dictionary<string, string>();
        DictionaryExtensions.MapSingle(columnMaps, string.Empty, queriedModel);

        var proxyResult = new ProxyResult<object>
        {
            Result = DictionaryExtensions.GetFirstProperty(queriedModel),
            ParameterSet = JsonConvert.SerializeObject(proxyParameter.ParameterSet)
        };

        return Ok(proxyResult);
    }

    [HttpPost("get")]
    public async Task<IActionResult> Get(ProxyParameter proxyParameter)
    {
        using var objectDb = Proxy.ObjectDbFactory.CreateInstance(proxyParameter);
        var model = await objectDb.QueryEntityAsync<dynamic>(proxyParameter.ParameterSet);

        var columnMaps = proxyParameter.DbSetting.ColumnMaps ?? new Dictionary<string, string>();
        DictionaryExtensions.MapSingle(columnMaps, string.Empty, model);

        var proxyResult = new ProxyResult<dynamic>
        {
            Result = model,
            ParameterSet = JsonConvert.SerializeObject(proxyParameter.ParameterSet)
        };

        return Ok(proxyResult);
    }

    [HttpPost("execute")]
    public async Task<IActionResult> Execute(ProxyParameter proxyParameter)
    {
        using var objectDb = Proxy.ObjectDbFactory.CreateInstance(proxyParameter);
        var model = await objectDb.ExecuteNonQueryAsync(proxyParameter.ParameterSet);

        var proxyResult = new ProxyResult<int>
        {
            Result = model,
            ParameterSet = JsonConvert.SerializeObject(proxyParameter.ParameterSet)
        };

        return Ok(proxyResult);
    }
}