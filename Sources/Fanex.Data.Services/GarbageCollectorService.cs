namespace Fanex.Data.Services;

public class GarbageCollectorService : BackgroundService
{
    private readonly Random random = new(DateTimeOffset.Now.Millisecond);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        const int minSeconds = 2 * 60 * 60;
        const int maxSeconds = 3 * 60 * 60;
        while (!stoppingToken.IsCancellationRequested)
        {
            var delaySeconds = random.Next(minSeconds, maxSeconds);
            await Task.Delay(TimeSpan.FromSeconds(delaySeconds), stoppingToken).ConfigureAwait(false);
            GC.Collect(2, GCCollectionMode.Aggressive, true, true);
        }
    }
}