using System.Net;
using Dapper;
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
        await Fanex.Logging.Logger.Log.InfoAsync("Test Log");
        return Ok("Log is written");
    }

    [HttpGet("alive")]
    public IActionResult Alive(int longevity)
    {
        longevityMinute = longevity;
        var workedMinute = (DateTimeOffset.UtcNow - Monitoring.StartTime).TotalMinutes;
        var remainMinute = longevityMinute - workedMinute;

        return Ok(new { longevityMinute, workedMinute, remainMinute });
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
                }
            });
    }

    private static int GetRandomMinute()
    {
        var primeNumbers = new int[] { 1087, 1091, 1097, 1103, 1109, 1117, 1123, 1129, 1151, 1163, 1171, 1181, 1187, 1193, 1201, 1213, 1217, 1223, 1229, 1237, 1249, 1259, 1277, 1283, 1289, 1297, 1301, 1307, 1319, 1327, 1361, 1367, 1373, 1381, 1399, 1409, 1423, 1427, 1433, 1439, 1447, 1451, 1459, 1471, 1481, 1487, 1493, 1499, 1511, 1523, 1531, 1543, 1549, 1553, 1559, 1567, 1571, 1579, 1583, 1597, 1601, 1607, 1613, 1619, 1627, 1637, 1657, 1663, 1667, 1693, 1697, 1709, 1721, 1733, 1741, 1747, 1753, 1759, 1777, 1783, 1787, 1801, 1811, 1823, 1831, 1847, 1861, 1867, 1871, 1877, 1889, 1901, 1907, 1913, 1931, 1949, 1973, 1979, 1987, 1993, 1997, 2003, 2011, 2017, 2027, 2039, 2053, 2063, 2069, 2081, 2087, 2099, 2111, 2129, 2137, 2141, 2153 };
        var random = new Random(DateTime.Now.Second);
        var index = random.Next(0, primeNumbers.Length);
        return primeNumbers[index];
    }
}