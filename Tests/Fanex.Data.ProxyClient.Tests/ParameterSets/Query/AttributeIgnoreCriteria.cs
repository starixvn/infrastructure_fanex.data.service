namespace Fanex.Data.ProxyClient.Tests.ParameterSets.Query
{
    using Repository;

    internal class AttributeIgnoreCriteria : CriteriaBase
    {
        public string CustomerId { get; set; }

        [SpParam(Ignored = true)]
        public int IgnoredParam { get; set; }

        public override string GetSettingKey()
            => "GetCustomer";

        public override bool IsValid()
            => !string.IsNullOrWhiteSpace(CustomerId);
    }
}