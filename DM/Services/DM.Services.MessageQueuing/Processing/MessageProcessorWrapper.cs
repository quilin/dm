using System;
using System.Threading.Tasks;
using DM.Services.Authentication.Implementation.CorrelationToken;
using DM.Services.Core.Implementation;

namespace DM.Services.MessageQueuing.Processing
{
    public class MessageProcessorWrapper<TMessage> : IMessageProcessorWrapper<TMessage>
    {
        private readonly ICorrelationTokenSetter correlationTokenSetter;
        private readonly IGuidFactory guidFactory;
        private readonly IMessageProcessor<TMessage> processor;

        public MessageProcessorWrapper(
            ICorrelationTokenSetter correlationTokenSetter,
            IGuidFactory guidFactory,
            IMessageProcessor<TMessage> processor)
        {
            this.correlationTokenSetter = correlationTokenSetter;
            this.guidFactory = guidFactory;
            this.processor = processor;
        }

        public Task<ProcessResult> ProcessWithCorrelation(TMessage message, string correlationToken)
        {
            correlationTokenSetter.Current = Guid.TryParse(correlationToken, out var token)
                ? token
                : guidFactory.Create();
            return processor.Process(message);
        }
    }
}