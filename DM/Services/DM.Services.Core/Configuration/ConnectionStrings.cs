namespace DM.Services.Core.Configuration
{
    public class ConnectionStrings
    {
        public string DmDbContext { get; set; }
        public string DmMongoClient { get; set; }
        public string DmCacheClient { get; set; }
        public string DmSearchEngine { get; set; }
    }
}