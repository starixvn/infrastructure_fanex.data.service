namespace Fanex.Data.Services.Cache;

using MassTransit;

public class ClearStoredProcedureCacheConsumerDefinition : ConsumerDefinition<ClearStoredProcedureCacheConsumer>
{
    private const int RetryInterval = 1000;

    private const int RetryCount = 1;

    private const int FetchItemNumber = 1;

    public ClearStoredProcedureCacheConsumerDefinition()
    {
        EndpointName = $"clear-stored-procedure-definition-queue-{Environment.MachineName}";
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<ClearStoredProcedureCacheConsumer> consumerConfigurator,
        IRegistrationContext context)
    {
        if (endpointConfigurator is IRabbitMqReceiveEndpointConfigurator rmq)
        {
            rmq.PrefetchCount = FetchItemNumber;
            rmq.DiscardSkippedMessages();
            rmq.DiscardFaultedMessages();
            rmq.UseMessageRetry(retryConfig =>
                {
                    retryConfig.Interval(RetryCount, TimeSpan.FromMilliseconds(RetryInterval));
                }
            );
        }
    }
}