namespace Fanex.Data.ProxyClient.Tests.ParameterSets.Query;

using Repository;

public class SqlParamNameMapCriteria : CriteriaBase
{
    [SpParam(SqlParamName = "CustomerId")]
    public string Id { get; set; }

    public override string GetSettingKey()
        => "GetCustomer";

    public override bool IsValid()
        => !string.IsNullOrEmpty(Id);
}