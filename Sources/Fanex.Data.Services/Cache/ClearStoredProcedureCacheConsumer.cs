using MassTransit;

namespace Fanex.Data.Services.Cache;

using Fanex.Data.Services.Extensions;
using Middleware;

public class ClearStoredProcedureCacheConsumer(ILogger<DataServiceLogger> logger) : IConsumer<ClearStoredProcedureCacheMessage>
{
    public async Task Consume(ConsumeContext<ClearStoredProcedureCacheMessage> context)
    {
        var message = context.Message;
        var definitions = message.StoredProcedureDefinitions;
        
        definitions.ForEach(c => DbParameterDefinitionCache.Remove(c.ConnectionString, c.StoreProcedureName));

        var connectionStrings = GetSimplifiedConnectionString(definitions);
        logger.LogInformation("Cache of server {MachineName} is cleared at {Now}.\nConnection String: {ConnectionStrings}",
            MachineIdentifier.MachineName,
            DateTimeOffset.Now,
            connectionStrings);

        await Task.CompletedTask;
    }

    private static string GetSimplifiedConnectionString(List<StoredProcedureDefinition> definitions)
    {
        var connectionStrings = GetSimplifiedConnectionStringCollection(definitions);
        return $"[{string.Join(", ", connectionStrings)}]";
    }

    private static IEnumerable<string> GetSimplifiedConnectionStringCollection(List<StoredProcedureDefinition> definitions)
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