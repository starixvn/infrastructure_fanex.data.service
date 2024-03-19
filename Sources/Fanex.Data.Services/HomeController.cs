using System.Net;
using Dapper;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Data.Services;

[Route("")]
[ApiController]
public class HomeController : ControllerBase
{
    public static DateTimeOffset StartTime { get; set; }

    private static int longevityMinute = GetRandomMinute();

    [HttpGet]
    public IActionResult Index()
    {
        if((DateTimeOffset.UtcNow - StartTime).TotalMinutes> AliveDurationInMinute)
        {
            SqlMapper.PurgeQueryCache();
            return Problem(
                detail: "Time to shutdown", 
                instance: Environment.MachineName, 
                statusCode: (int)HttpStatusCode.ServiceUnavailable);
        }

        return Ok(Environment.MachineName);
    }

    [HttpGet]
    [Route("gc")]
    public IActionResult Gc()
    {
        GC.Collect();
        return Ok("Done Gc");
    }

    [HttpGet("Log")]
    public async Task<IActionResult> Log()
    {
        await Fanex.Logging.Logger.Log.InfoAsync("Test Log");
        return Ok("Log is written");
    }

    [HttpGet("alive")]
    public IActionResult Alive(int longevity)
    {
        longevityMinute = longevity;
        var workedMinute = (DateTimeOffset.UtcNow - StartTime).TotalMinutes;        
        var remainMinute = longevityMinute - workedMinute;
        
        return Ok(new { longevityMinute, workedMinute, remainMinute });
    }


    private static int GetRandomMinute()
    {
        var random = new Random(DateTime.Now.Second);
        return random.Next(24 * 60, 36 * 60);
    }
}