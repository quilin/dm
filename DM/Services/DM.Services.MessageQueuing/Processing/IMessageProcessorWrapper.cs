using System.Threading.Tasks;

namespace DM.Services.MessageQueuing.Processing
{
    public interface IMessageProcessorWrapper<in TMessage>
    {
        Task<ProcessResult> ProcessWithCorrelation(TMessage message, string correlationToken);
    }
}