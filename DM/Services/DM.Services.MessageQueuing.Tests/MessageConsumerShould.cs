using System;
using System.Collections.Generic;
using DM.Services.Core.Implementation;
using DM.Services.MessageQueuing.Configuration;
using DM.Services.MessageQueuing.Consume;
using DM.Tests.Core;
using Moq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Xunit;

namespace DM.Services.MessageQueuing.Tests
{
    public class MessageConsumerShould : UnitTestBase
    {
        private readonly Mock<IModel> channel;
        private readonly MessageConsumer<TestMessage> consumer;

        public MessageConsumerShould()
        {
            var connectionFactory = Mock<IConnectionFactory>();
            var connection = Mock<IConnection>();
            channel = Mock<IModel>();
            channel.Setup(c => c.QueueDeclare(
                It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(),
                It.IsAny<IDictionary<string, object>>())).Returns((QueueDeclareOk) null);
            channel.Setup(c => c.QueueBind(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>()));

            connectionFactory.Setup(f => f.CreateConnection()).Returns(connection.Object);
            connection.Setup(c => c.CreateModel()).Returns(channel.Object);
            connection.Setup(c => c.Close());

            var guidFactory = Mock<IGuidFactory>();
            guidFactory.Setup(f => f.Create()).Returns(Guid.NewGuid);

            consumer = new MessageConsumer<TestMessage>(
                connectionFactory.Object, guidFactory.Object, null);
        }

        [Fact]
        public void SubscribeToChannel()
        {
            var queueArguments = new Dictionary<string, object>();
            var configuration = new MessageConsumeConfiguration
            {
                QueueName = "queue.name",
                RoutingKeys = new[] {"routing.key.1", "routing.key.2"},
                ExchangeName = "exchange.name",
                Arguments = queueArguments,
                ConsumerTag = "consumerTag"
            };
            channel
                .Setup(c => c.BasicConsume(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<string>(),
                    It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<IDictionary<string, object>>(),
                    It.IsAny<IBasicConsumer>()))
                .Returns("consumer.tag");
            consumer.Consume(configuration);

            channel.Verify(c => c
                .BasicConsume(
                    "queue.name", false,
                    It.Is<string>(s => s.StartsWith("consumerTag")),
                    false, false, null,
                    It.IsAny<EventingBasicConsumer>()), Times.Once);
            channel.Verify(c => c.QueueDeclare("queue.name", true, false, false, queueArguments), Times.Once);
            channel.Verify(c => c.ExchangeDeclare("exchange.name", "topic", true, false, null));
            channel.Verify(c => c.QueueBind("queue.name", "exchange.name", "routing.key.1", null), Times.Once);
            channel.Verify(c => c.QueueBind("queue.name", "exchange.name", "routing.key.2", null), Times.Once);
            channel.Verify(c => c.BasicQos(0, 0, true), Times.Once);
            channel.VerifyNoOtherCalls();
        }

        [Fact]
        public void CloseConnectionsOnDisposition()
        {
            channel.Setup(c => c.BasicCancel(It.IsAny<string>()));
            channel.Setup(c => c.Close());
            consumer.Dispose();
        }
    }
}