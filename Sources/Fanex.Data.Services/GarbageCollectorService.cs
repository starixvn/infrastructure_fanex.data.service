using System.Diagnostics;
using System.Runtime;

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

            using var currentProcess = Process.GetCurrentProcess();
            currentProcess.Refresh();
            var memory = currentProcess.WorkingSet64 / megabyte;
            logger.LogInformation("=============Total memory - Before ==========: {Memory} Mb", memory);

            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Aggressive | GCCollectionMode.Forced, true, true);
            GC.WaitForPendingFinalizers();
            GC.WaitForFullGCComplete();

            currentProcess.Refresh();
            memory = currentProcess.WorkingSet64 / megabyte;

            var duration = sw.ElapsedMilliseconds;
            logger.LogInformation("GC.Collected. Usage memory = {Memory}. Duration: {Duration} milliseconds", memory, duration);
        }
    }
}