using System.Diagnostics;
using System.Runtime;
using System;
using Fanex.Data.Services.Cache;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Fanex.Data.Services;

using System.Text;

[Route("")]
[ApiController]
public class HomeController : ControllerBase
{
    [HttpGet]
    public IActionResult Index()
    {
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

        stringBuilder.AppendFormat("Total memory - Before GC.Collect: {0} Mb", memory);

        GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
        GC.Collect(GC.MaxGeneration, GCCollectionMode.Aggressive | GCCollectionMode.Forced, true, true);
        GC.WaitForPendingFinalizers();
        GC.WaitForFullGCComplete();

        currentProcess.Refresh();
        memory = currentProcess.WorkingSet64 / megabyte;

        var duration = sw.ElapsedMilliseconds;
        stringBuilder.AppendFormat("GC.Collected. Usage memory = {0}. Duration: {1} milliseconds", memory, duration);

        return Ok(stringBuilder.ToString());
    }
}