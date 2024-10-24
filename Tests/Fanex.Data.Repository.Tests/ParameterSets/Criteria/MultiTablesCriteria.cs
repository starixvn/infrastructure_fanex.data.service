namespace Fanex.Data.Repository.Tests.ParameterSets.Criteria;

public class MultiTablesCriteria : CriteriaBase
{
    public override string GetSettingKey()
        => "FetchMultipleTables";

    public override bool IsValid()
        => true;
}