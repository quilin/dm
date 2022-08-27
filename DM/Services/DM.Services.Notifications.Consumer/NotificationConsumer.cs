﻿using System;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Implementation;
using DM.Services.MessageQueuing.GeneralBus;
using DM.Services.Notifications.Consumer.Implementation;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using RMQ.Client.Abstractions;
using RMQ.Client.Abstractions.Consuming;

namespace DM.Services.Notifications.Consumer;

internal class NotificationConsumer : BackgroundService
{
    private readonly ILogger<NotificationConsumer> logger;
    private readonly IConsumerBuilder consumerBuilder;
    private readonly RetryPolicy consumeRetryPolicy;

    public NotificationConsumer(
        ILogger<NotificationConsumer> logger,
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
        logger.LogDebug("[🚴] Starting notifications consumer");

        var parameters = new RabbitConsumerParameters("dm.notifications", "dm.notifications", ProcessingOrder.Unmanaged)
        {
            ExchangeName = InvokedEventsTransport.ExchangeName,
            RoutingKeys = new[]
            {
                EventType.ActivatedUser,
                EventType.NewForumComment,
                EventType.ChangedForumComment,
                EventType.DeletedForumComment,
                EventType.LikedForumComment,
                EventType.NewForumTopic,
                EventType.ChangedForumTopic,
                EventType.DeletedForumTopic,
                EventType.NewCharacter,
                EventType.LikedTopic
            }.ToRoutingKeys(),
        };
        var consumer = consumerBuilder.BuildRabbit<NotificationProcessor, InvokedEvent>(parameters);
        consumeRetryPolicy.Execute(consumer.Subscribe);

        logger.LogDebug("[👂] Notifications consumer is listening to {QueueName} queue", parameters.QueueName);
        return Task.CompletedTask;
    }
}