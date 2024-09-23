using Fanex.Data.Services.Cache;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.Data.Services;

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
        GC.Collect(2, GCCollectionMode.Aggressive, true, true);
        
        return Ok(Environment.MachineName);
    }
}