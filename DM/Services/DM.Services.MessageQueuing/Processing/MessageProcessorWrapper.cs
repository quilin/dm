using System;
using System.Threading.Tasks;
using DM.Services.Core.Implementation;
using DM.Services.Core.Implementation.CorrelationToken;

namespace DM.Services.MessageQueuing.Processing
{
    /// <inheritdoc />
    internal class MessageProcessorWrapper<TMessage> : IMessageProcessorWrapper<TMessage>
    {
        private readonly ICorrelationTokenSetter correlationTokenSetter;
        private readonly IGuidFactory guidFactory;
        private readonly IMessageProcessor<TMessage> processor;

        /// <inheritdoc />
        public MessageProcessorWrapper(
            ICorrelationTokenSetter correlationTokenSetter,
            IGuidFactory guidFactory,
            IMessageProcessor<TMessage> processor)
        {
            this.correlationTokenSetter = correlationTokenSetter;
            this.guidFactory = guidFactory;
            this.processor = processor;
        }

        /// <inheritdoc />
        public Task<ProcessResult> ProcessWithCorrelation(TMessage message, string correlationToken)
        {
            correlationTokenSetter.Current = Guid.TryParse(correlationToken, out var token)
                ? token
                : guidFactory.Create();
            return processor.Process(message);
        }
    }
}