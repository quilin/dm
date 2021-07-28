using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DM.Services.MessageQueuing;
using DM.Services.MessageQueuing.Configuration;
using DM.Services.MessageQueuing.Dto;
using DM.Services.MessageQueuing.Processing;
using DM.Services.MessageQueuing.Publish;
using DM.Services.Notifications.BusinessProcesses.Creating;
using DM.Services.Notifications.Consumer.Implementation.Notifiers;
using DM.Services.Notifications.Dto;

namespace DM.Services.Notifications.Consumer.Implementation
{
    /// <inheritdoc />
    public class NotificationMessageProcessor : IMessageProcessor<InvokedEvent>
    {
        private readonly IEnumerable<INotificationGenerator> generators;
        private readonly INotificationCreatingService service;
        private readonly IMapper mapper;
        private readonly IMessagePublisher publisher;

        /// <inheritdoc />
        public NotificationMessageProcessor(
            IEnumerable<INotificationGenerator> generators,
            INotificationCreatingService service,
            IMapper mapper,
            IMessagePublisher publisher)
        {
            this.generators = generators;
            this.service = service;
            this.mapper = mapper;
            this.publisher = publisher;
        }

        /// <inheritdoc />
        public async Task<ProcessResult> Process(InvokedEvent message)
        {
            var notificationsToCreate = new List<CreateNotification>();
            foreach (var generator in generators.Where(g => g.CanResolve(message.Type)))
            {
                await foreach (var createNotification in generator.Generate(message.EntityId))
                {
                    notificationsToCreate.Add(createNotification with {EventType = message.Type});
                }
            }

            if (!notificationsToCreate.Any())
            {
                return ProcessResult.Success;
            }

            var notifications = await service.Create(notificationsToCreate);
            await publisher.Publish(notifications
                .Select(mapper.Map<RealtimeNotification>)
                .Select(n => (n, string.Empty)), new MessagePublishConfiguration
            {
                ExchangeName = "dm.notifications.sent"
            });

            return ProcessResult.Success;
        }
    }
}