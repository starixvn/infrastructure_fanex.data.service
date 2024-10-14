namespace Fanex.Data.ProxyClient.Tests.ParameterSets.Commands;

using Repository;

public class NewEmployeeTerritoryCommand : NonQueryCommand
{
    public int EmployeeId { get; set; }

    public string TerritoryId { get; set; }

    public override string GetSettingKey()
        => "InsertEmployeeTerritory";

    public override bool IsValid()
        => EmployeeId > 0 && !string.IsNullOrEmpty(TerritoryId);
}