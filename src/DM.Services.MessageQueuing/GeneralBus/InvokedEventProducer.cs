using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DM.Services.Core.Dto.Enums;
using DM.Services.Core.Extensions;
using Jamq.Client.Abstractions.Producing;
using Jamq.Client.Rabbit.Producing;

namespace DM.Services.MessageQueuing.GeneralBus;

internal class InvokedEventProducer(IProducerBuilder producerBuilder) : IInvokedEventProducer
{
    private readonly IProducer<string, InvokedEvent> producer = producerBuilder.BuildRabbit<InvokedEvent>(
        new RabbitProducerParameters(InvokedEventsTransport.ExchangeName));

    public Task Send(EventType eventType, Guid entityId) =>
        producer.Send(GetRoutingKey(eventType), new InvokedEvent
        {
            Type = eventType,
            EntityId = entityId
        }, CancellationToken.None);

    public async Task Send(IEnumerable<EventType> eventTypes, Guid entityId)
    {
        foreach (var eventType in eventTypes)
        {
            await Send(eventType, entityId);
        }
    }

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