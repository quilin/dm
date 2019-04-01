using DM.Services.MessageQueuing.Configuration;

namespace DM.Services.MessageQueuing.Consume
{
    // ReSharper disable once UnusedTypeParameter
    public interface IMessageConsumer<in TMessage> where TMessage : class
    {
        void Consume(MessageConsumeConfiguration configuration);
    }
}