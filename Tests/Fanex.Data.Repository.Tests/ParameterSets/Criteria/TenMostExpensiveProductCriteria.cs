namespace Fanex.Data.Repository.Tests.ParameterSets.Criteria;

public class TenMostExpensiveProductCriteria : CriteriaBase
{
    public override string GetSettingKey()
        => "TenMostExpensiveProducts";

    public override bool IsValid()
        => true;
}