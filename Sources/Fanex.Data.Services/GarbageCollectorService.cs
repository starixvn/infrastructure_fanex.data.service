using System.Diagnostics;

namespace Fanex.Data.Services;

public class GarbageCollectorService(ILogger<GarbageCollectorService> logger) : BackgroundService
{
    private readonly Random random = new(DateTimeOffset.Now.Millisecond);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        const int minSeconds = 60 * 60;
        const int maxSeconds = 120 * 60;
        const int megabyte = 1024 * 1024;
        while (!stoppingToken.IsCancellationRequested)
        {
            var delaySeconds = random.Next(minSeconds, maxSeconds);
            await Task.Delay(TimeSpan.FromSeconds(delaySeconds), stoppingToken).ConfigureAwait(false);

            Stopwatch sw = Stopwatch.StartNew();
            sw.Start();
            
            GC.Collect(2, GCCollectionMode.Aggressive, true, true);
            using var currentProcess = Process.GetCurrentProcess();
            currentProcess.Refresh();
            var memory = currentProcess.WorkingSet64 / megabyte;
            sw.Stop();
            
            var duration = sw.ElapsedMilliseconds;
            logger.LogInformation("GC.Collected. Usage memory = {Memory}. Duration: {Duration} milliseconds", memory, duration);
        }
    }
}