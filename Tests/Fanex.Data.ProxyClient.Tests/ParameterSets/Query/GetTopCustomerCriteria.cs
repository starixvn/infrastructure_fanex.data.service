namespace Fanex.Data.ProxyClient.Tests.ParameterSets.Query;

using Repository;

public class GetTopCustomerCriteria : CriteriaBase
{
    public int Top { get; set; }

    public int RemainingCustomers { get; set; }

    public string WaitFor { get; set; }

    public override string GetSettingKey()
        => "GetTopCustomers";

    public override bool IsValid()
        => Top > 0;
}