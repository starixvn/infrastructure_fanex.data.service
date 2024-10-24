namespace Fanex.Data.Services.Cache;

using Caching;

public class ServerAliveReportService : BackgroundService
{
    private readonly ICache cache;

    private readonly int timerPeriod;

    public ServerAliveReportService(ICache cache)
    {
        this.cache = cache;

        var random = new Random();
        timerPeriod = random.Next(13, 29);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await TryToReportStatus();

            await Task.Delay(TimeSpan.FromSeconds(timerPeriod), stoppingToken).ConfigureAwait(false);
        }
    }

    private async Task TryToReportStatus()
    {
        try
        {
            await ReportStatus().ConfigureAwait(false);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception when try to report status. Exception message: {e.Message}");
        }
    }

    private async Task ReportStatus()
    {
        var serverStatus = await GetServerAliveStatus().ConfigureAwait(false);
        serverStatus = GetNotExpiredServers(serverStatus);

        var cacheItemOptions = new CacheItemOptions
        {
            AbsoluteExpiration = DateTimeOffset.Now.AddDays(1)
        };
        await cache.SetAsync(CacheKey.ServerStatusCacheKey, serverStatus, cacheItemOptions).ConfigureAwait(false);
    }

    private async Task<Dictionary<string, DateTimeOffset>> GetServerAliveStatus()
    {
        var (found, serverStatus) =
            await cache.TryGetAsync<Dictionary<string, DateTimeOffset>>(CacheKey.ServerStatusCacheKey)
                        .ConfigureAwait(false);
        if (!found)
        {
            serverStatus = [];
        }

        var currentMachineName = Environment.MachineName;
        serverStatus[currentMachineName] = DateTimeOffset.Now;

        return serverStatus;
    }

    private static Dictionary<string, DateTimeOffset> GetNotExpiredServers(Dictionary<string, DateTimeOffset> serverStatus)
    {
        return serverStatus.Where(x => (DateTimeOffset.Now - x.Value).TotalMinutes <= 5)
            .ToDictionary();
    }
}