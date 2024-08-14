using MongoDB.Driver;

namespace DM.Services.DataAccess.MongoIntegration;

/// <summary>
/// Base Mongo entity repository
/// </summary>
public abstract class MongoRepository
{
    private readonly DmMongoClient client;

    /// <inheritdoc />
    protected MongoRepository(DmMongoClient client)
    {
        this.client = client;
    }

    /// <summary>
    /// Create filter definition builder
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    protected static FilterDefinitionBuilder<TEntity> Filter<TEntity>() => new();

    /// <summary>
    /// Create update definition builder
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    protected static UpdateDefinitionBuilder<TEntity> Update<TEntity>() => new();

    /// <summary>
    /// Create sort definition builder
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    protected static SortDefinitionBuilder<TEntity> Sort<TEntity>() => new();

    /// <summary>
    /// Create projection definition builder
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    protected static ProjectionDefinitionBuilder<TEntity> Project<TEntity>() =>
        new();

    /// <summary>
    /// Get collection
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    protected IMongoCollection<TEntity> Collection<TEntity>() => client.GetCollection<TEntity>();
}