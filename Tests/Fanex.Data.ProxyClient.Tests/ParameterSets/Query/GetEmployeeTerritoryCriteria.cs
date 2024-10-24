namespace Fanex.Data.ProxyClient.Tests.ParameterSets.Query;

using Repository;

public class GetEmployeeTerritoryCriteria : CriteriaBase
{
    public int EmployeeId { get; set; }

    public string TerritoryId { get; set; }

    public override string GetSettingKey()
        => "GetEmployeeTerritory";

    public override bool IsValid()
        => EmployeeId > 0 && !string.IsNullOrEmpty(TerritoryId);
}