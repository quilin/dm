using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Extensions;
using DM.Services.MessageQueuing.Building;
using DM.Services.MessageQueuing.RabbitMq.Configuration;

namespace DM.Services.MessageQueuing.GeneralBus
{
    internal class InvokedEventProducer : IInvokedEventProducer
    {
        private readonly IProducer<string, InvokedEvent> producer;

        public InvokedEventProducer(
            IProducerBuilder<string, InvokedEvent> producerBuilder)
        {
            producer = producerBuilder.BuildRabbit(new RabbitProducerParameters
            {
                ExchangeName = InvokedEventsTransport.ExchangeName,
                ExchangeType = ExchangeType.Topic,
            });
        }

        public Task Send(EventType eventType, Guid entityId) =>
            producer.Send(GetRoutingKey(eventType), new InvokedEvent
            {
                Type = eventType,
                EntityId = entityId
            }, CancellationToken.None);

        public Task Send(IEnumerable<EventType> eventTypes, Guid entityId) =>
            producer.Send(eventTypes.Select(t => (GetRoutingKey(t), new InvokedEvent
            {
                Type = t,
                EntityId = entityId
            })), CancellationToken.None);

        private static string GetRoutingKey(EventType eventType)
        {
            var name = Enum.GetName(eventType.GetType(), eventType) ??
                throw new InvokedEventException($"Отсутствует имя перечисления {eventType}");
            var field = eventType.GetType().GetField(name) ??
                throw new InvokedEventException($"Не найдено поле перечисления {eventType}");
            var attribute = Attribute.GetCustomAttribute(field, typeof(EventRoutingKeyAttribute));
            return attribute is EventRoutingKeyAttribute eventRoutingKeyAttribute
                ? eventRoutingKeyAttribute.RoutingKey
                : throw new InvokedEventException($"Отсутствует аттрибут {nameof(EventRoutingKeyAttribute)}");
        }
    }
}