namespace Fanex.Data.Repository.Tests.ParameterSets.Criteria
{
    public class AttributeAllowNullCriteria : CriteriaBase
    {
        [SpParam(AllowNull = true)]
        public string ContactTitle { get; set; }

        public override string GetSettingKey()
            => "allowPassingNull";

        public override bool IsValid()
            => true;
    }
}