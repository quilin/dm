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
using Microsoft.Extensions.Options;

namespace DM.Services.Notifications.Consumer.Implementation
{
    /// <inheritdoc />
    public class NotificationMessageProcessor : IMessageProcessor<InvokedEvent>
    {
        private readonly IEnumerable<INotificationGenerator> generators;
        private readonly INotificationRepository repository;
        private readonly MessagePublishConfiguration publishConfiguration;
        private readonly IMessagePublisher publisher;

        /// <inheritdoc />
        public NotificationMessageProcessor(
            IEnumerable<INotificationGenerator> generators,
            INotificationRepository repository,
            IOptions<MessagePublishConfiguration> publishConfiguration,
            IMessagePublisher publisher)
        {
            this.generators = generators;
            this.repository = repository;
            this.publishConfiguration = publishConfiguration.Value;
            this.publisher = publisher;
        }
        
        /// <inheritdoc />
        public async Task<ProcessResult> Process(InvokedEvent message)
        {
            var notifications = (await generators
                    .Where(g => g.CanResolve(message.Type))
                    .SelectManyAsync(async g => await g.Generate(message.EntityId)))
                .ToArray();

            await repository.Create(notifications.Select(n => n.notification));
            await publisher.Publish(notifications.Select(n => n.userNotification), publishConfiguration, string.Empty);

            return ProcessResult.Success;
        }
    }
}