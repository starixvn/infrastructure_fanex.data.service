using Microsoft.AspNetCore.Mvc;

namespace Fanex.Data.Services;

[Route("")]
[ApiController]
public class HomeController : ControllerBase
{
    [HttpGet]
    public IActionResult Index()
    {
        return Ok("OK");
    }
}