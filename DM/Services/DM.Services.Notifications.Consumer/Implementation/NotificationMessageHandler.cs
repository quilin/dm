using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DM.Services.MessageQueuing;
using DM.Services.MessageQueuing.Building;
using DM.Services.MessageQueuing.GeneralBus;
using DM.Services.MessageQueuing.RabbitMq.Configuration;
using DM.Services.Notifications.BusinessProcesses.Creating;
using DM.Services.Notifications.Consumer.Implementation.Notifiers;
using DM.Services.Notifications.Dto;

namespace DM.Services.Notifications.Consumer.Implementation
{
    /// <inheritdoc />
    internal class NotificationMessageHandler : IMessageHandler<InvokedEvent>
    {
        private readonly IEnumerable<INotificationGenerator> generators;
        private readonly INotificationCreatingService service;
        private readonly IMapper mapper;
        private readonly IProducer<string, RealtimeNotification> producer;

        /// <inheritdoc />
        public NotificationMessageHandler(
            IEnumerable<INotificationGenerator> generators,
            INotificationCreatingService service,
            IMapper mapper,
            IProducerBuilder<string, RealtimeNotification> producerBuilder)
        {
            this.generators = generators;
            this.service = service;
            this.mapper = mapper;
            producer = producerBuilder.BuildRabbit(new RabbitProducerParameters
            {
                ExchangeName = "dm.notifications.sent"
            });
        }

        /// <inheritdoc />
        public async Task<ProcessResult> Handle(InvokedEvent message, CancellationToken cancellationToken)
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
            await producer.Send(notifications
                .Select(mapper.Map<RealtimeNotification>)
                .Select(n => (string.Empty, n)), CancellationToken.None);

            return ProcessResult.Success;
        }
    }
}