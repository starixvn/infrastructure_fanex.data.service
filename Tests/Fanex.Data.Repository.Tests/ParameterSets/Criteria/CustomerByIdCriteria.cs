namespace Fanex.Data.Repository.Tests.ParameterSets.Criteria;

public class CustomerByIdCriteria : CriteriaBase
{
    public string CustomerId { get; set; }

    public override string GetSettingKey()
        => "CustomerByID";

    public override bool IsValid()
        => !string.IsNullOrWhiteSpace(CustomerId);
}