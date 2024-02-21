namespace Fanex.Data.ProxyClient.Tests.ParameterSets.Query
{
    using Repository;

    public class Get100kItemsCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "Get100kItems";

        public override bool IsValid() => true;
    }
}