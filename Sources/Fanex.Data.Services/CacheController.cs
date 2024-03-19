using Dapper;
using Fanex.Caching;
using Fanex.Data.Services.Cache;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Fanex.Data.Services;

using Logging;
using System.Diagnostics;

[Route("api/cache")]
[ApiController]
public class CacheController(ICache cache) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Status()
    {
        var serverStatus = await GetServerStatus();

        return Ok(serverStatus);
    }

    [HttpPost("remove")]
    public async Task<IActionResult> Remove(List<StoredProcedureDefinition> commands)
    {
        commands.ForEach(c => DbParameterDefinitionCache.Remove(c.ConnectionString, c.StoreProcedureName));
        var serverName = Environment.MachineName;

        var serverStatus = await GetServerStatus();

        return Ok(new
        {
            ServerName = serverName,
            ServerStatus = serverStatus
        });
    }

    [HttpGet("clear-server-status")]
    public async Task<IActionResult> ClearServerStatus()
    {
        await cache.RemoveAsync(CacheKey.ServerStatusCacheKey);
        return Ok();
    }

    [HttpGet("clear-all-fanex-data-cache")]
    [HttpPost("clear-all-fanex-data-cache")]
    public async Task<IActionResult> ClearAllFanexDataCache()
    {
        DbParameterDefinitionCache.Clear();
        var serverName = Environment.MachineName;

        var serverStatus = await GetServerStatus();

        return Ok(new
        {
            ServerName = serverName,
            ServerStatus = serverStatus
        });
    }

    [HttpGet("Log")]
    public async Task<IActionResult> Log()
    {
        await Logger.Log.InfoAsync("Test Log");
        return Ok("Log is written");
    }


    private async Task<Dictionary<string, DateTimeOffset>> GetServerStatus()
    {
        var (found, serverStatus) =
            await cache.TryGetAsync<Dictionary<string, DateTimeOffset>>(CacheKey.ServerStatusCacheKey);
        if (!found)
        {
            serverStatus = new Dictionary<string, DateTimeOffset>();
        }

        return serverStatus;
    }

    [HttpGet("caching-storage")]
    public IActionResult GetCachingStorage()
    {
        return Ok(DbParameterDefinitionCache.GetCachingStorage());
    }

    [HttpGet("pure-cache")]
    public IActionResult PureCache()
    {
        SqlMapper.PurgeQueryCache();

        return Ok();
    }


    [HttpGet("get-current-memory")]
    public IActionResult GetCurrentMemory()
    {
        Process currentProcess = System.Diagnostics.Process.GetCurrentProcess();
        currentProcess.Refresh();
        return Ok($"MB: {currentProcess.WorkingSet64 / 1000000}");
    }

    [HttpGet("get-dapper-cached")]
    public IActionResult GetDapperCached()
    {
        var cached = SqlMapper.GetCachedSQL();

        var info = new StringBuilder();

        info.AppendLine($"Count: {SqlMapper.GetCachedSQLCount()}");
        foreach (var tuple in cached)
        {
            info.AppendLine($"Key: {tuple.Item2}, HitCount: {tuple.Item3}");
        }

        return Ok(info.ToString());
    }

}