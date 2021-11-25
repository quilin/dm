using System.Threading.Tasks;
using DM.Services.MessageQueuing.RabbitMq.Configuration;

namespace DM.Services.MessageQueuing.RabbitMq
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