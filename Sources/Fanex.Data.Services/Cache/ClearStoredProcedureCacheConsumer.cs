using MassTransit;

namespace Fanex.Data.Services.Cache;

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

		var procedure = definitions.Select(d => d.StoreProcedureName).FirstOrDefault();

		definitions.ForEach(c => DbParameterDefinitionCache.Remove(c.ConnectionString, c.StoreProcedureName));
		Console.WriteLine($"Cache of server {Environment.MachineName} is cleared at {DateTimeOffset.Now}. One of procedure: {procedure}");

		await Task.CompletedTask;
	}
}