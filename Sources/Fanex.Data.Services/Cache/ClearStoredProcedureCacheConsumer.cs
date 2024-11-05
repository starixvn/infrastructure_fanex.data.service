using MassTransit;

namespace Fanex.Data.Services.Cache;

using Middleware;

public class ClearStoredProcedureCacheConsumer : IConsumer<ClearStoredProcedureCacheMessage>
{
	public async Task Consume(ConsumeContext<ClearStoredProcedureCacheMessage> context)
	{
		var message = context.Message;
		var definitions = message.StoredProcedureDefinitions;
		if (!Environment.MachineName.StartsWith(message.ServiceName) || !definitions.Any())
		{
			return;
		}
		
		definitions.ForEach(c => DbParameterDefinitionCache.Remove(c.ConnectionString, c.StoreProcedureName));

		var connectionStrings = GetSimplifiedConnectionString(definitions);
		Console.WriteLine($"Cache of server {Environment.MachineName} is cleared at {DateTimeOffset.Now}.\nConnection String: {connectionStrings}");

		await Task.CompletedTask;
	}

	private string GetSimplifiedConnectionString(List<StoredProcedureDefinition> definitions)
	{
		var connectionStrings = GetSimplifiedConnectionStringCollection(definitions);
		return $"[{string.Join(", ", connectionStrings)}]";
	}

	private IEnumerable<string> GetSimplifiedConnectionStringCollection(List<StoredProcedureDefinition> definitions)
	{
		var connectionStrings = definitions.Select(d => d.ConnectionString).Distinct();
		foreach (var connectionString in connectionStrings)
		{
			var builder = new ConnectionStringBuilder(connectionString);
			var server = builder.GetServer();
			var user = builder.GetUserId();
			yield return $"{user}@{server}";
		}
	}
}