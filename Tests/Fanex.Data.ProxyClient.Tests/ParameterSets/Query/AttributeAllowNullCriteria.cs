namespace Fanex.Data.ProxyClient.Tests.ParameterSets.Query;

using Repository;

public class AttributeAllowNullCriteria : CriteriaBase
{
    [SpParam(AllowNull = true)]
    public string ContactTitle { get; set; }

    public override string GetSettingKey()
        => "allowPassingNull";

    public override bool IsValid()
        => true;
}