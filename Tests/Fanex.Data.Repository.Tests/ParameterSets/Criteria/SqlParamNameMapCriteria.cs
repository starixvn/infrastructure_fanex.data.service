namespace Fanex.Data.Repository.Tests.ParameterSets.Criteria;

public class SqlParamNameMapCriteria : CriteriaBase
{
    [SpParam(SqlParamName = "CustomerId")]
    public string Id { get; set; }

    public override string GetSettingKey()
        => "CustomerByID";

    public override bool IsValid()
        => !string.IsNullOrEmpty(Id);
}