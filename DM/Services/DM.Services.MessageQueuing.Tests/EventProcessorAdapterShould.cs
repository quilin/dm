using System;
using System.Text;
using System.Threading.Tasks;
using DM.Services.MessageQueuing.Consume;
using DM.Services.MessageQueuing.Processing;
using DM.Tests.Core;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Language.Flow;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Framing;
using Xunit;

namespace DM.Services.MessageQueuing.Tests
{
    public class EventProcessorAdapterShould : UnitTestBase
    {
        private readonly Mock<IMessageProcessorWrapper<TestMessage>> processor;
        private readonly Mock<IModel> channel;
        private readonly ISetup<IMessageProcessorWrapper<TestMessage>, Task<ProcessResult>> processSetup;
        private readonly EventProcessorAdapter<TestMessage> processorAdapter;

        public EventProcessorAdapterShould()
        {
            channel = Mock<IModel>(MockBehavior.Loose);
            channel.Setup(c => c.BasicAck(It.IsAny<ulong>(), It.IsAny<bool>()));
            channel.Setup(c => c.BasicNack(It.IsAny<ulong>(), It.IsAny<bool>(), It.IsAny<bool>()));
            channel.Setup(c => c.BasicReject(It.IsAny<ulong>(), It.IsAny<bool>()));

            processor = Mock<IMessageProcessorWrapper<TestMessage>>(MockBehavior.Loose);
            processSetup = processor.Setup(p => p.ProcessWithCorrelation(It.IsAny<TestMessage>(), It.IsAny<string>()));

            var logger = Mock<ILogger<IMessageProcessorWrapper<TestMessage>>>(MockBehavior.Loose);
            logger.Setup(l => l.Log(
                It.IsAny<LogLevel>(), It.IsAny<EventId>(), It.IsAny<object>(),
                It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()));
            processorAdapter = new EventProcessorAdapter<TestMessage>(() => processor.Object, logger.Object);
        }

        [Fact]
        public async Task NackIfMessageCannotBeDeserialized()
        {
            var basicDeliverEventArgs = new BasicDeliverEventArgs
            {
                Body = Encoding.UTF8.GetBytes("{\"not\":\"test message\""),
                DeliveryTag = 12
            };
            await processorAdapter.ProcessEvent(basicDeliverEventArgs, channel.Object);

            channel.Verify(c => c.BasicNack(12, false, false), Times.Once);
            channel.VerifyNoOtherCalls();
            processor.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ProcessWithDeserializedMessageAndPassedCorrelationToken()
        {
            var basicDeliverEventArgs = new BasicDeliverEventArgs
            {
                Body = Encoding.UTF8.GetBytes("{\"id\":\"ddc9186e-b48f-4298-942a-9f45e3d976e6\"}"),
                DeliveryTag = 45,
                BasicProperties = new BasicProperties
                {
                    CorrelationId = "correlationId"
                }
            };
            processSetup.ReturnsAsync(ProcessResult.Success);
            await processorAdapter.ProcessEvent(basicDeliverEventArgs, channel.Object);

            processor.Verify(p => p.ProcessWithCorrelation(
                    It.Is<TestMessage>(m => m.Id == Guid.Parse("ddc9186e-b48f-4298-942a-9f45e3d976e6")),
                    "correlationId"),
                Times.Once);
            processor.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task AckIfSuccessfullyProcessed()
        {
            var basicDeliverEventArgs = new BasicDeliverEventArgs
            {
                Body = Encoding.UTF8.GetBytes("{\"id\":\"ddc9186e-b48f-4298-942a-9f45e3d976e6\"}"),
                DeliveryTag = 45,
                BasicProperties = new BasicProperties
                {
                    CorrelationId = "correlationId"
                }
            };
            processSetup.ReturnsAsync(ProcessResult.Success);

            await processorAdapter.ProcessEvent(basicDeliverEventArgs, channel.Object);

            channel.Verify(c => c.BasicAck(45, false), Times.Once);
            channel.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task NackWithRequeueIfProcessedAccordingly()
        {
            var basicDeliverEventArgs = new BasicDeliverEventArgs
            {
                Body = Encoding.UTF8.GetBytes("{\"id\":\"ddc9186e-b48f-4298-942a-9f45e3d976e6\"}"),
                DeliveryTag = 45,
                BasicProperties = new BasicProperties
                {
                    CorrelationId = "correlationId"
                }
            };
            processSetup.ReturnsAsync(ProcessResult.RetryNeeded);

            await processorAdapter.ProcessEvent(basicDeliverEventArgs, channel.Object);

            channel.Verify(c => c.BasicNack(45, false, true), Times.Once);
            channel.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task NackWithoutRequeueIfProcessingFailed()
        {
            var basicDeliverEventArgs = new BasicDeliverEventArgs
            {
                Body = Encoding.UTF8.GetBytes("{\"id\":\"ddc9186e-b48f-4298-942a-9f45e3d976e6\"}"),
                DeliveryTag = 45,
                BasicProperties = new BasicProperties
                {
                    CorrelationId = "correlationId"
                }
            };
            processSetup.ReturnsAsync(ProcessResult.Fail);

            await processorAdapter.ProcessEvent(basicDeliverEventArgs, channel.Object);

            channel.Verify(c => c.BasicNack(45, false, false), Times.Once);
            channel.VerifyNoOtherCalls();
        }
    }
}