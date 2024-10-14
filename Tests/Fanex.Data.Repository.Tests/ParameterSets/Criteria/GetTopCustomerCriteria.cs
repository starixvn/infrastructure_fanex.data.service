namespace Fanex.Data.Repository.Tests.ParameterSets.Criteria;

internal class GetTopCustomerCriteria : CriteriaBase
{
    public int Top { get; set; }

    public int RemainingCustomers { get; set; }

    public string WaitFor { get; set; } = string.Empty;

    public override string GetSettingKey()
        => "GetTopCustomers";

    public override bool IsValid()
        => Top >= 0;
}