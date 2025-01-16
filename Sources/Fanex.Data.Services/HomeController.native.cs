using Fanex.Data.Services.Cache;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Data.Services;

using System.Text;
using MassTransit.MessageData.Values;
using Olympus.Diagnostic;

[Route("")]
[ApiController]
public class HomeController(AliveStatusService aliveStatusService) : ControllerBase
{
    private readonly Random random = new Random(DateTimeOffset.Now.Millisecond);

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

    private static byte[] randomBytes;

    [HttpGet("random")]
    public IActionResult Random()
    {
        ReadBytes();
        randomBytes[20] = (byte)random.Next(0x61, 0x7B);
        return File(randomBytes, "text/plain", $"{Guid.NewGuid()}.txt");
    }

    private static void ReadBytes()
    {
        if (randomBytes != null)
        {
            return;
        }
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "content.txt");
        randomBytes = System.IO.File.ReadAllBytes(filePath);
    }
}