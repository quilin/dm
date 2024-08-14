using MongoDB.Driver;

namespace DM.Services.DataAccess.MongoIntegration;

/// <summary>
/// Repository for Mongo collection
/// </summary>
/// <typeparam name="TEntity">Entity type</typeparam>
public abstract class MongoCollectionRepository<TEntity> : MongoRepository
    where TEntity : class
{
    /// <inheritdoc />
    protected MongoCollectionRepository(DmMongoClient client) : base(client)
    {
    }

    /// <summary>
    /// Typed filter definition builder
    /// </summary>
    protected static FilterDefinitionBuilder<TEntity> Filter => Filter<TEntity>();

    /// <summary>
    /// Typed update definition builder
    /// </summary>
    protected static UpdateDefinitionBuilder<TEntity> Update => Update<TEntity>();

    /// <summary>
    /// Typed sort definition builder
    /// </summary>
    protected static SortDefinitionBuilder<TEntity> Sort => Sort<TEntity>();

    /// <summary>
    /// Typed projection definition builder
    /// </summary>
    protected static ProjectionDefinitionBuilder<TEntity> Project => Project<TEntity>();

    /// <summary>
    /// Typed collection
    /// </summary>
    protected IMongoCollection<TEntity> Collection => Collection<TEntity>();
}