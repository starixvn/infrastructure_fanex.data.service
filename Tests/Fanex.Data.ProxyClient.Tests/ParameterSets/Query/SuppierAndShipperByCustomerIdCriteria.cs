namespace Fanex.Data.ProxyClient.Tests.ParameterSets.Query;

using Repository;

public class SupplierAndShipperByCustomerIdCriteria : CriteriaBase
{
    public string CustomerId { get; set; }

    public int NumberOfOrders { get; set; }

    public override string GetSettingKey()
        => "SupplierAndShipperByCustomerID";

    public override bool IsValid()
        => !string.IsNullOrWhiteSpace(CustomerId);
}