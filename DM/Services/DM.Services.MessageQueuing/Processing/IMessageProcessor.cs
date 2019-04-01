using System.Threading.Tasks;

namespace DM.Services.MessageQueuing.Processing
{
    public interface IMessageProcessor<in TMessage>
    {
        Task<ProcessResult> Process(TMessage message);
    }
}