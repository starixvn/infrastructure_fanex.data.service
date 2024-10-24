namespace Fanex.Data.Repository.Tests.ParameterSets.Criteria.ErrorCriteria;

internal class SpNameNotExistCriteria : CriteriaBase
{
    public override string GetSettingKey() => "SpNotExist";

    public override bool IsValid() => true;
}