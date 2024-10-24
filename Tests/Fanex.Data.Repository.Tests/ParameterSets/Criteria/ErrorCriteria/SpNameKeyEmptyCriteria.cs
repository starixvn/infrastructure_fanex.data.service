namespace Fanex.Data.Repository.Tests.ParameterSets.Criteria.ErrorCriteria;

internal class SpNameKeyEmptyCriteria : CriteriaBase
{
    public override string GetSettingKey() => string.Empty;

    public override bool IsValid() => true;
}