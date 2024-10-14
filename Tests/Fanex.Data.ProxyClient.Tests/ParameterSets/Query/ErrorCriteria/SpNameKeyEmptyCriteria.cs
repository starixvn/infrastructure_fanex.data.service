namespace Fanex.Data.ProxyClient.Tests.ParameterSets.Query.ErrorCriteria;

using Repository;

internal class SpNameKeyEmptyCriteria : CriteriaBase
{
    public override string GetSettingKey() => string.Empty;

    public override bool IsValid() => true;
}