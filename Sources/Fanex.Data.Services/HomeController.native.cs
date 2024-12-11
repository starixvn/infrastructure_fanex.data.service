using Fanex.Data.Services.Cache;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Data.Services;

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
                aliveStatusService.MinutesToLive,
                aliveStatusService.MinutesWorked
            })
            : Problem(
                $"MinuteWorked: {aliveStatusService.MinutesWorked}",
                Environment.MachineName,
                400,
                $"MinutesToLive: {aliveStatusService.MinutesToLive}",
                $"Alive: {false}"
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
                server = Environment.MachineName
            });
    }

    [HttpGet("gc")]
    public IActionResult Gc()
    {
        var gc = GarbageCollector.CollectExtremely();
        return Ok(gc);
    }
}