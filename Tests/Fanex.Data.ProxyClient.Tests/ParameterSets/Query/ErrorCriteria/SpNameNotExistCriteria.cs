namespace Fanex.Data.ProxyClient.Tests.ParameterSets.Query.ErrorCriteria;

using Repository;

internal class SpNameNotExistCriteria : CriteriaBase
{
    public override string GetSettingKey() => "SpNotExist";

    public override bool IsValid() => true;
}