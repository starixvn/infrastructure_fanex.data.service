using Microsoft.AspNetCore.Mvc;
using Fanex.Data.Services.Cache;
using Fanex.Caching;

namespace Fanex.Data.Services;

[Route("api/cache")]
[ApiController]
public class CacheController(ICache cache) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Status()
    {
        var serverStatus= await GetServerStatus();
        
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

    [HttpGet("clear")]
    public async Task<IActionResult> Clear()
    {
        await cache.RemoveAsync(CacheKey.ServerStatusCacheKey);
        return Ok();
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
}