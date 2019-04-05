using System;
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

        public MessagePublisherShould()
        {
            var connectionFactory = Mock<IConnectionFactory>();
            var connection = Mock<IConnection>();
            channel = Mock<IModel>();
            channel.Setup(c => c.BasicPublish(It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<bool>(), It.IsAny<IBasicProperties>(), It.IsAny<byte[]>()));
            channel.Setup(c => c.Dispose());
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
                It.Is<IBasicProperties>(p =>
                    p.Persistent &&
                    p.ContentType == "application/json" &&
                    p.CorrelationId == "ed45a797-bcfe-4750-a6ed-83af87954aa6"),
                It.Is<byte[]>(b => Encoding.UTF8.GetString(b) == "{\"test\":1,\"thing\":\"2\"}")), Times.Once);
            channel.Verify(c => c.Dispose());
            channel.VerifyNoOtherCalls();
        }
    }
}