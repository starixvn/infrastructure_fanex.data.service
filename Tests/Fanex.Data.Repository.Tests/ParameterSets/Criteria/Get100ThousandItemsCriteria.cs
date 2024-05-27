namespace Fanex.Data.Repository.Tests.ParameterSets.Criteria
{
    public class Get100ThousandItemsCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "Get100kItems";

        public override bool IsValid() => true;
    }
}