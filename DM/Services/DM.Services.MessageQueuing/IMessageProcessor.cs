using System.Threading.Tasks;

namespace DM.Services.MessageQueuing
{
    public interface IMessageProcessor<in TMessage>
    {
        Task<ProcessResult> Process(TMessage message);
    }
}