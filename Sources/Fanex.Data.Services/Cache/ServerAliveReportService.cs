namespace Fanex.Data.Services.Cache;

using Caching;

public class ServerAliveReportService : IHostedService, IDisposable
{
    private readonly ICache cache;

    private readonly int timerPeriod;

    private Timer timer;

    public ServerAliveReportService(ICache cache)
    {
        this.cache = cache;

        var random = new Random();
        timerPeriod = random.Next(10, 20);
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        timer = new Timer(Report, null, TimeSpan.Zero, TimeSpan.FromSeconds(timerPeriod));

        await Task.CompletedTask;
    }

    private async void Report(object state)
    {
        var serverStatus = await GetServerAliveStatus();
        CheckExpiredServer(serverStatus);

        var cacheItemOptions = new CacheItemOptions
        {
            AbsoluteExpiration = DateTimeOffset.Now.AddDays(1)
        };
        await cache.SetAsync(CacheKey.ServerStatusCacheKey, serverStatus, cacheItemOptions);
    }

    private async Task<Dictionary<string, DateTimeOffset>> GetServerAliveStatus()
    {
        var (found, serverStatus) =
            await cache.TryGetAsync<Dictionary<string, DateTimeOffset>>(CacheKey.ServerStatusCacheKey);
        if (!found)
        {
            serverStatus = new Dictionary<string, DateTimeOffset>();
        }

        var currentMachineName = Environment.MachineName;
        serverStatus[currentMachineName] = DateTimeOffset.Now;

        return serverStatus;
    }

    private static void CheckExpiredServer(Dictionary<string, DateTimeOffset> serverStatus)
    {
        var keySet = new HashSet<string>(serverStatus.Keys);
        foreach (var machineName in keySet)
        {
            var lastActive = serverStatus[machineName];
            if ((DateTimeOffset.Now - lastActive).TotalMinutes > 5)
            {
                serverStatus.Remove(machineName);
            }
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        timer?.Change(Timeout.Infinite, 0);
        await Task.CompletedTask;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        timer?.Dispose();
    }
}