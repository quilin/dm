using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Core.Extensions;
using DM.Services.MessageQueuing;
using DM.Services.MessageQueuing.Configuration;
using DM.Services.MessageQueuing.Dto;
using DM.Services.MessageQueuing.Processing;
using DM.Services.MessageQueuing.Publish;
using DM.Services.Notifications.Consumer.Implementation.Notifiers;

namespace DM.Services.Notifications.Consumer.Implementation
{
    /// <inheritdoc />
    public class NotificationMessageProcessor : IMessageProcessor<InvokedEvent>
    {
        private readonly IEnumerable<INotificationGenerator> generators;
        private readonly INotificationRepository repository;
        private readonly IMessagePublisher publisher;

        /// <inheritdoc />
        public NotificationMessageProcessor(
            IEnumerable<INotificationGenerator> generators,
            INotificationRepository repository,
            IMessagePublisher publisher)
        {
            this.generators = generators;
            this.repository = repository;
            this.publisher = publisher;
        }

        /// <inheritdoc />
        public async Task<ProcessResult> Process(InvokedEvent message)
        {
            var notifications = (await generators
                    .Where(g => g.CanResolve(message.Type))
                    .SelectManyAsync(async g => await g.Generate(message.EntityId)))
                .ToArray();

            if (!notifications.Any())
            {
                return ProcessResult.Success;
            }

            await repository.Create(notifications.Select(n => n.notification));
            await publisher.Publish(
                notifications.Select(n => (n.userNotification, string.Empty)),
                new MessagePublishConfiguration
                {
                    ExchangeName = "dm.notifications.sent"
                });

            return ProcessResult.Success;
        }
    }
}