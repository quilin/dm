using MongoDB.Driver;

namespace DM.Services.DataAccess.MongoIntegration
{
    public abstract class MongoRepository<TEntity> where TEntity : class
    {
        private readonly DmMongoClient client;

        protected MongoRepository(DmMongoClient client)
        {
            this.client = client;
        }

        protected static FilterDefinitionBuilder<TEntity> Filter => new FilterDefinitionBuilder<TEntity>();
        protected static UpdateDefinitionBuilder<TEntity> Update => new UpdateDefinitionBuilder<TEntity>();
        protected static SortDefinitionBuilder<TEntity> Sort => new SortDefinitionBuilder<TEntity>();

        protected IMongoCollection<TEntity> Collection => client.GetCollection<TEntity>();
    }
}