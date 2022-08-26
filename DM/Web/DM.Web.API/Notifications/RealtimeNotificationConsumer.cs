using System.Threading;
using System.Threading.Tasks;
using DM.Services.Notifications.Dto;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RMQ.Client.Abstractions;
using RMQ.Client.Abstractions.Consuming;

namespace DM.Web.API.Notifications;

internal class RealtimeNotificationConsumer : BackgroundService
{
    private readonly ILogger<RealtimeNotificationConsumer> logger;
    private readonly IConsumerBuilder consumerBuilder;

    public RealtimeNotificationConsumer(
        ILogger<RealtimeNotificationConsumer> logger,
        IConsumerBuilder consumerBuilder)
    {
        this.logger = logger;
        this.consumerBuilder = consumerBuilder;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogDebug("[🚴] Starting realtime notifications consumer");

        var parameters = new RabbitConsumerParameters("dm.api", "dm.notifications.api", ProcessingOrder.Sequential)
        {
            ExchangeName = "dm.notifications.sent",
            RoutingKeys = new[] { "#" },
            Exclusive = true
        };
        var consumer = consumerBuilder.BuildRabbit<RealtimeNotificationProcessor, RealtimeNotification>(parameters);
        consumer.Subscribe();

        logger.LogDebug("[👂] Realtime notifications consumer is listening to {QueueName} queue", parameters.QueueName);
        return Task.CompletedTask;
    }
}