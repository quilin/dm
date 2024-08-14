using System;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Notifications.Dto;
using Jamq.Client.Abstractions.Consuming;
using Jamq.Client.Rabbit.Consuming;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace DM.Web.API.Notifications;

internal class RealtimeNotificationConsumer : BackgroundService
{
    private readonly ILogger<RealtimeNotificationConsumer> logger;
    private readonly IConsumerBuilder consumerBuilder;
    private readonly RetryPolicy consumeRetryPolicy;

    public RealtimeNotificationConsumer(
        ILogger<RealtimeNotificationConsumer> logger,
        IConsumerBuilder consumerBuilder)
    {
        this.logger = logger;
        this.consumerBuilder = consumerBuilder;
        consumeRetryPolicy = Policy.Handle<Exception>().WaitAndRetry(5,
            attempt => TimeSpan.FromSeconds(1 << attempt),
            (exception, _) => logger.LogWarning(exception, "Could not subscribe to the queue"));
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
        var consumer = consumerBuilder.BuildRabbit<RealtimeNotification, RealtimeNotificationProcessor>(parameters);
        consumeRetryPolicy.Execute(consumer.Subscribe);

        logger.LogDebug("[👂] Realtime notifications consumer is listening to {QueueName} queue",
            parameters.QueueName);
        return Task.CompletedTask;
    }
}