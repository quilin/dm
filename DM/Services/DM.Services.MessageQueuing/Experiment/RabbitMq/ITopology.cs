using System.Threading.Tasks;
using DM.Services.MessageQueuing.Experiment.RabbitMq.Configuration;

namespace DM.Services.MessageQueuing.Experiment.RabbitMq
{
    /// <summary>
    /// Абстракция работы с топологией RabbitMQ
    /// </summary>
    public interface ITopology
    {
        /// <summary>
        /// Создать топологию, необходимую для работы консюмера
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task Create(RabbitConsumerParameters parameters);

        /// <summary>
        /// Создать топологию, необходимую для работы продюсера
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task Create(RabbitProducerParameters parameters);
    }
}