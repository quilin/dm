using System;
using System.Text;
using System.Threading.Tasks;
using DM.Services.MessageQueuing.Processing;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DM.Services.MessageQueuing.Consume
{
    /// <inheritdoc />
    public class EventProcessorAdapter<TMessage> : IEventProcessorAdapter<TMessage>
        where TMessage : class
    {
        private readonly Func<IMessageProcessorWrapper<TMessage>> processorFactory;

        /// <inheritdoc />
        public EventProcessorAdapter(
            Func<IMessageProcessorWrapper<TMessage>> processorFactory)
        {
            this.processorFactory = processorFactory;
        }
        
        /// <inheritdoc />
        public async Task ProcessEvent(BasicDeliverEventArgs eventArgs, IModel channel)
        {
            try
            {
                var message = JsonConvert.DeserializeObject<TMessage>(Encoding.UTF8.GetString(eventArgs.Body));
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
            catch
            {
                channel.BasicNack(eventArgs.DeliveryTag, false, false);
            }
        }
    }
}