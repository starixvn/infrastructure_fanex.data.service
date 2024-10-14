namespace Fanex.Data.Repository.Tests.ParameterSets.Criteria.ErrorCriteria;

internal class SpNameNotConfiguredCriteria : CriteriaBase
{
    public override string GetSettingKey() => "testSpNameNotConfigured";

    public override bool IsValid() => true;
}