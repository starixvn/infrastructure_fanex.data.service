namespace Fanex.Data.ProxyClient.Tests.ParameterSets.Query.ErrorCriteria
{
    using Repository;

    internal class SpNameNotConfiguredCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "testSpNameNotConfigured";

        public override bool IsValid() => true;
    }
}