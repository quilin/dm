using System.Threading.Tasks;
using DM.Services.MessageQueuing.Configuration;

namespace DM.Services.MessageQueuing
{
    public interface IMessagePublisher
    {
        Task Publish<TMessage>(TMessage message, MessagePublishConfiguration configuration) where TMessage : class;
    }
}