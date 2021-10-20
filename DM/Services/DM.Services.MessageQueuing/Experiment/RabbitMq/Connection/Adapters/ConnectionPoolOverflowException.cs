using System;

namespace DM.Services.MessageQueuing.Experiment.RabbitMq.Connection.Adapters
{
    /// <summary>
    /// Больше нельзя создавать подключений
    /// </summary>
    public class ConnectionPoolOverflowException : Exception
    {
        internal ConnectionPoolOverflowException()
            : base("Пул подключений переполнен, новые подключения невозможны")
        {
        }
    }
}