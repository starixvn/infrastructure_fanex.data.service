using Dapper;
using Fanex.Data.Services.Cache;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Fanex.Data.Services.Controllers;

using Fanex.Data.Services.Extensions;
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
            ServiceName = MachineIdentifier.MachineName,
            StoredProcedureDefinitions = commands
        };

        await publishEndpoint.Publish(message, context =>
        {
            context.TimeToLive = TimeSpan.FromMinutes(1);
        });

        return Ok(new
        {
            ServerName = MachineIdentifier.MachineName,
            ServerStatus = new Dictionary<string, DateTimeOffset>
            {
                { MachineIdentifier.MachineName , DateTimeOffset.Now }
            }
        });
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

        return Ok(MachineIdentifier.MachineName);
    }

    [HttpGet("clean")]
    public IActionResult CleanCache()
    {
        SqlMapper.PurgeQueryCache();
        DbParameterDefinitionCache.Clear();

        return Ok(MachineIdentifier.MachineName);
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