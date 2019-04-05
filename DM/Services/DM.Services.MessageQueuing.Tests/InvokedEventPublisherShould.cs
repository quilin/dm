using System;
using System.Threading.Tasks;
using DM.Services.Core.Dto.Enums;
using DM.Services.MessageQueuing.Configuration;
using DM.Services.MessageQueuing.Dto;
using DM.Services.MessageQueuing.Publish;
using DM.Tests.Core;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace DM.Services.MessageQueuing.Tests
{
    public class InvokedEventPublisherShould : UnitTestBase
    {
        private readonly InvokedEventPublisher publisherWrapper;
        private readonly Mock<IMessagePublisher> publisher;
        private readonly MessagePublishConfiguration messagePublishConfiguration;

        public InvokedEventPublisherShould()
        {
            publisher = Mock<IMessagePublisher>();
            publisher
                .Setup(p => p.Publish(
                    It.IsAny<InvokedEvent>(), It.IsAny<MessagePublishConfiguration>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);
            var options = Mock<IOptions<MessagePublishConfiguration>>();
            messagePublishConfiguration = new MessagePublishConfiguration();
            options.Setup(o => o.Value).Returns(messagePublishConfiguration);
            publisherWrapper = new InvokedEventPublisher(publisher.Object, options.Object);
        }

        [Fact]
        public async Task PublishEventWithMappedFields()
        {
            var entityId = Guid.NewGuid();

            await publisherWrapper.Publish(EventType.ChangedTopic, entityId);

            publisher.Verify(p => p.Publish(
                It.Is<InvokedEvent>(e => e.EntityId == entityId && e.Type == EventType.ChangedTopic),
                messagePublishConfiguration, "changed.topic"), Times.Once);
            publisher.VerifyNoOtherCalls();
        }
    }
}