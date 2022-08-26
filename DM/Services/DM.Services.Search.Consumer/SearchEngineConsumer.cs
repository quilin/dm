using System.Threading;
using System.Threading.Tasks;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Implementation;
using DM.Services.MessageQueuing.GeneralBus;
using DM.Services.Search.Configuration;
using DM.Services.Search.Consumer.Implementation;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nest;
using RMQ.Client.Abstractions;
using RMQ.Client.Abstractions.Consuming;

namespace DM.Services.Search.Consumer;

internal class SearchEngineConsumer : BackgroundService
{
    private readonly ILogger<SearchEngineConsumer> logger;
    private readonly IElasticClient elasticClient;
    private readonly IConsumerBuilder consumerBuilder;

    public SearchEngineConsumer(
        ILogger<SearchEngineConsumer> logger,
        IElasticClient elasticClient,
        IConsumerBuilder consumerBuilder)
    {
        this.logger = logger;
        this.elasticClient = elasticClient;
        this.consumerBuilder = consumerBuilder;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogDebug("[🚴] Starting search engine consumer");

        var existsResponse = elasticClient.Indices.Exists(SearchEngineConfiguration.IndexName);
        if (existsResponse is not { IsValid: true, Exists: true })
        {
            logger.LogDebug("Creating search engine index {IndexName}", SearchEngineConfiguration.IndexName);
            var createIndexResponse = elasticClient.Indices.Create(SearchEngineConfiguration.IndexName);
            if (createIndexResponse is not { IsValid: true, Index: SearchEngineConfiguration.IndexName })
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
        var consumer = consumerBuilder.BuildRabbit<CompositeIndexer, InvokedEvent>(parameters);
        consumer.Subscribe();

        logger.LogDebug("[👂] Search engine consumer is listening to {QueueName} queue", parameters.QueueName);
        return Task.CompletedTask;
    }
}