using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Extensions;
using DM.Services.MessageQueuing.Configuration;
using DM.Services.MessageQueuing.Dto;

namespace DM.Services.MessageQueuing.Publish
{
    /// <inheritdoc />
    internal class InvokedEventPublisher : IInvokedEventPublisher
    {
        private readonly IMessagePublisher messagePublisher;
        private readonly MessagePublishConfiguration configuration;

        /// <inheritdoc />
        public InvokedEventPublisher(
            IMessagePublisher messagePublisher)
        {
            this.messagePublisher = messagePublisher;
            configuration = new MessagePublishConfiguration {ExchangeName = InvokedEventsTransport.ExchangeName};
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
            var field = eventType.GetType().GetField(Enum.GetName(eventType.GetType(), eventType));
            var attribute = Attribute.GetCustomAttribute(field, typeof(EventRoutingKeyAttribute));
            var eventRoutingKeyAttribute = (EventRoutingKeyAttribute) attribute;
            return eventRoutingKeyAttribute.RoutingKey;
        }
    }
}