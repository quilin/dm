namespace DM.Services.Core.Configuration
{
    /// <summary>
    /// Storage connection configuration
    /// </summary>
    public class ConnectionStrings
    {
        /// <summary>
        /// RDB connection string
        /// </summary>
        public string Rdb { get; set; }

        /// <summary>
        /// Mongo connection string
        /// </summary>
        public string Mongo { get; set; }

        /// <summary>
        /// Mongo in-memory connection string
        /// </summary>
        public string Cache { get; set; }

        /// <summary>
        /// Search engine connection string
        /// </summary>
        public string SearchEngine { get; set; }

        /// <summary>
        /// Message queue connection string
        /// </summary>
        public MessageQueueConnectionConfiguration MessageQueue { get; set; }

        /// <summary>
        /// Logging storage connection string
        /// </summary>
        public string Logs { get; set; }
    }

    /// <summary>
    /// Connection configuration for Message Queue endpoint
    /// </summary>
    public class MessageQueueConnectionConfiguration
    {
        /// <summary>
        /// Endpoint url
        /// </summary>
        public string Endpoint { get; set; }

        /// <summary>
        /// User name
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Virtual host name
        /// </summary>
        public string VirtualHost { get; set; }
    }
}