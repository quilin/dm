namespace DM.Services.MessageQueuing.RabbitMq.Configuration
{
    /// <summary>
    /// Параметры подключения к RabbitMq
    /// </summary>
    public class RabbitMqConfiguration
    {
        /// <summary>
        /// protocol://host:port
        /// </summary>
        public string Endpoint { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; }
    }
}