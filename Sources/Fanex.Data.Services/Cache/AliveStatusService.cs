namespace Fanex.Data.Services.Cache;

public class AliveStatusService
{
    private readonly DateTimeOffset startTimeOffset = DateTimeOffset.Now;
    
    private bool isAlive = true;
    
    public AliveStatusService(IConfiguration configuration)
    {
        var minutesToLiveFrom = configuration.GetValue<int>("MinutesToLive:From");
        var minutesToLiveTo = configuration.GetValue<int>("MinutesToLive:To");
        var random = new Random(DateTimeOffset.Now.Millisecond);
        MinutesToLive = random.Next(minutesToLiveFrom, minutesToLiveTo);
    }

    public bool IsAlive()
    {
        return isAlive && MinutesWorked < MinutesToLive;
    }

    public void Kill()
    {
        isAlive = false;
    }

    public int MinutesToLive { get; }

    public int MinutesWorked => (int)(DateTimeOffset.Now - startTimeOffset).TotalMinutes;
}