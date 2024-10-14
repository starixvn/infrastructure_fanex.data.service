namespace Fanex.Data.Repository.Tests.ParameterSets.Criteria;

internal class AttributeIgnoreCriteria : CriteriaBase
{
    public string CustomerId { get; set; }

    [SpParam(Ignored = true)]
    public int IgnoredParam { get; set; }

    public override string GetSettingKey()
        => "CustomerByID";

    public override bool IsValid()
        => !string.IsNullOrWhiteSpace(CustomerId);
}