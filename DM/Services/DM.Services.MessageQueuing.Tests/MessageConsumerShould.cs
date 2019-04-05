using System.Collections.Generic;
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
            connectionFactory.Setup(f => f.CreateConnection()).Returns(connection.Object);
            connection.Setup(c => c.CreateModel()).Returns(channel.Object);
            connection.Setup(c => c.Close());
            consumer = new MessageConsumer<TestMessage>(
                connectionFactory.Object, null);
        }

        [Fact]
        public void SubscribeToChannel()
        {
            var configuration = new MessageConsumeConfiguration {QueueName = "queue.name"};
            channel
                .Setup(c => c.BasicConsume(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<string>(),
                    It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<IDictionary<string, object>>(),
                    It.IsAny<IBasicConsumer>()))
                .Returns("consumer.tag");
            consumer.Consume(configuration);

            channel.Verify(c => c
                .BasicConsume("queue.name", false, "", false, false, null,
                    It.IsAny<EventingBasicConsumer>()), Times.Once);
            channel.VerifyNoOtherCalls();
        }

        [Fact]
        public void CloseConnectionsOnDisposition()
        {
            channel.Setup(c => c.Close());
            consumer.Dispose();
        }
    }
}