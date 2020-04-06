using System;
using System.Threading.Tasks;
using DM.Services.Core.Dto.Enums;
using DM.Services.MessageQueuing.Configuration;
using DM.Services.MessageQueuing.Dto;
using DM.Services.MessageQueuing.Publish;
using DM.Tests.Core;
using Moq;
using Xunit;

namespace DM.Services.MessageQueuing.Tests
{
    public class InvokedEventPublisherShould : UnitTestBase
    {
        private readonly InvokedEventPublisher publisherWrapper;
        private readonly Mock<IMessagePublisher> publisher;

        public InvokedEventPublisherShould()
        {
            publisher = Mock<IMessagePublisher>();
            publisher
                .Setup(p => p.Publish(
                    It.IsAny<InvokedEvent>(), It.IsAny<MessagePublishConfiguration>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);
            publisherWrapper = new InvokedEventPublisher(publisher.Object);
        }

        [Fact]
        public async Task PublishEventWithMappedFields()
        {
            var entityId = Guid.NewGuid();

            await publisherWrapper.Publish(EventType.ChangedForumTopic, entityId);

            publisher.Verify(p => p.Publish(
                    It.Is<InvokedEvent>(e => e.EntityId == entityId && e.Type == EventType.ChangedForumTopic),
                    It.Is<MessagePublishConfiguration>(c => c.ExchangeName == "dm.events"),
                    "forum.topic.changed"),
                Times.Once);
            publisher.VerifyNoOtherCalls();
        }
    }
}