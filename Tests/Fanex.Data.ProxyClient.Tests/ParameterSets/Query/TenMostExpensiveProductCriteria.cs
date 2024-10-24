namespace Fanex.Data.ProxyClient.Tests.ParameterSets.Query;

using Repository;

public class TenMostExpensiveProductCriteria : CriteriaBase
{
    public override string GetSettingKey()
        => "TenMostExpensiveProducts";

    public override bool IsValid()
        => true;
}