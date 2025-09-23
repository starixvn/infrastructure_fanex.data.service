namespace Fanex.Data.Services.Cache;

public class AliveStatusService
{
    private readonly DateTimeOffset startTimeOffset = DateTimeOffset.Now;
    
    private bool isAlive = true;
    
    public AliveStatusService(IConfiguration configuration)
    {
        var minutesToLiveFrom = configuration.GetValue<int>("MaximumUptimeMinutes:From");
        var minutesToLiveTo = configuration.GetValue<int>("MaximumUptimeMinutes:To");
        var random = new Random(DateTimeOffset.Now.Millisecond);
        MaximumUptimeMinutes = random.Next(minutesToLiveFrom, minutesToLiveTo);
    }

    public bool IsAlive()
    {
        return isAlive && UptimeMinutes < MaximumUptimeMinutes;
    }

    public void Kill()
    {
        isAlive = false;
    }

    public int MaximumUptimeMinutes { get; }

    public int UptimeMinutes => (int)(DateTimeOffset.Now - startTimeOffset).TotalMinutes;
}