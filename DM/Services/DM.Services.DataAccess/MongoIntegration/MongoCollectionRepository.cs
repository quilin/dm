using MongoDB.Driver;

namespace DM.Services.DataAccess.MongoIntegration
{
    public abstract class MongoCollectionRepository<TEntity> : MongoRepository
        where TEntity : class
    {
        protected MongoCollectionRepository(DmMongoClient client) : base(client)
        {
        }

        protected static FilterDefinitionBuilder<TEntity> Filter => Filter<TEntity>();
        protected static UpdateDefinitionBuilder<TEntity> Update => Update<TEntity>();
        protected static SortDefinitionBuilder<TEntity> Sort => Sort<TEntity>();
        protected static ProjectionDefinitionBuilder<TEntity> Select => Select<TEntity>();

        protected IMongoCollection<TEntity> Collection => Collection<TEntity>();
    }
}