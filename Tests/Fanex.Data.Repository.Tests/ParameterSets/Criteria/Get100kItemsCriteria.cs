namespace Fanex.Data.Repository.Tests.ParameterSets.Criteria
{
    public class Get100kItemsCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "Get100kItems";

        public override bool IsValid() => true;
    }
}