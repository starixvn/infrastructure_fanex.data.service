namespace Fanex.Data.ProxyClient.Tests.ParameterSets.Query
{
    using Repository;

    public class CustomerByIdCriteria : CriteriaBase
    {
        public string CustomerId { get; set; }

        public override string GetSettingKey()
            => "GetCustomer";

        public override bool IsValid()
            => !string.IsNullOrWhiteSpace(CustomerId);
    }
}