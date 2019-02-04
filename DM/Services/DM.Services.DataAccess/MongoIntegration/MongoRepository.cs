using MongoDB.Driver;

namespace DM.Services.DataAccess.MongoIntegration
{
    public abstract class MongoRepository
    {
        private readonly DmMongoClient client;

        protected MongoRepository(DmMongoClient client)
        {
            this.client = client;
        }

        protected static FilterDefinitionBuilder<TEntity> Filter<TEntity>() => new FilterDefinitionBuilder<TEntity>();
        protected static UpdateDefinitionBuilder<TEntity> Update<TEntity>() => new UpdateDefinitionBuilder<TEntity>();
        protected static SortDefinitionBuilder<TEntity> Sort<TEntity>() => new SortDefinitionBuilder<TEntity>();
        protected static ProjectionDefinitionBuilder<TEntity> Select<TEntity>() => new ProjectionDefinitionBuilder<TEntity>();

        protected IMongoCollection<TEntity> Collection<TEntity>() => client.GetCollection<TEntity>();
    }
}