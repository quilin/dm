using System;
using System.Text.Json;
using System.Threading.Tasks;
using DM.Services.MessageQueuing.Processing;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DM.Services.MessageQueuing.Consume
{
    /// <inheritdoc />
    public class EventProcessorAdapter<TMessage> : IEventProcessorAdapter<TMessage>
        where TMessage : class
    {
        private readonly Func<IMessageProcessorWrapper<TMessage>> processorFactory;
        private readonly ILogger<EventProcessorAdapter<TMessage>> logger;

        /// <inheritdoc />
        public EventProcessorAdapter(
            Func<IMessageProcessorWrapper<TMessage>> processorFactory,
            ILogger<EventProcessorAdapter<TMessage>> logger)
        {
            this.processorFactory = processorFactory;
            this.logger = logger;
        }

        /// <inheritdoc />
        public async Task ProcessEvent(BasicDeliverEventArgs eventArgs, IModel channel)
        {
            try
            {
                var message = JsonSerializer.Deserialize<TMessage>(
                    eventArgs.Body.ToArray(), SerializationSettings.ForMessage);
                var processor = processorFactory();
                var result = await processor.ProcessWithCorrelation(message, eventArgs.BasicProperties.CorrelationId);
                switch (result)
                {
                    case ProcessResult.Success:
                        channel.BasicAck(eventArgs.DeliveryTag, false);
                        break;
                    case ProcessResult.RetryNeeded:
                        channel.BasicNack(eventArgs.DeliveryTag, false, true);
                        break;
                    case ProcessResult.Fail:
                        channel.BasicNack(eventArgs.DeliveryTag, false, false);
                        break;
                    default:
                        channel.BasicReject(eventArgs.DeliveryTag, false);
                        break;
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "Ошибка при обработке сообщения");
                channel.BasicNack(eventArgs.DeliveryTag, false, false);
            }
        }
    }
}