using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DM.Services.Core.Implementation.CorrelationToken;
using DM.Services.MessageQueuing.Configuration;
using DM.Services.MessageQueuing.Publish;
using DM.Tests.Core;
using Moq;
using Moq.Language.Flow;
using RabbitMQ.Client;
using Xunit;

namespace DM.Services.MessageQueuing.Tests
{
    public class MessagePublisherShould : UnitTestBase
    {
        private readonly MessagePublisher publisher;
        private readonly ISetup<ICorrelationTokenProvider, Guid> getTokenSetup;
        private readonly Mock<IModel> channel;
        private readonly Mock<IBasicProperties> basicProperties;

        public MessagePublisherShould()
        {
            var connectionFactory = Mock<IConnectionFactory>();
            var connection = Mock<IConnection>();

            basicProperties = Mock<IBasicProperties>();
            basicProperties.SetupSet(p => p.Persistent = It.IsAny<bool>());
            basicProperties.SetupSet(p => p.ContentType = It.IsAny<string>());
            basicProperties.SetupSet(p => p.CorrelationId = It.IsAny<string>());

            channel = Mock<IModel>();
            channel.Setup(c => c.CreateBasicProperties()).Returns(basicProperties.Object);
            channel.Setup(c => c.BasicPublish(It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<bool>(), It.IsAny<IBasicProperties>(), It.IsAny<ReadOnlyMemory<byte>>()));
            channel.Setup(c => c.Dispose());
            channel.Setup(c => c.ExchangeDeclare(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(),
                It.IsAny<IDictionary<string, object>>()));
            connectionFactory.Setup(f => f.CreateConnection()).Returns(connection.Object);
            connection.Setup(c => c.CreateModel()).Returns(channel.Object);
            connection.Setup(c => c.Dispose());
            var correlationTokenProvider = Mock<ICorrelationTokenProvider>();
            getTokenSetup = correlationTokenProvider.Setup(p => p.Current);
            publisher = new MessagePublisher(connectionFactory.Object, correlationTokenProvider.Object);
        }

        [Fact]
        public async Task PublishSerializedMessage()
        {
            var messagePublishConfiguration = new MessagePublishConfiguration {ExchangeName = "exchange.name"};
            getTokenSetup.Returns(Guid.Parse("ed45a797-bcfe-4750-a6ed-83af87954aa6"));

            await publisher.Publish(new {test = 1, thing = "2"}, messagePublishConfiguration, "routing.key");

            channel.Verify(c => c.BasicPublish("exchange.name", "routing.key", false,
                basicProperties.Object,
                It.Is<ReadOnlyMemory<byte>>(b =>
                    Encoding.UTF8.GetString(b.ToArray()) == "{\"test\":1,\"thing\":\"2\"}")), Times.Once);

            basicProperties.VerifySet(p => p.Persistent = true);
            basicProperties.VerifySet(p => p.ContentType = "application/json");
            basicProperties.VerifySet(p => p.CorrelationId = "ed45a797-bcfe-4750-a6ed-83af87954aa6");

            channel.Verify(c => c.Dispose());
            channel.Verify(c => c.ExchangeDeclare("exchange.name", "topic", true, false, null));
            channel.VerifyNoOtherCalls();
        }
    }
}