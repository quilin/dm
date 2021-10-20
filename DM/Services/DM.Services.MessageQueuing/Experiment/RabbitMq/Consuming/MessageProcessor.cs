using System;
using System.Threading;
using System.Threading.Tasks;
using Autofac.Features.OwnedInstances;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DM.Services.MessageQueuing.Experiment.RabbitMq.Consuming
{
    internal class MessageProcessor<TMessage, THandler> : IMessageProcessor<TMessage, THandler>
        where THandler : IMessageHandler<TMessage>
    {
        private readonly Func<Owned<IConsumerStrategy<TMessage, THandler>>> strategyFactory;

        public MessageProcessor(
            Func<Owned<IConsumerStrategy<TMessage, THandler>>> strategyFactory)
        {
            this.strategyFactory = strategyFactory;
        }

        public async Task Process(
            CountdownEvent countdownEvent,
            Func<IModel> channelAccessor,
            CancellationTokenSource cancellationTokenSource,
            BasicDeliverEventArgs deliverArguments,
            ILogger logger)
        {
            if (cancellationTokenSource.IsCancellationRequested)
            {
                logger.LogWarning("Консюмер просигналил об окончании работы, но сообщения продолжают поступать");
                return;
            }

            if (!SafeIncrement(countdownEvent, logger))
            {
                channelAccessor().BasicNack(deliverArguments.DeliveryTag, multiple: false, requeue: true);
                return;
            }

            try
            {
                ProcessResult result;
                using (var strategy = strategyFactory())
                {
                    result = await strategy.Value.Execute(deliverArguments, cancellationTokenSource.Token);
                }

                var channel = channelAccessor();
                switch (result)
                {
                    case ProcessResult.Success:
                        channel.BasicAck(deliverArguments.DeliveryTag, multiple: false);
                        break;
                    case ProcessResult.RetryNeeded:
                        channel.BasicNack(deliverArguments.DeliveryTag, multiple: false, requeue: true);
                        break;
                    case ProcessResult.Fail:
                        channel.BasicNack(deliverArguments.DeliveryTag, multiple: false, requeue: false);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "Ошибка при обработке сообщения из очереди ({correlationId})",
                    deliverArguments.BasicProperties.CorrelationId);
                channelAccessor().BasicNack(deliverArguments.DeliveryTag, multiple: false, requeue: false);
            }
            finally
            {
                SafeSignal(countdownEvent, logger);
            }
        }

        private static bool SafeIncrement(CountdownEvent countdownEvent, ILogger logger)
        {
            try
            {
                return countdownEvent.TryAddCount(1);
            }
            catch (Exception e)
            {
                logger.LogCritical(e, "Ошибка при попытке увеличить счетчик обратного отсчета при получении сообщения");
                return false;
            }
        }

        private static void SafeSignal(CountdownEvent countdownEvent, ILogger logger)
        {
            try
            {
                countdownEvent.Signal();
            }
            catch (Exception e)
            {
                logger.LogCritical(e, "Ошибка при попытке просигналить в обратный отсчет после завершения работы");
            }
        }
    }
}