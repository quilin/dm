using DM.Services.MessageQueuing.Configuration;

namespace DM.Services.MessageQueuing
{
    public interface IMessageConsumer<in TMessage> where TMessage : class
    {
        void Consume(MessageConsumeConfiguration configuration);
    }
}