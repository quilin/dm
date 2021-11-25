using System.Threading.Tasks;
using DM.Services.MessageQueuing.RabbitMq.Configuration;
using DM.Services.MessageQueuing.RabbitMq.Connection;

namespace DM.Services.MessageQueuing.RabbitMq
{
    internal class Topology : ITopology
    {
        private readonly IConnectionPool connectionPool;

        public Topology(
            IConnectionPool connectionPool)
        {
            this.connectionPool = connectionPool;
        } 
        
        public Task Create(RabbitConsumerParameters parameters)
        {
            using var channelAdapter = connectionPool.Get();
            Ensure.Consume(channelAdapter.GetChannel(), parameters);
            return Task.CompletedTask;
        }

        public Task Create(RabbitProducerParameters parameters)
        {
            using var channelAdapter = connectionPool.Get();
            Ensure.Produce(channelAdapter.GetChannel(), parameters);
            return Task.CompletedTask;
        }
    }
}