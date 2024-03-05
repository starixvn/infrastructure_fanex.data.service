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

    [HttpGet]
    [Route("gc")]
    public IActionResult Gc()
    {
        GC.Collect();
        return Ok("Done Gc");
    }
}