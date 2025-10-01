namespace Fanex.Data.Services.Extensions;

public static class MachineIdentifier
{
    private static readonly string Id = Guid.NewGuid().ToString();

    public static string MachineName => Environment.MachineName + Id;
}