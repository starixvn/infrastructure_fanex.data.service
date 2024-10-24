namespace Fanex.Data.ProxyClient.Tests.ParameterSets.Query;

using Repository;

public class MultiTablesCriteria : CriteriaBase
{
    public override string GetSettingKey()
        => "FetchMultipleTables";

    public override bool IsValid()
        => true;
}