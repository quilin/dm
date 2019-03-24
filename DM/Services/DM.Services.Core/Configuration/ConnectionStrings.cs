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
        public string DmDbContext { get; set; }

        /// <summary>
        /// Mongo connection string
        /// </summary>
        public string DmMongoClient { get; set; }

        /// <summary>
        /// Mongo in-memory connection string
        /// </summary>
        public string DmCacheClient { get; set; }

        /// <summary>
        /// Search engine connection string
        /// </summary>
        public string DmSearchEngine { get; set; }
    }
}