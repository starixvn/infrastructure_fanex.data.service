namespace Fanex.Data.ProxyClient.Tests.ParameterSets.Query;

public class GetTopCustomerWithMapping : GetTopCustomerCriteria
{
    [SpParam(Ignored = true)]
    public bool ClassEmpty { get; set; } = false;

    public override string GetSettingKey()
        => ClassEmpty ? "GetTopCustomersWithMappingClassEmpty" : "GetTopCustomersWithMapping";
}