using System;
using System.Reflection;
using DM.Services.Core.Extensions;
using MongoDB.Driver;

namespace DM.Services.DataAccess.MongoIntegration;

/// <summary>
/// Mongo DB client wrapper
/// </summary>
public class DmMongoClient : MongoClient
{
    private readonly string databaseName;

    private IMongoDatabase Database => GetDatabase(databaseName);

    /// <inheritdoc />
    public DmMongoClient(MongoClientSettings settings, string connectionString) : base(settings)
    {
        databaseName = new Uri(connectionString).AbsolutePath.Trim('/');
    }

    /// <summary>
    /// Get collection for entity type
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    /// <returns>Mongo collection</returns>
    public IMongoCollection<T> GetCollection<T>()
    {
        var mongoCollectionNameAttribute = typeof(T).GetCustomAttribute<MongoCollectionNameAttribute>() ??
                                           throw new AttributeNotFoundException(typeof(MongoCollectionNameAttribute));
        return Database.GetCollection<T>(mongoCollectionNameAttribute.CollectionName);
    }
}