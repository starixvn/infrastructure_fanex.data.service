namespace Fanex.Data.ProxyClient.Tests.ParameterSets.Query;

using Repository;

public class CustomerByIdWithFakeServerNameCriteria : CriteriaBase
{
    public string CustomerId { get; set; }

    public override string GetSettingKey()
        => "GetCustomerWithFakeServerName";

    public override bool IsValid()
        => !string.IsNullOrWhiteSpace(CustomerId);
}