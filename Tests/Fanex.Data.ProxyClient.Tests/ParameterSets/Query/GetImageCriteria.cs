using Fanex.Data.Repository;

namespace Fanex.Data.ProxyClient.Tests.ParameterSets.Query;
public class GetImageCriteria : CriteriaBase
{
    public override string GetSettingKey() => "GenerateImages";

    public override bool IsValid() => true;
}
