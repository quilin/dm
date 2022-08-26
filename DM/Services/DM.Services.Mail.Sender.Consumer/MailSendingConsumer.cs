using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RMQ.Client.Abstractions;
using RMQ.Client.Abstractions.Consuming;

namespace DM.Services.Mail.Sender.Consumer;

internal class MailSendingConsumer : BackgroundService
{
    private readonly ILogger<MailSendingConsumer> logger;
    private readonly IConsumerBuilder consumerBuilder;

    public MailSendingConsumer(
        ILogger<MailSendingConsumer> logger,
        IConsumerBuilder consumerBuilder)
    {
        this.logger = logger;
        this.consumerBuilder = consumerBuilder;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogDebug("[🚴] Starting mail sending consumer");

        var parameters = new RabbitConsumerParameters("dm.mail.sender", "dm.mail.sending", ProcessingOrder.Sequential)
        {
            ExchangeName = "dm.mail.sending",
            RoutingKeys = new[] { "#" },
            DeadLetterExchange = "dm.mail.unsent"
        };
        var consumer = consumerBuilder.BuildRabbit<MailSendingProcessor, MailLetter>(parameters);
        consumer.Subscribe();

        logger.LogDebug("[👂] Mail sending consumer is listening to {QueueName} queue", parameters.QueueName);
        return Task.CompletedTask;
    }
}