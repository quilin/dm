using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Extensions;
using DM.Services.MessageQueuing.Configuration;
using DM.Services.MessageQueuing.Dto;
using Microsoft.Extensions.Options;

namespace DM.Services.MessageQueuing.Publish
{
    /// <inheritdoc />
    public class InvokedEventPublisher : IInvokedEventPublisher
    {
        private readonly IMessagePublisher messagePublisher;
        private readonly IMessagePublishConfiguration configuration;

        /// <inheritdoc />
        public InvokedEventPublisher(
            IMessagePublisher messagePublisher,
            IOptions<DmEventPublishConfiguration> configuration)
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

        /// <inheritdoc />
        public Task Publish(IEnumerable<EventType> types, Guid entityId)
        {
            return messagePublisher.Publish(types.Select(type => (new InvokedEvent
            {
                Type = type,
                EntityId = entityId
            }, GetRoutingKey(type))), configuration);
        }

        private static string GetRoutingKey(EventType eventType)
        {
            var type = eventType.GetType();
            var name = Enum.GetName(type, eventType);
            var field = type.GetField(name);
            var attribute = (EventRoutingKeyAttribute) Attribute.GetCustomAttribute(field, typeof(EventRoutingKeyAttribute));
            return attribute.RoutingKey;
        }
    }
}