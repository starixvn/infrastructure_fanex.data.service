using System.Diagnostics;
using System.Runtime;
using Fanex.Data.Services.Cache;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Data.Services;

using System.Text;

[Route("")]
[ApiController]
public class HomeController : ControllerBase
{
    private static bool alive = true;

    private static readonly DateTimeOffset StartTime = DateTimeOffset.Now;

    private static readonly int MinutesToLive = new Random(StartTime.Microsecond).Next(29 * 60, 47 * 60);

    [HttpGet]
    public IActionResult Index()
    {
        return alive && (DateTimeOffset.Now - StartTime).TotalMinutes < MinutesToLive
            ? Ok(new
            {
                Environment.MachineName,
                Alive = alive,
                MinutesToLive,
                WorkedMinutes = (DateTimeOffset.Now - StartTime).TotalMinutes
            })
            : Problem(
                $"WorkedMinutes: {(DateTimeOffset.Now - StartTime).TotalMinutes}",
                Environment.MachineName,
                400,
                $"MinutesToLive: {MinutesToLive}",
                $"Alive: {alive}"
                );
    }

    [HttpGet("kill")]
    public IActionResult Kill()
    {
        alive = false;
        return Ok(Environment.MachineName);
    }

    [HttpGet("statistic")]
    public IActionResult Statistic()
    {
        return Ok(
            new
            {
                Requests = Monitoring.RequestStatistics.Where(x => x > 0),
                Responses = Monitoring.ResponseStatistics,
                server = Environment.MachineName
            });
    }

    [HttpGet("gc")]
    public IActionResult Gc()
    {
        const int megabyte = 1024 * 1024;
        var stringBuilder = new StringBuilder();
        var sw = Stopwatch.StartNew();
        sw.Start();

        using var currentProcess = Process.GetCurrentProcess();
        currentProcess.Refresh();
        var memory = currentProcess.WorkingSet64 / megabyte;

        stringBuilder.Append($"Before GC.Collect: {memory} Mb.\r\n");

        GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
        GC.Collect(GC.MaxGeneration, GCCollectionMode.Aggressive, true, true);
        GC.WaitForPendingFinalizers();
        GC.WaitForFullGCComplete();

        currentProcess.Refresh();
        memory = currentProcess.WorkingSet64 / megabyte;

        var duration = sw.ElapsedMilliseconds;
        stringBuilder.Append($"After GC.Collect. Usage memory = {memory}.\r\n");
        stringBuilder.Append($"Duration: {duration} milliseconds.\r\n");

        return Ok(stringBuilder.ToString());
    }
}