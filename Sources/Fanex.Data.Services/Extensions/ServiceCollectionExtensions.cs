using MassTransit;

namespace Fanex.Data.Services.Extensions;

using Cache;

internal static class ServiceCollectionExtensions
{
    internal static void AddMassTransitRabbitMq(this IServiceCollection service, IConfiguration configuration)
    {
        var host = configuration.GetSection("RabbitMQ:Hosting").Value;
        var virtualHost = configuration.GetSection("RabbitMQ:VirtualHost").Value;
        var username = configuration.GetSection("RabbitMQ:Username").Value ?? string.Empty;
        var password = configuration.GetSection("RabbitMQ:Password").Value ?? string.Empty;
        service.AddMassTransit(busConfigurator =>
        {
            busConfigurator.AddConsumer<ClearStoredProcedureCacheConsumer, ClearStoredProcedureCacheConsumerDefinition>();

            busConfigurator.UsingRabbitMq((context, busFactoryConfigurator) =>
            {
                busFactoryConfigurator.Host(host, virtualHost, h =>
                {
                    h.Username(username);
                    h.Password(password);
                });

                busFactoryConfigurator.ConfigureEndpoints(context);
                busFactoryConfigurator.QueueExpiration = TimeSpan.FromMinutes(15);
            });
        });
    }
}