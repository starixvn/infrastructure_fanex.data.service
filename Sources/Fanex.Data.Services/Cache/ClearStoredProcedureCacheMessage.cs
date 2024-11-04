namespace Fanex.Data.Services.Cache;

public class ClearStoredProcedureCacheMessage
{
	public string ServiceName { get; set; } = string.Empty;

	public List<StoredProcedureDefinition> StoredProcedureDefinitions { get; set; } = [];
}