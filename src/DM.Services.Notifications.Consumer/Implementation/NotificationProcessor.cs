using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DM.Services.MessageQueuing.GeneralBus;
using DM.Services.Notifications.BusinessProcesses.Creating;
using DM.Services.Notifications.Consumer.Implementation.Notifiers;
using DM.Services.Notifications.Dto;
using Jamq.Client.Abstractions.Consuming;
using Jamq.Client.Abstractions.Producing;
using Jamq.Client.Rabbit.Producing;

namespace DM.Services.Notifications.Consumer.Implementation;

/// <inheritdoc />
internal class NotificationProcessor : IProcessor<string, InvokedEvent>
{
    private readonly IEnumerable<INotificationGenerator> generators;
    private readonly INotificationCreatingService service;
    private readonly IMapper mapper;
    private readonly IProducer<string, RealtimeNotification> producer;

    /// <inheritdoc />
    public NotificationProcessor(
        IEnumerable<INotificationGenerator> generators,
        INotificationCreatingService service,
        IMapper mapper,
        IProducerBuilder producerBuilder)
    {
        this.generators = generators;
        this.service = service;
        this.mapper = mapper;
        producer = producerBuilder.BuildRabbit<RealtimeNotification>(
            new RabbitProducerParameters("dm.notifications.sent"));
    }

    /// <inheritdoc />
    public async Task<ProcessResult> Process(string key, InvokedEvent message, CancellationToken cancellationToken)
    {
        var notificationsToCreate = new List<CreateNotification>();
        foreach (var generator in generators.Where(g => g.CanResolve(message.Type)))
        {
            await foreach (var createNotification in generator.Generate(message.EntityId)
                               .WithCancellation(cancellationToken))
            {
                notificationsToCreate.Add(createNotification with { EventType = message.Type });
            }
        }

        if (!notificationsToCreate.Any())
        {
            return ProcessResult.Success;
        }

        var notifications = await service.Create(notificationsToCreate);
        foreach (var notification in notifications.Select(mapper.Map<RealtimeNotification>))
        {
            await producer.Send(string.Empty, notification, cancellationToken);
        }

        return ProcessResult.Success;
    }
}