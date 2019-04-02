using System;
using System.Threading.Tasks;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Extensions;
using DM.Services.DataAccess.Eventing;
using DM.Services.MessageQueuing.Configuration;
using DM.Services.MessageQueuing.Publish;
using Microsoft.Extensions.Options;

namespace DM.Services.Common.Implementation
{
    /// <inheritdoc />
    public class InvokedEventPublisher : IInvokedEventPublisher
    {
        private readonly IMessagePublisher messagePublisher;
        private readonly MessagePublishConfiguration configuration;

        /// <inheritdoc />
        public InvokedEventPublisher(
            IMessagePublisher messagePublisher,
            IOptions<MessagePublishConfiguration> configuration)
        {
            this.messagePublisher = messagePublisher;
            this.configuration = configuration.Value;
        }

        /// <inheritdoc />
        public Task Publish(EventType type, Guid entityId)
        {
            return messagePublisher.Publish(new InvokedEvent
            {
                Type = type,
                EntityId = entityId
            }, configuration, GetRoutingKey(type));
        }

        private static string GetRoutingKey(EventType eventType)
        {
            var type = eventType.GetType();
            var name = Enum.GetName(type, eventType);
            var field = type.GetField(name);
            var attribute = (EventRoutingKeyAttribute)Attribute.GetCustomAttribute(field, typeof(EventRoutingKeyAttribute));
            return attribute.RoutingKey;
        }
    }
}