
namespace Fanex.Data.ProxyClient.Tests.ParameterSets.Query;

using Repository;

public class GetImageCriteria : CriteriaBase
{
    public override string GetSettingKey() => "GenerateImages";

    public override bool IsValid() => true;
}