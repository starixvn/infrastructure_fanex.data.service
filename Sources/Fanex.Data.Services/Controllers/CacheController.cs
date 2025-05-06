using Dapper;
using Fanex.Data.Services.Cache;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Fanex.Data.Services.Controllers;

using MassTransit;

[Route("api/cache")]
[ApiController]
public class CacheController(IPublishEndpoint publishEndpoint) : ControllerBase
{
    [HttpPost("remove")]
    public async Task<IActionResult> Remove(List<StoredProcedureDefinition> commands)
    {
        var message = new ClearStoredProcedureCacheMessage
        {
            ServiceName = GetServiceName(),
            StoredProcedureDefinitions = commands
        };

        await publishEndpoint.Publish(message, context =>
        {
            context.TimeToLive = TimeSpan.FromMinutes(15);
        });

        return Ok(new
        {
            ServerName = Environment.MachineName,
            ServerStatus = new Dictionary<string, DateTimeOffset>
            {
                { Environment.MachineName , DateTimeOffset.Now }
            }
        });
    }

    private static string GetServiceName()
    {
        var machineName = Environment.MachineName;
        var lastIndex = machineName.LastIndexOf("-", StringComparison.Ordinal);
        var index = lastIndex >= 0 ? lastIndex + 1 : machineName.Length;
        return machineName[..index];
    }

    [HttpGet("caching-storage")]
    public IActionResult GetCachingStorage()
    {
        return Ok(DbParameterDefinitionCache.GetCachingStorage());
    }

    [HttpGet("purge-cache")]
    public IActionResult PurgeCache()
    {
        SqlMapper.PurgeQueryCache();

        return Ok(Environment.MachineName);
    }

    [HttpGet("clean")]
    public IActionResult CleanCache()
    {
        SqlMapper.PurgeQueryCache();
        DbParameterDefinitionCache.Clear();

        return Ok(Environment.MachineName);
    }

    [HttpGet("dapper-cached")]
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