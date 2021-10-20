using System;
using System.Collections.Generic;
using DM.Services.Core.Extensions;
using DM.Services.MessageQueuing.Experiment.RabbitMq.Configuration;
using RabbitMQ.Client;
using ExchangeType = RabbitMQ.Client.ExchangeType;

namespace DM.Services.MessageQueuing.Experiment.RabbitMq
{
    internal static class Ensure
    {
        public static QueueDeclareOk Consume(IModel channel, RabbitConsumerParameters parameters)
        {
            channel.ExchangeDeclare(parameters.ExchangeName, parameters.ExchangeType.GetDescription(), true);

            var queueArgs = new Dictionary<string, object>();
            var queueName = parameters.Exclusive
                ? $"{parameters.QueueName}_{Guid.NewGuid()}"
                : parameters.QueueName;

            if (parameters.WithDeadLetterExchange)
            {
                var dlxName = $"{parameters.QueueName}-dlx";
                channel.ExchangeDeclare(dlxName, ExchangeType.Fanout, true);
                var dlqName = $"{parameters.QueueName}-dlq";
                channel.QueueDeclare(dlqName, true, false, false);
                channel.QueueBind(dlqName, dlxName, string.Empty);

                queueArgs.Add("x-dead-letter-exchange", dlxName);
                queueArgs.Add("x-dead-letter-routing-key", queueName);
            }

            var result = channel.QueueDeclare(queueName, true, parameters.Exclusive, false, queueArgs);
            foreach (var routingKey in parameters.RoutingKeys)
            {
                channel.QueueBind(queueName, parameters.ExchangeName, routingKey);
            }

            if (parameters.ProcessingOrder != ProcessingOrder.Unmanaged)
            {
                channel.BasicQos(0, parameters.ProcessingOrder.Value, true);
            }

            return result;
        }

        public static void Produce(IModel channel, RabbitProducerParameters parameters)
        {
            channel.ExchangeDeclare(parameters.ExchangeName, parameters.ExchangeType.GetDescription(), true);
        }
    }
}