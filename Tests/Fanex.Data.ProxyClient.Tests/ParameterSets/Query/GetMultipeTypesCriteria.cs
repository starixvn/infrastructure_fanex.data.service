using Fanex.Data.Repository;
namespace Fanex.Data.ProxyClient.Tests.ParameterSets.Query
{
    public class GetMultipleTypesCriteria : CriteriaBase
    {
        public int Top { get; set; }
        public override string GetSettingKey() => "GetMultipleTypes";

        public override bool IsValid() => true;
    }
}
