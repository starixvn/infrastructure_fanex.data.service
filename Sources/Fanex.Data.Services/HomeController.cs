using System.Net;
using Fanex.Data.Services.Cache;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Data.Services;

[Route("")]
[ApiController]
public class HomeController : ControllerBase
{
    private static int longevityMinute = GetRandomMinute();

    [HttpGet]
    public IActionResult Index()
    {
        if ((DateTimeOffset.UtcNow - Monitoring.StartTime).TotalMinutes > longevityMinute)
        {
            return Problem(
                detail: "Time to shutdown",
                instance: Environment.MachineName,
                title: "Time to shutdown",
                type: "Time to shutdown",
                statusCode: (int)HttpStatusCode.ServiceUnavailable);
        }

        return Ok(Environment.MachineName);
    }

    [HttpGet]
    [Route("gc")]
    public IActionResult Gc()
    {
        GC.Collect(2, GCCollectionMode.Forced, true);
        GC.Collect(2, GCCollectionMode.Aggressive, true);
        return Ok("Done Gc");
    }

    [HttpGet("Log")]
    public async Task<IActionResult> Log()
    {
        await Logging.Logger.Log.InfoAsync("Test Log");
        return Ok("Log is written");
    }

    [HttpGet("alive")]
    public IActionResult Alive(int longevity)
    {
        longevityMinute = longevity;
        var workedMinute = (DateTimeOffset.UtcNow - Monitoring.StartTime).TotalMinutes;
        var remainMinute = longevityMinute - workedMinute;

        return Ok(new { longevityMinute, workedMinute, remainMinute, server = Environment.MachineName });
    }

    [HttpGet("statistic")]
    public IActionResult Statistic()
    {
        var workedMinute = (DateTimeOffset.UtcNow - Monitoring.StartTime).TotalMinutes;
        var remainMinute = longevityMinute - workedMinute;

        return Ok(
            new
            {
                Requests = Monitoring.RequestStatistics.Where(x => x > 0),
                Responses = Monitoring.ResponseStatistics,
                Alive = new
                {
                    Longevity = longevityMinute,
                    WorkedMinute = workedMinute,
                    RemainMinute = remainMinute
                },
                server = Environment.MachineName
            });
    }

    private static int GetRandomMinute()
    {
        var random = new Random(DateTime.Now.Second);
        return random.Next(4 * 60, 6 * 60);
    }
}