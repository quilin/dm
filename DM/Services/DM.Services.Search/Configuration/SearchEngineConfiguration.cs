namespace DM.Services.Search.Configuration
{
    /// <summary>
    /// Search engine configuration
    /// </summary>
    public class SearchEngineConfiguration
    {
        /// <summary>
        /// Searchable entity index name
        /// </summary>
        public string IndexName { get; set; }

        /// <summary>
        /// Searchable entity type name
        /// </summary>
        public string TypeName { get; set; }
    }
}