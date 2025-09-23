using Fanex.Data.Services.Cache;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Data.Services.Controllers;

using Olympus.Diagnostic;

[Route("")]
[ApiController]
public class HomeController(AliveStatusService aliveStatusService) : ControllerBase
{
    [HttpGet]
    public IActionResult Index()
    {
        var alive = aliveStatusService.IsAlive();
        return alive
            ? Ok(new
            {
                Environment.MachineName,
                Alive = true,
                aliveStatusService.MaximumUptimeMinutes,
                aliveStatusService.UptimeMinutes
            })
            : Problem(
                $"Alive: false. UptimeMinutes: {aliveStatusService.UptimeMinutes}. MaximumUptimeMinutes: {aliveStatusService.MaximumUptimeMinutes}",
                Environment.MachineName,
                400,
                "Time to shutdown",
                "Time to shutdown"
                );
    }

    [HttpGet("kill")]
    public IActionResult Kill()
    {
        aliveStatusService.Kill();
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
                DbStatistic = Monitoring.DbServerStatistics,
                SpStatistic = Monitoring.SpNameStatistics,
                Server = Environment.MachineName,
                aliveStatusService.UptimeMinutes,
                aliveStatusService.MaximumUptimeMinutes
            });
    }

    [HttpGet("gc")]
    public IActionResult Gc()
    {
        var gc = GarbageCollector.CollectExtremely();
        return Ok($"{Environment.MachineName}. {gc}");
    }
}