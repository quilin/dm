using System;

namespace DM.Services.MessageQueuing.Experiment.RabbitMq.Connection.Adapters
{
    /// <summary>
    /// В данном подключении больше нельзя создавать каналов
    /// </summary>
    public class ConnectionChannelsExceededException : Exception
    {
        internal ConnectionChannelsExceededException(int maxChannelsCount)
            : base($"Нельзя создать больше {maxChannelsCount} каналов в соединении")
        {
        }
    }
}