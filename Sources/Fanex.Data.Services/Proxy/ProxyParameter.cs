namespace Fanex.Data.Services.Proxy;

public class ProxyParameter
{
    public bool IgnoreRedundantParameters { get; set; }

    public DbSetting DbSetting { get; set; }

    public Dictionary<string, object> ParameterSet { get; set; } = new();

    public List<string> TypeNames { get; set; } = new();
}