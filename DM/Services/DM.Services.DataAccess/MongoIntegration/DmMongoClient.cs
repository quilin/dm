using System;
using System.Reflection;
using DM.Services.Core.Configuration;
using DM.Services.Core.Extensions;
using Microsoft.Extensions.Options;
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
    public DmMongoClient(IOptions<ConnectionStrings> options) : base(options.Value.Mongo)
    {
        var uri = new Uri(options.Value.Mongo);
        databaseName = uri.AbsolutePath.Trim('/');
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