using System;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Implementation;
using DM.Services.MessageQueuing.GeneralBus;
using DM.Services.Search.Consumer.Implementation;
using Jamq.Client.Abstractions.Consuming;
using Jamq.Client.Rabbit.Consuming;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenSearch.Client;
using Polly;
using Polly.Retry;
using Policy = Polly.Policy;

namespace DM.Services.Search.Consumer;

internal class SearchEngineConsumer : BackgroundService
{
    private readonly ILogger<SearchEngineConsumer> logger;
    private readonly IOpenSearchClient elasticClient;
    private readonly IConsumerBuilder consumerBuilder;
    private readonly RetryPolicy consumeRetryPolicy;

    public SearchEngineConsumer(
        ILogger<SearchEngineConsumer> logger,
        IOpenSearchClient elasticClient,
        IConsumerBuilder consumerBuilder)
    {
        this.logger = logger;
        this.elasticClient = elasticClient;
        this.consumerBuilder = consumerBuilder;
        consumeRetryPolicy = Policy.Handle<Exception>().WaitAndRetry(5,
            attempt => TimeSpan.FromSeconds(1 << attempt),
            (exception, _) => logger.LogWarning(exception, "Could not subscribe to the queue"));
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogDebug("[🚴] Starting search engine consumer");

        var existsResponse = elasticClient.Indices.Exists(Configuration.SearchEngineConfiguration.IndexName);
        if (existsResponse is not { IsValid: true, Exists: true })
        {
            logger.LogDebug("Creating search engine index {IndexName}", Configuration.SearchEngineConfiguration.IndexName);
            var createIndexResponse = elasticClient.Indices.Create(Configuration.SearchEngineConfiguration.IndexName);
            if (createIndexResponse is not { IsValid: true, Index: Configuration.SearchEngineConfiguration.IndexName })
            {
                logger.LogError("Could not create search index on consumer start");
            }
        }

        var parameters = new RabbitConsumerParameters("dm.search-engine", "dm.search-engine", ProcessingOrder.Unmanaged)
        {
            ExchangeName = InvokedEventsTransport.ExchangeName,
            RoutingKeys = new[]
            {
                EventType.ActivatedUser,
                EventType.NewForumComment,
                EventType.ChangedForumComment,
                EventType.DeletedForumComment,
                EventType.NewForumTopic,
                EventType.ChangedForumTopic,
                EventType.DeletedForumTopic,
            }.ToRoutingKeys(),
        };
        var consumer = consumerBuilder.BuildRabbit<InvokedEvent, CompositeIndexer>(parameters);
        consumeRetryPolicy.Execute(consumer.Subscribe);

        logger.LogDebug("[👂] Search engine consumer is listening to {QueueName} queue", parameters.QueueName);
        return Task.CompletedTask;
    }
}