using System;
using System.Threading.Tasks;
using DM.Services.Core.Implementation;
using DM.Services.Core.Implementation.CorrelationToken;
using DM.Services.MessageQueuing.Processing;
using DM.Tests.Core;
using FluentAssertions;
using Moq;
using Xunit;

namespace DM.Services.MessageQueuing.Tests
{
    public class MessageProcessorWrapperShould : UnitTestBase
    {
        private readonly MessageProcessorWrapper<TestMessage> messageProcessorWrapper;
        private readonly Mock<IGuidFactory> guidFactory;
        private readonly Mock<ICorrelationTokenSetter> tokenSetter;
        private readonly Mock<IMessageProcessor<TestMessage>> messageProcessor;

        public MessageProcessorWrapperShould()
        {
            tokenSetter = Mock<ICorrelationTokenSetter>();
            tokenSetter.SetupSet(s => s.Current = It.IsAny<Guid>());

            guidFactory = Mock<IGuidFactory>();
            messageProcessor = Mock<IMessageProcessor<TestMessage>>();
            messageProcessorWrapper = new MessageProcessorWrapper<TestMessage>(
                tokenSetter.Object, guidFactory.Object, messageProcessor.Object);
        }

        [Fact]
        public async Task CreateNewCorrelationTokenIfPassedTokenIsInvalid()
        {
            var message = new TestMessage();
            var invalidToken = "some invalid boi";
            var correlationId = Guid.NewGuid();

            var expected = ProcessResult.Success;
            guidFactory.Setup(f => f.Create()).Returns(correlationId);
            messageProcessor
                .Setup(p => p.Process(It.IsAny<TestMessage>()))
                .ReturnsAsync(expected);

            var actual = await messageProcessorWrapper.ProcessWithCorrelation(message, invalidToken);

            actual.Should().Be(expected);
            tokenSetter.VerifySet(s => s.Current = correlationId);
            tokenSetter.VerifyNoOtherCalls();
            messageProcessor.Verify(p => p.Process(message), Times.Once);
            messageProcessor.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task UsePassedCorrelationTokenIfValid()
        {
            var message = new TestMessage();
            var validToken = "3fbb16c2-4655-44db-a1f4-b45b729f61a7";

            var expected = ProcessResult.Success;
            messageProcessor
                .Setup(p => p.Process(It.IsAny<TestMessage>()))
                .ReturnsAsync(expected);

            var actual = await messageProcessorWrapper.ProcessWithCorrelation(message, validToken);

            actual.Should().Be(expected);
            tokenSetter.VerifySet(s => s.Current = Guid.Parse("3fbb16c2-4655-44db-a1f4-b45b729f61a7"));
            tokenSetter.VerifyNoOtherCalls();
            messageProcessor.Verify(p => p.Process(message), Times.Once);
            messageProcessor.VerifyNoOtherCalls();
        }

        public class TestMessage
        {
            public Guid SomeId { get; set; }
            public string SomeText { get; set; }
        }
    }
}