namespace Fanex.Data.ProxyClient.Tests.ParameterSets.Query;

using Repository;

public class Get100ThousandItemsCriteria : CriteriaBase
{
    public override string GetSettingKey() => "Get100kItems";

    public override bool IsValid() => true;
}